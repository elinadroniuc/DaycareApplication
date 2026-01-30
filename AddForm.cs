using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.ReportingServices.Diagnostics.Internal;

namespace DaycareApplication
{
    public partial class AddForm : Form
    {
        private string selectedImagePath = null;
        private string defaultImagePath = Path.Combine(Application.StartupPath, "img", "default.jpg");
        private string tableMode;
        string connectionString = "YOUR_CONNECTION_STRING";
        public int LastInsertedId { get; private set; }

        public AddForm(string mode, string userRole)
        {
            InitializeComponent();
            tableMode = mode;

            if (userRole == "Parent" && mode != "applications")
            {
                MessageBox.Show("Вы можете добавлять только заявки");
                this.Close();
                return;
            }

            if (userRole == "Accountant")
            {
                MessageBox.Show("Добавление записей недоступно");
                this.Close();
                return;
            }

            AdaptFormToMode();
        }

        private void AdaptFormToMode()
        {
            foreach (Control ctrl in Controls)
            {
                if (ctrl is Label lbl && lbl.Name.StartsWith("label")) lbl.Visible = false;
                else if (ctrl is TextBox tb && tb.Name.StartsWith("textBox")) tb.Visible = false;
                else if (ctrl is MaskedTextBox mtb && (mtb.Name == "maskedInput" || mtb.Name == "maskedPhone")) mtb.Visible = false;
                else if (ctrl is DateTimePicker dtp && dtp.Name == "dateTimePicker1") dtp.Visible = false;
                else if (ctrl is ComboBox cb && (cb.Name == "comboBox1" || cb.Name == "comboBox2")) cb.Visible = false;
            }

            Label label1 = Controls["label1"] as Label;
            Label label2 = Controls["label2"] as Label;
            Label label3 = Controls["label3"] as Label;
            Label label4 = Controls["label4"] as Label;

            TextBox textBox1 = Controls["textBox1"] as TextBox;
            TextBox textBoxEmail = Controls["textBoxEmail"] as TextBox;
            MaskedTextBox maskedInput = Controls["maskedInput"] as MaskedTextBox;
            DateTimePicker dateTimePicker1 = Controls["dateTimePicker1"] as DateTimePicker;
            ComboBox comboBox1 = Controls["comboBox1"] as ComboBox;
            ComboBox comboBox2 = Controls["comboBox2"] as ComboBox;

            MaskedTextBox maskedPhone = Controls["maskedPhone"] as MaskedTextBox;
            if (maskedPhone == null)
            {
                maskedPhone = new MaskedTextBox();
                maskedPhone.Name = "maskedPhone";
                maskedPhone.Mask = "00000000000";
                maskedPhone.Location = new Point(maskedInput.Location.X, maskedInput.Location.Y + 40);
                maskedPhone.Size = maskedInput.Size;
                Controls.Add(maskedPhone);
            }
            maskedPhone.Visible = false;

            switch (tableMode)
            {
                case "parents":
                    ShowControl(label1, "Full name", textBox1);
                    ShowControl(label2, "Passport", maskedInput); maskedInput.Mask = "0000000"; maskedInput.Text = "";
                    ShowControl(label3, "Email", textBoxEmail); textBoxEmail.Text = "";
                    ShowControl(label4, "Phone number", maskedPhone); maskedPhone.Text = "";
                    break;
                case "children":
                    ShowControl(label1, "Name", textBox1);
                    ShowControl(label2, "Birthday", dateTimePicker1);
                    ShowControl(label3, "Certificate", maskedInput); maskedInput.Mask = "00000"; maskedInput.Text = "";
                    break;
                case "applications":
                    ShowControl(label1, "Child", comboBox1);
                    ShowControl(label2, "Entering date", comboBox2);
                    ShowControl(label3, "Parent", dateTimePicker1);
                    LoadChildren(); LoadParents(); LoadApplications();
                    break;
                case "appointments":
                    ShowControl(label1, "Payment", comboBox1);
                    ShowControl(label2, "Entering date", textBox1);
                    ShowControl(label3, "Application", textBoxEmail);
                    ShowControl(label4, "Rezult", dateTimePicker1);
                    LoadAppointments(); LoadApplications();
                    break;
            }

            if (tableMode == "parents")
            {
                pictureBoxPhoto.Visible = true;
                buttonSelectPhoto.Visible = true;
                buttonRemovePhoto.Visible = true;
                pictureBoxPhoto.Image = File.Exists(defaultImagePath) ? Image.FromFile(defaultImagePath) : null;
            }
        }

        private void ShowControl(Label label, string text, Control input)
        {
            label.Text = text;
            label.Visible = true;
            input.Visible = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    TextBox textBox1 = this.Controls["textBox1"] as TextBox;
                    TextBox textBoxEmail = this.Controls["textBoxEmail"] as TextBox;
                    MaskedTextBox maskedInput = this.Controls["maskedInput"] as MaskedTextBox;
                    MaskedTextBox maskedPhone = this.Controls["maskedPhone"] as MaskedTextBox;
                    DateTimePicker dateTimePicker1 = this.Controls["dateTimePicker1"] as DateTimePicker;
                    ComboBox comboBox1 = this.Controls["comboBox1"] as ComboBox;
                    ComboBox comboBox2 = this.Controls["comboBox2"] as ComboBox;

                    string newIdQuery = "";
                    string folderSubPath = "";

                    switch (tableMode)
                    {
                        case "parents":
                            if (string.IsNullOrWhiteSpace(textBox1.Text))
                                throw new Exception("ФИО обязательно!");

                            if (maskedInput.Text.Length != 7 || maskedInput.Text.Contains(" "))
                                throw new Exception("Паспорт должен содержать 7 цифр!");

                            if (maskedPhone == null || maskedPhone.Text.Length != 8 || maskedPhone.Text.Contains(" "))
                                throw new Exception("Телефон должен содержать 8 цифр!");

                            if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
                                throw new Exception("Email обязателен!");

                            command.CommandText = "AddParent";
                            command.Parameters.AddWithValue("@p_fullName", textBox1.Text.Trim());
                            command.Parameters.AddWithValue("@p_passportDetails", "P" + maskedInput.Text.Trim());
                            command.Parameters.AddWithValue("@p_email", textBoxEmail.Text.Trim() + "@gmail.com");
                            command.Parameters.AddWithValue("@p_phoneNumber", "+373" + maskedPhone.Text.Trim());

                            newIdQuery = "SELECT MAX(p_id) FROM Parent";
                            folderSubPath = "parents";
                            break;

                        case "children":
                            if (string.IsNullOrWhiteSpace(textBox1.Text))
                                throw new Exception("ФИО обязательно!");

                            if (maskedInput.Text.Length != 5 || maskedInput.Text.Contains(" "))
                                throw new Exception("Свидетельство должно содержать 5 цифр!");

                            command.CommandText = "AddChild";
                            command.Parameters.AddWithValue("@c_fullName", textBox1.Text.Trim());
                            command.Parameters.AddWithValue("@c_birthdayDate", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@c_birthCertificate", "BC" + maskedInput.Text.Trim());

                            newIdQuery = "SELECT MAX(c_id) FROM Child";
                            folderSubPath = "children";
                            break;

                        case "applications":
                            if (comboBox1.SelectedValue == null)
                                throw new Exception("Выберите ребёнка!");
                            if (comboBox2.SelectedValue == null)
                                throw new Exception("Выберите родителя!");

                            command.CommandText = "AddApplication";
                            command.Parameters.AddWithValue("@c_id", comboBox1.SelectedValue);
                            command.Parameters.AddWithValue("@p_id", comboBox2.SelectedValue);
                            command.Parameters.AddWithValue("@requiredEnteringDate", dateTimePicker1.Value);
                            break;

                        case "appointments":
                            if (comboBox1.SelectedValue == null)
                                throw new Exception("Выберите заявку!");
                            if (string.IsNullOrWhiteSpace(textBox1.Text))
                                throw new Exception("Результат обязателен!");
                            if (!decimal.TryParse(textBoxEmail.Text, out decimal payment))
                                throw new Exception("Неверный формат оплаты!");

                            command.CommandText = "AddAppointment";
                            command.Parameters.AddWithValue("@id_application", comboBox1.SelectedValue);
                            command.Parameters.AddWithValue("@applicationResult", textBox1.Text.Trim());
                            command.Parameters.AddWithValue("@paymentAmount", payment);
                            command.Parameters.AddWithValue("@enteringDate", dateTimePicker1.Value);
                            break;
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    if ((tableMode == "parents" || tableMode == "children") && !string.IsNullOrEmpty(newIdQuery))
                    {
                        using (SqlCommand getIdCmd = new SqlCommand(newIdQuery, connection))
                        {
                            object result = getIdCmd.ExecuteScalar();
                            if (result != DBNull.Value)
                            {
                                LastInsertedId = Convert.ToInt32(result);

                                if (selectedImagePath != null)
                                {
                                    string folderPath = Path.Combine(Application.StartupPath, "img", folderSubPath);
                                    if (!Directory.Exists(folderPath))
                                        Directory.CreateDirectory(folderPath);

                                    string savePath = Path.Combine(folderPath, $"{LastInsertedId}.jpg");

                                    try { File.Copy(selectedImagePath, savePath, true); }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Не удалось скопировать изображение:\n{ex.Message}", "Ошибка копирования", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    string updateSql = tableMode == "parents"
                                        ? "UPDATE Parent SET p_photoPath = @photoPath WHERE p_id = @id"
                                        : "UPDATE Child SET c_photoPath = @photoPath WHERE c_id = @id";

                                    using (SqlCommand updateCmd = new SqlCommand(updateSql, connection))
                                    {
                                        updateCmd.Parameters.AddWithValue("@photoPath", savePath);
                                        updateCmd.Parameters.AddWithValue("@id", LastInsertedId);
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }

                    MessageBox.Show("Данные успешно добавлены!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChildren()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT c_id, c_fullName FROM Child", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ComboBox comboBox1 = this.Controls["comboBox1"] as ComboBox;
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "c_fullName";
                comboBox1.ValueMember = "c_id";
            }
        }

        private void LoadParents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT p_id, p_fullName FROM Parent", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ComboBox comboBox2 = this.Controls["comboBox2"] as ComboBox;
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "p_fullName";
                comboBox2.ValueMember = "p_id";
            }
        }

        private void LoadApplications()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT id_application, CONCAT(c.c_fullName, ' (реб.) - ', p.p_fullName, ' (род.)') AS displayText FROM Application a INNER JOIN Child c ON a.c_id = c.c_id INNER JOIN Parent p ON a.p_id = p.p_id";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "displayText";
                comboBox1.ValueMember = "id_application";
            }
        }

        private void LoadAppointments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT id_appointment, CONCAT(c.c_fullName, ' (реб.) - ', p.p_fullName, ' (род.)') AS displayText FROM Appointment ap INNER JOIN Application a ON ap.id_application = a.id_application INNER JOIN Child c ON a.c_id = c.c_id INNER JOIN Parent p ON a.p_id = p.p_id";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "displayText";
                comboBox1.ValueMember = "id_appointment";
            }
        }

        private void buttonSelectPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    pictureBoxPhoto.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void buttonRemovePhoto_Click(object sender, EventArgs e)
        {
            selectedImagePath = null;
            pictureBoxPhoto.Image = File.Exists(defaultImagePath) ? Image.FromFile(defaultImagePath) : null;
        }
    }
}
