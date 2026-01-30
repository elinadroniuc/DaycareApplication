using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class DetailsForm : Form
    {
        private readonly string _mode;
        private readonly DataGridViewRow _selectedRow;

        public DetailsForm(string mode, DataGridViewRow selectedRow)
        {
            if (selectedRow == null)
                throw new ArgumentNullException(nameof(selectedRow));

            InitializeComponent();
            _mode = mode;
            _selectedRow = selectedRow;
            InitializeDetails();
        }

        private void InitializeDetails()
        {
            try
            {
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = false;
                pictureBoxPhoto.Visible = false;

                if (_mode == "parents")
                {
                    labelInfo.Text = "Информация о родителе";
                    InitializeParentDetails();
                }
                else if (_mode == "children")
                {
                    labelInfo.Text = "Информация о ребёнке";
                    InitializeChildDetails();
                }

                LoadPhoto();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении данных: {ex.Message}");
                Close();
            }
        }

        private bool HasCellWithValue(string cellName)
        {
            if (_selectedRow.DataGridView.Columns.Contains(cellName))
            {
                var cell = _selectedRow.Cells[cellName];
                return cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString());
            }
            return false;
        }

        private void InitializeParentDetails()
        {
            SetLabelText(label1, "ФИО", "p_fullName");
            SetLabelText(label2, "Паспорт", "p_passportDetails");
            SetLabelText(label3, "Email", "p_email");
            SetLabelText(label4, "Телефон", "p_phoneNumber");
        }

        private void InitializeChildDetails()
        {
            SetLabelText(label1, "ФИО", "c_fullName");

            if (HasCellWithValue("c_birthdayDate"))
            {
                label2.Text = $"Дата рождения: {GetDateString(_selectedRow.Cells["c_birthdayDate"].Value)}";
                label2.Visible = true;
            }

            SetLabelText(label3, "Свидетельство", "c_birthCertificate");
        }

        private string GetDateString(object value)
        {
            if (value == null || value == DBNull.Value)
                return "нет данных";

            return value is DateTime date ? date.ToShortDateString() : value.ToString();
        }

        private void SetLabelText(Label label, string prefix, string cellName)
        {
            if (HasCellWithValue(cellName))
            {
                label.Text = $"{prefix}: {_selectedRow.Cells[cellName].Value}";
                label.Visible = true;
            }
        }

        private void LoadPhoto()
        {
            try
            {
                string defaultPhotoPath = Path.Combine(Application.StartupPath, "img", "default.png");
                string photoPath = defaultPhotoPath;
                string photoCellName = _mode == "parents" ? "p_photoPath" : "c_photoPath";

                if (HasCellWithValue(photoCellName))
                {
                    string relativePhotoPath = _selectedRow.Cells[photoCellName].Value.ToString();

                    string fullPhotoPath = Path.Combine(Application.StartupPath, "img", relativePhotoPath);

                    if (File.Exists(fullPhotoPath))
                        photoPath = fullPhotoPath;
                }

                pictureBoxPhoto.Image = File.Exists(photoPath)
                    ? Image.FromFile(photoPath)
                    : Properties.Resources.DefaultPhoto;

                pictureBoxPhoto.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
                pictureBoxPhoto.Image = Properties.Resources.DefaultPhoto;
            }
        }

    }
}