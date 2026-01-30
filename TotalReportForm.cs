using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaycareApplication
{
    public partial class TotalReportForm : Form
    {
        public TotalReportForm()
        {
            InitializeComponent();
        }

        private void TotalReportForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'daycareDataSet1.Total_View' table. You can move, or remove it, as needed.
            this.total_ViewTableAdapter.Fill(this.daycareDataSet1.Total_View);

            this.reportViewer1.RefreshReport();
        }
    }
}
