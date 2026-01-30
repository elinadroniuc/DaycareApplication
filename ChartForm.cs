using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DaycareApplication
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();
            LoadAgeDistributionChart();
        }

        private void LoadAgeDistributionChart()
        {
            string connectionString = @"Data Source=DESKTOP-KPFP023;Initial Catalog=daycare;Integrated Security=True;Encrypt=False";
            string query = @"
                SELECT DATEDIFF(YEAR, c_birthdayDate, GETDATE()) AS Age, COUNT(*) AS Count
                FROM Child
                GROUP BY DATEDIFF(YEAR, c_birthdayDate, GETDATE())
                ORDER BY Age";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    chart1.Series.Clear();
                    chart1.Series.Add("AgeDistribution");
                    chart1.Series["AgeDistribution"].ChartType = SeriesChartType.Column;

                    while (reader.Read())
                    {
                        int age = reader.GetInt32(0);
                        int count = reader.GetInt32(1);
                        chart1.Series["AgeDistribution"].Points.AddXY(age, count);
                    }

                    chart1.ChartAreas[0].AxisX.Title = "Возраст";
                    chart1.ChartAreas[0].AxisY.Title = "Количество детей";
                    chart1.Series["AgeDistribution"].IsValueShownAsLabel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
    }
}
