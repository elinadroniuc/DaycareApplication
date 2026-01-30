using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class EditForm : Form
    {
        private string tableMode;
        private int recordId;
        private string connectionString = "YOUR_CONNECTION_STRING";

        public EditForm(string mode, int id, string userRole)
        {
            InitializeComponent();
            tableMode = mode;
            recordId = id;

            if (userRole == "Clerk" && mode == "appointments")
            {
                label3.Visible = textBox3.Visible = false;
            }

            if (userRole == "Accountant" && mode != "appointments")
            {
                MessageBox.Show("Доступ ограничен");
                this.Close();
                return;
            }

            AdaptFormToMode();
            LoadData();
        }

        private void AdaptFormToMode()
        {
            label1.Visible = label2.Visible = label3.Visible = label4.Visible = true;
            textBox1.Visible = textBox2.Visible = textBox3.Visible = textBox4.Visible = true;
            comboBox1.Visible = comboBox2.Visible = false;
            dateTimePicker1.Visible = false;

            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
            comboBox1.DataSource = null;
            comboBox2.DataSource = null;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;

            switch (tableMode)
            {
                case "parents":
                    label1.Text = "Name";
                    label2.Text = "Passport";
                    label3.Text = "Email";
                    label4.Text = "Phone";

                    textBox4.Visible = true;
                    break;

                case "children":
                    label1.Text = "Name";
                    label2.Text = "Birthday";
                    label3.Text = "Certificate";
                    label4.Visible = false;
                    textBox4.Visible = false;

                    dateTimePicker1.Visible = true;
                    break;

                case "applications":
                    label1.Text = "Child";
                    label2.Text = "Entering date";
                    label3.Text = "Parent";
                    label4.Visible = false;
                    textBox4.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                    

                    comboBox1.Visible = true;
                    comboBox2.Visible = true;
                    dateTimePicker1.Visible = true;

                    LoadChildren();
                    LoadParents();
                    break;

                case "appointments":
                    label1.Text = "Application";
                    label2.Text = "Entering date";
                    label3.Text = "Payment";
                    label4.Text = "Result";

                    comboBox1.Visible = true;
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox1.Visible = false;

                    LoadApplications();
                    dateTimePicker1.Visible = true;
                    break;

                default:
                    label1.Visible = label2.Visible = label3.Visible = label4.Visible = false;
                    textBox1.Visible = textBox2.Visible = textBox3.Visible = textBox4.Visible = false;
                    comboBox1.Visible = comboBox2.Visible = false;
                    dateTimePicker1.Visible = false;
                    break;
            }
        }


        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "";

                if (tableMode == "parents")
                    query = $"SELECT * FROM Parent WHERE p_id = {recordId}";
                else if (tableMode == "children")
                    query = $"SELECT * FROM Child WHERE c_id = {recordId}";
                else if (tableMode == "applications")
                    query = $"SELECT * FROM Application WHERE id_application = {recordId}";
                else if (tableMode == "appointments")
                    query = $"SELECT * FROM Appointment WHERE id_appointment = {recordId}";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    switch (tableMode)
                    {
                        case "parents":
                            textBox1.Text = reader["p_fullName"].ToString();
                            textBox2.Text = reader["p_passportDetails"].ToString();
                            textBox3.Text = reader["p_email"].ToString();
                            textBox4.Text = reader["p_phoneNumber"].ToString();
                            break;

                        case "children":
                            textBox1.Text = reader["c_fullName"].ToString();
                            dateTimePicker1.Value = Convert.ToDateTime(reader["c_birthdayDate"]);
                            textBox3.Text = reader["c_birthCertificate"].ToString();
                            textBox2.Visible = false;
                            break;

                        case "applications":
                            comboBox1.SelectedValue = reader["c_id"];
                            comboBox2.SelectedValue = reader["p_id"];
                            dateTimePicker1.Value = Convert.ToDateTime(reader["requiredEnteringDate"]);
                            break;

                        case "appointments":
                            comboBox1.SelectedValue = reader["id_application"];
                            textBox4.Text = reader["applicationResult"].ToString();
                            textBox3.Text = reader["paymentAmount"].ToString();
                            dateTimePicker1.Value = Convert.ToDateTime(reader["enteringDate"]);
                            break;
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    switch (tableMode)
                    {
                        case "parents":
                            cmd.CommandText = "UPDATE Parent SET p_fullName=@name, p_passportDetails=@pass, p_email=@mail, p_phoneNumber=@phone WHERE p_id=@id";
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                            cmd.Parameters.AddWithValue("@mail", textBox3.Text);
                            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
                            cmd.Parameters.AddWithValue("@id", recordId);
                            break;

                        case "children":
                            cmd.CommandText = "UPDATE Child SET c_fullName=@name, c_birthdayDate=@birth, c_birthCertificate=@cert WHERE c_id=@id";
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@birth", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@cert", textBox3.Text);
                            cmd.Parameters.AddWithValue("@id", recordId);
                            break;

                        case "applications":
                            cmd.CommandText = "UPDATE Application SET c_id=@cid, p_id=@pid, requiredEnteringDate=@date WHERE id_application=@id";
                            cmd.Parameters.AddWithValue("@cid", comboBox1.SelectedValue);
                            cmd.Parameters.AddWithValue("@pid", comboBox2.SelectedValue);
                            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@id", recordId);
                            break;

                        case "appointments":
                            cmd.CommandText = "UPDATE Appointment SET id_application=@appId, applicationResult=@res, paymentAmount=@pay, enteringDate=@date WHERE id_appointment=@id";
                            cmd.Parameters.AddWithValue("@appId", comboBox1.SelectedValue);
                            cmd.Parameters.AddWithValue("@res", textBox2.Text);
                            cmd.Parameters.AddWithValue("@pay", decimal.Parse(textBox3.Text));
                            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@id", recordId);
                            break;
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Запись обновлена!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка редактирования: " + ex.Message);
            }
        }

        private void LoadChildren()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT c_id, c_fullName FROM Child", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
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
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "p_fullName";
                comboBox2.ValueMember = "p_id";
            }
        }

        private void LoadApplications()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT id_application FROM Application", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "id_application";
                comboBox1.ValueMember = "id_application";
            }
        }

        private void EditForm_Load(object sender, EventArgs e)
        {

        }
    }
}
