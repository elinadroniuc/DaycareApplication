using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class AuthorisationForm : Form
    {
        public string LoggedInUser { get; private set; }
        public string LoggedInRole { get; private set; }
        public int? ParentId { get; private set; }

        private const string ConnectionString = "YOUR_CONNECTION_STRING";

        public AuthorisationForm()
        {
            InitializeComponent();
#if DEBUG
            textBoxUsername.Text = "admin";
            textBoxPassword.Text = "admin123";
#endif
        }

        private void buttonAuthorisation_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                labelError.Text = "Введите логин и пароль";
                labelError.Visible = true;
                return;
            }

            if (ValidateUser(username, password, out string role, out int? parentId))
            {
                LoggedInUser = username;
                LoggedInRole = role;
                ParentId = parentId;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                labelError.Text = "Неверный логин или пароль";
                labelError.Visible = true;
                textBoxPassword.Focus();
                textBoxPassword.SelectAll();
            }
        }

        private bool ValidateUser(string username, string password, out string role, out int? parentId)
        {
            role = null;
            parentId = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT Role, parent_id FROM Users 
                                   WHERE Username = @username AND Password = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                role = reader["Role"].ToString();

                                if (!reader.IsDBNull(reader.GetOrdinal("parent_id")))
                                {
                                    parentId = reader.GetInt32(reader.GetOrdinal("parent_id"));
                                }

                                return true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                buttonAuthorisation_Click(sender, e);
            }
        }

        private void labelRegistration_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            if (registrationForm.ShowDialog() == DialogResult.OK)
            {
                textBoxUsername.Text = registrationForm.RegisteredLogin;
                textBoxPassword.Text = registrationForm.RegisteredPassword;
                textBoxPassword.Focus();
            }

        }
    }
}