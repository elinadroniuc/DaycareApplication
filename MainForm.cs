using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class MainForm : Form
    {
        string connectionString = "YOUR_CONNECTION_STRING";
        string mode = "";
        private DataTable originalTable = null;


        private string loggedInUser;
        private string loggedInRole;

        public bool AccessGranted { get; private set; } = true;

        public MainForm(string user, string role, int? parentId = null)
        {
            InitializeComponent();

            loggedInUser = user;
            loggedInRole = role;

            SetAccessByRole(role);

            if (role == "Parent" && parentId.HasValue)
            {
                LoadParentData(parentId.Value);
            }
        }

        private void LoadParentData(int parentId)
        {
            try
            {
                string query = @"
            SELECT p.p_id, p.p_fullName, p.p_passportDetails, p.p_email, p.p_phoneNumber 
            FROM Parent p
            WHERE p.p_id = @parentId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@parentId", parentId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "ParentData");

                    dataGridViewMain.DataSource = ds;
                    dataGridViewMain.DataMember = "ParentData";

                    dataGridViewMain.Columns["p_id"].Visible = false;
                    ShowHeadersParents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных родителя: {ex.Message}");
            }
        }

        private void SetAccessByRole(string role)
        {
            labelCurrentUser.Visible = true;
            labelCurrentUser.Text = $"Вы вошли как: {loggedInUser} ({role})";

            buttonAdd.Visible = buttonEdit.Visible = buttonDelete.Visible = false;
            reportsToolStripMenuItem.Visible = diagrammToolStripMenuItem.Visible = false;
            buttonParents.Visible = buttonChildren.Visible = buttonApplications.Visible = buttonAppointments.Visible = true;

            switch (role)
            {
                case "Admin": 
                    buttonAdd.Visible = buttonEdit.Visible = buttonDelete.Visible = true;
                    reportsToolStripMenuItem.Visible = diagrammToolStripMenuItem.Visible = true;
                    break;

                case "Clerk":
                    buttonAdd.Visible = buttonEdit.Visible = true;
                    buttonDelete.Visible = false;
                    reportsToolStripMenuItem.Visible = true;
                    break;

                case "Accountant": 
                    buttonEdit.Visible = true; 
                    buttonParents.Visible = buttonChildren.Visible = false;
                    reportsToolStripMenuItem.Visible = true;
                    break;

                case "Parent":
                    buttonAdd.Visible = true;
                    buttonParents.Visible = buttonChildren.Visible = false; 
                    break;

                case "Guest":
                    buttonParents.Visible = buttonChildren.Visible = false;
                    break;
            }
        }


        void RowNumbering()
        {
            for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
                dataGridViewMain.Rows[i].HeaderCell.Value = (i + 1).ToString();
        }

        private void buttonParents_Click(object sender, EventArgs e)
        {
            string query = "SELECT p_id, p_fullName, p_passportDetails, p_email, p_phoneNumber FROM Parent";
            LoadData(query, "Parents", "Родители");
            ShowHeadersParents();
            dataGridViewMain.Columns["p_id"].Visible = false;
            UpdateFilterControlsVisibility();
            panel1.Visible = true;
            panel2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            dataGridViewMain.Visible = true;
            buttonDetails.Visible = true;
        }

        private void buttonChildren_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = true;
            string query = "SELECT c_id, c_fullName, c_birthdayDate, c_birthCertificate FROM Child";
            LoadData(query, "Children", "Дети");
            ShowHeadersChildren();
            dataGridViewMain.Columns["c_id"].Visible = false;
            UpdateFilterControlsVisibility();
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            dataGridViewMain.Visible = true;
            buttonDetails.Visible = false;
        }

        private void buttonApplications_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT 
                    Application.id_application,
                    Child.c_fullName AS ChildName,
                    Parent.p_fullName AS ParentName,
                    Application.requiredEnteringDate
                FROM Application
                JOIN Child ON Application.c_id = Child.c_id
                JOIN Parent ON Application.p_id = Parent.p_id";
            LoadData(query, "Applications", "Заявки");
            ShowHeadersApplications();
            dataGridViewMain.Columns["id_application"].Visible = false;
            UpdateFilterControlsVisibility();
            panel2.Visible = false;
            panel1.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            dataGridViewMain.Visible = true;
            buttonDetails.Visible = false;

        }

        private void buttonAppointments_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT 
                    Appointment.id_appointment,
                    Child.c_fullName AS ChildName,
                    Appointment.applicationResult,
                    Appointment.paymentAmount,
                    Appointment.enteringDate
                FROM Appointment
                JOIN Application ON Appointment.id_application = Application.id_application
                JOIN Child ON Application.c_id = Child.c_id";
            LoadData(query, "Appointments", "Записи");
            ShowHeadersAppointments();
            dataGridViewMain.Columns["id_appointment"].Visible = false;
            UpdateFilterControlsVisibility();

            label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryCountAppointments = "SELECT COUNT(*) FROM Appointment WHERE applicationResult = 'Accepted'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int acceptedCount = (int)command.ExecuteScalar();

                    label1.Text = "Accepted applications: " + acceptedCount.ToString();
                }
                string queryTotal = "SELECT COUNT(*) FROM Application";
                using (SqlCommand command = new SqlCommand(queryTotal, connection))
                {
                    int totalApplications = (int)command.ExecuteScalar();
                    label2.Text = $"Total applications: {totalApplications}";
                }
                string queryRejected = "SELECT COUNT(*) FROM Appointment WHERE applicationResult = 'Rejected'";
                using (SqlCommand command = new SqlCommand(queryRejected, connection))
                {
                    int rejectedCount = (int)command.ExecuteScalar();
                    label3.Text = $"Rejected applications: {rejectedCount}";
                }
                string queryAvgPayment = "SELECT AVG(paymentAmount) FROM Appointment WHERE applicationResult = 'Accepted'";
                using (SqlCommand command = new SqlCommand(queryAvgPayment, connection))
                {
                    object result = command.ExecuteScalar();
                    decimal avgPayment = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    label4.Text = $"Average payment: {avgPayment:C}";
                }

                string queryMaxPayment = "SELECT MAX(paymentAmount) FROM Appointment";
                using (SqlCommand command = new SqlCommand(queryMaxPayment, connection))
                {
                    object result = command.ExecuteScalar();
                    decimal maxPayment = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    label5.Text = $"Max payment: {maxPayment:C}";
                }

            }
            panel2.Visible = true;
            panel1.Visible = true;
            dateTimePickerFrom.Visible = true;
            dateTimePickerTo.Visible = true;
            dataGridViewMain.Visible = true;
            buttonDetails.Visible = false;

        }
        private void LoadData(string query, string tableName, string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, tableName);

                    mode = tableName.ToLower();

                    originalTable = ds.Tables[tableName];

                    dataGridViewMain.DataSource = originalTable;

                    RowNumbering();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowHeadersParents()
        {
            SetHeader("p_fullName", "ФИО");
            SetHeader("p_passportDetails", "Паспорт");
            SetHeader("p_email", "Email");
            SetHeader("p_phoneNumber", "Телефон");
        }

        private void ShowHeadersChildren()
        {
            SetHeader("c_fullName", "ФИО");
            SetHeader("c_birthdayDate", "Дата рождения");
            SetHeader("c_birthCertificate", "Свидетельство о рождении");
        }

        private void ShowHeadersApplications()
        {
            SetHeader("ChildName", "Ребёнок");
            SetHeader("ParentName", "Родитель");
            SetHeader("requiredEnteringDate", "Требуемая дата поступления");
        }

        private void ShowHeadersAppointments()
        {
            SetHeader("ChildName", "Ребёнок");
            SetHeader("applicationResult", "Результат");
            SetHeader("paymentAmount", "Оплата");
            SetHeader("enteringDate", "Дата поступления");
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewMain.Columns.Contains(columnName))
            {
                dataGridViewMain.Columns[columnName].HeaderText = headerText;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (loggedInRole == "Accountant")
            {
                return;
            }

            AddForm addForm = new AddForm(mode, loggedInRole);
            addForm.ShowDialog();
            ReloadData();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0) return;

            string idColumn = "";
            switch (mode)
            {
                case "parents":
                    idColumn = "p_id";
                    break;
                case "children":
                    idColumn = "c_id";
                    break;
                case "applications":
                    idColumn = "id_application";
                    break;
                case "appointments":
                    idColumn = "id_appointment";
                    break;
            }

            object idValue = dataGridViewMain.SelectedRows[0].Cells[idColumn].Value;
            if (idValue == null || !int.TryParse(idValue.ToString(), out int id))
            {
                MessageBox.Show("Не удалось определить ID записи.");
                return;
            }

            EditForm editForm = new EditForm(mode, id, loggedInRole);
            editForm.ShowDialog();

            ReloadData();
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (loggedInRole != "Admin") return;

            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!");
                return;
            }

            string tableName = "";
            string idColumn = "";

            if (mode == "parents")
            {
                tableName = "Parent";
                idColumn = "p_id";
            }
            else if (mode == "children")
            {
                tableName = "Child";
                idColumn = "c_id";
            }
            else if (mode == "applications")
            {
                tableName = "Application";
                idColumn = "id_application";
            }
            else if (mode == "appointments")
            {
                tableName = "Appointment";
                idColumn = "id_appointment";
            }

            object value = dataGridViewMain.SelectedRows[0].Cells[idColumn].Value;
            if (value == null || !int.TryParse(value.ToString(), out int id))
            {
                MessageBox.Show("Не удалось определить ID записи для удаления.");
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?",
                                                  "Подтверждение удаления",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = $"DELETE FROM {tableName} WHERE {idColumn} = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Запись успешно удалена.");

                    switch (mode)
                    {
                        case "parents":
                            buttonParents_Click(null, null);
                            dataGridViewMain.Columns["p_id"].Visible = false;
                            break;
                        case "children":
                            buttonChildren_Click(null, null);
                            dataGridViewMain.Columns["c_id"].Visible = false;
                            break;
                        case "applications":
                            buttonApplications_Click(null, null);
                            dataGridViewMain.Columns["application_id"].Visible = false;
                            break;
                        case "appointments":
                            buttonAppointments_Click(null, null);
                            dataGridViewMain.Columns["app_id"].Visible = false;

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string textSearch = textBoxSearch.Text.Trim();

            if (originalTable == null)
                return;

            try
            {
                DataView view = new DataView(originalTable);

                string filterExpression = "";

                switch (mode)
                {
                    case "parents":
                        filterExpression = $"p_fullName LIKE '%{textSearch.Replace("'", "''")}%' OR p_phoneNumber LIKE '%{textSearch.Replace("'", "''")}%'";
                        break;

                    case "children":
                        filterExpression = $"c_fullName LIKE '%{textSearch.Replace("'", "''")}%'";
                        break;

                    case "applications":
                        filterExpression = $"ChildName LIKE '%{textSearch.Replace("'", "''")}%' OR ParentName LIKE '%{textSearch.Replace("'", "''")}%'";
                        break;

                    case "appointments":
                        filterExpression = $"ChildName LIKE '%{textSearch.Replace("'", "''")}%'";
                        break;

                    default:
                        return;
                }

                view.RowFilter = filterExpression;

                dataGridViewMain.DataSource = view;

                switch (mode)
                {
                    case "parents":
                        ShowHeadersParents();
                        if (dataGridViewMain.Columns.Contains("p_id"))
                            dataGridViewMain.Columns["p_id"].Visible = false;
                        break;

                    case "children":
                        ShowHeadersChildren();
                        if (dataGridViewMain.Columns.Contains("c_id"))
                            dataGridViewMain.Columns["c_id"].Visible = false;
                        break;

                    case "applications":
                        ShowHeadersApplications();
                        if (dataGridViewMain.Columns.Contains("id_application"))
                            dataGridViewMain.Columns["id_application"].Visible = false;
                        break;

                    case "appointments":
                        ShowHeadersAppointments();
                        if (dataGridViewMain.Columns.Contains("id_appointment"))
                            dataGridViewMain.Columns["id_appointment"].Visible = false;
                        break;
                }

                RowNumbering();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            switch (mode)
            {
                case "parents":
                    buttonParents_Click(null, null);
                    break;
                case "children":
                    buttonChildren_Click(null, null);
                    break;
                case "applications":
                    buttonApplications_Click(null, null);
                    break;
                case "appointments":
                    buttonAppointments_Click(null, null);
                    break;
                default:
                    dataGridViewMain.DataSource = null;
                    break;
            }
        }

        private void childrenWithApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildrenApplicationReportForm childrenApplicationReportForm = new ChildrenApplicationReportForm();
            childrenApplicationReportForm.ShowDialog();
        }

        private void parentsWithChildrenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParentsChildrenReportForm parentsChildrenReportForm = new ParentsChildrenReportForm();
            parentsChildrenReportForm.ShowDialog();
        }

        private void totalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalReportForm totalReportForm = new TotalReportForm();
            totalReportForm.ShowDialog();
        }

        private void numberOfChildrenByAgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartForm chartForm = new ChartForm();
            chartForm.ShowDialog();
        }
        private void UpdateFilterControlsVisibility()
        {
            bool showFilters = mode == "children";
            dateTimePickerFrom.Visible = showFilters;
            dateTimePickerTo.Visible = showFilters;
        }

        private void ApplyFilters()
        {
            if (mode != "children") return;
            if (mode == "children") { 
            try
            {
                if (originalTable == null) return;

                DateTime dateFrom = dateTimePickerFrom.Value.Date;
                DateTime dateTo = dateTimePickerTo.Value.Date;

                if (dateFrom > dateTo)
                {
                    MessageBox.Show("Начальная дата не может быть позже конечной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<string> filters = new List<string>
        {
            $"c_birthdayDate >= #{dateFrom:yyyy-MM-dd}#",
            $"c_birthdayDate <= #{dateTo:yyyy-MM-dd}#"
        };

                string finalFilter = string.Join(" AND ", filters);

                DataView view = new DataView(originalTable)
                {
                    RowFilter = finalFilter
                };

                dataGridViewMain.DataSource = view;
                ShowHeadersChildren();
                RowNumbering();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка фильтрации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

            if (mode == "appointment") { 
                    try
                    {
                        if (originalTable == null) return;

                        DateTime dateFrom = dateTimePickerFrom.Value.Date;
                        DateTime dateTo = dateTimePickerTo.Value.Date;

                        if (dateFrom > dateTo)
                        {
                            MessageBox.Show("Начальная дата не может быть позже конечной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        List<string> filters = new List<string>
        {
            $"enteringDate >= #{dateFrom:yyyy-MM-dd}#",
            $"enteringDate <= #{dateTo:yyyy-MM-dd}#"
        };

                        string finalFilter = string.Join(" AND ", filters);

                        DataView view = new DataView(originalTable)
                        {
                            RowFilter = finalFilter
                        };

                        dataGridViewMain.DataSource = view;
                        ShowHeadersAppointments();
                        RowNumbering();

                }
                catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка фильтрации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void buttonDetails_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count > 0)
            {
                var detailsForm = new DetailsForm("parents", dataGridViewMain.SelectedRows[0]);
                detailsForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для просмотра");
            }
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                if (originalTable == null) return;

                dataGridViewMain.DataSource = originalTable;

                dateTimePickerFrom.Value = originalTable.AsEnumerable()
                    .Select(r => r.Field<DateTime>("c_birthdayDate"))
                    .DefaultIfEmpty(DateTime.Today)
                    .Min();

                dateTimePickerTo.Value = originalTable.AsEnumerable()
                    .Select(r => r.Field<DateTime>("c_birthdayDate"))
                    .DefaultIfEmpty(DateTime.Today)
                    .Max();

                ShowHeadersChildren(); 

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сбросе фильтров: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminToolsForm adminToolsForm = new AdminToolsForm();
            adminToolsForm.ShowDialog();
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserManualForm userManualForm = new UserManualForm();
            userManualForm.ShowDialog();
        }
    }
    }

