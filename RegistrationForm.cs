using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class RegistrationForm : Form
    {
        private const string connectionString = @"Data Source=DESKTOP-KPFP023;Initial Catalog=daycare;Integrated Security=True;Encrypt=False";
        public string RegisteredLogin { get; private set; }
        public string RegisteredPassword { get; private set; }

        public RegistrationForm()
        {
            InitializeComponent();
            LoadParents();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword1.Text;
            string confirmPassword = textBoxPassword2.Text;

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                comboBoxParents.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int parentId = Convert.ToInt32(comboBoxParents.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("AddUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@Password", password); // Можно позже заменить на хэш
                        cmd.Parameters.AddWithValue("@Role", "Parent");
                        cmd.Parameters.AddWithValue("@ParentId", parentId);

                        cmd.ExecuteNonQuery();
                    }

                    RegisteredLogin = login;
                    RegisteredPassword = password;
                    this.DialogResult = DialogResult.OK; // <-- обязательно
                    MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelAddParents_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm("parents", "Admin");
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadParents();

                // После загрузки ищем последнего добавленного родителя по ID, полученному из AddForm
                int newParentId = addForm.LastInsertedId;
                comboBoxParents.SelectedValue = newParentId;
            }
        }


        private void LoadParents()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT p_id, p_fullName FROM Parent";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxParents.DataSource = dt;
                    comboBoxParents.DisplayMember = "p_fullName";
                    comboBoxParents.ValueMember = "p_id";
                    comboBoxParents.SelectedIndex = -1; // Сбросить выбор
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке родителей: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
