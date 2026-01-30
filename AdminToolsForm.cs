using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class AdminToolsForm : Form
    {
        string connectionString = @"Data Source=DESKTOP-KPFP023;Initial Catalog=daycare;Integrated Security=True;Encrypt=False";

        public AdminToolsForm()
        {
            InitializeComponent();
            LoadUsers();
            comboBoxRole.Items.AddRange(new string[] { "Admin", "Clerk", "Accountant" }); // Только допустимые роли
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT user_id AS [ID], username AS [Логин], role AS [Роль] FROM Users";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable usersTable = new DataTable();
                        adapter.Fill(usersTable);
                        dataGridViewUsers.DataSource = usersTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveUser_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            string role = comboBoxRole.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверим, существует ли уже пользователь с таким именем
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists > 0)
                        {
                            // Обновляем
                            string updateQuery = "UPDATE Users SET password = @password, role = @role WHERE username = @username";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@username", username);
                                updateCmd.Parameters.AddWithValue("@password", password);
                                updateCmd.Parameters.AddWithValue("@role", role);
                                updateCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Пользователь обновлён.");
                        }
                        else
                        {
                            // Добавляем
                            string insertQuery = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@username", username);
                                insertCmd.Parameters.AddWithValue("@password", password);
                                insertCmd.Parameters.AddWithValue("@role", role);
                                insertCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Пользователь добавлен.");
                        }
                    }
                }

                LoadUsers();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении пользователя: " + ex.Message);
            }
        }

        private void buttonDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
                return;
            }

            int userId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["ID"].Value);
            string username = dataGridViewUsers.SelectedRows[0].Cells["Логин"].Value.ToString();

            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя '{username}'?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Users WHERE user_id = @user_id";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пользователь удалён.");
                LoadUsers();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message);
            }
        }

        private void dataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewUsers.Rows[e.RowIndex];
                textBoxUsername.Text = row.Cells["Логин"].Value.ToString();
                comboBoxRole.SelectedItem = row.Cells["Роль"].Value.ToString();
                // Пароль не загружаем по соображениям безопасности
                textBoxPassword.Text = "";
            }
        }

        private void ClearInput()
        {
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            comboBoxRole.SelectedIndex = -1;
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            string backupPath = @"C:\Backup\daycare.bak";

            string query = $@"
                BACKUP DATABASE daycare
                TO DISK = N'{backupPath}'
                WITH FORMAT, INIT, 
                NAME = 'daycare-Full Backup';";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Резервная копия успешно создана.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании резервной копии: " + ex.Message);
            }
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            string backupPath = @"C:\Backup\daycare.bak";
            string query = $@"
                ALTER DATABASE daycare SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE daycare
                FROM DISK = N'{backupPath}'
                WITH REPLACE;
                ALTER DATABASE daycare SET MULTI_USER;";

            try
            {
                string restoreConnectionString = "Data Source=DESKTOP-KPFP023;Initial Catalog=master;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(restoreConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("База данных успешно восстановлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при восстановлении базы данных: " + ex.Message);
            }
        }

        private void buttonClearInput_Click(object sender, EventArgs e)
        {
            ClearInput();
        }
    }
}
