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
    public partial class ChildrenApplicationReportForm : Form
    {
        public ChildrenApplicationReportForm()
        {
            InitializeComponent();
        }

        private void ChildrenApplicationReportForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'daycareDataSet2.Child_Application_View' table. You can move, or remove it, as needed.
            this.child_Application_ViewTableAdapter.Fill(this.daycareDataSet2.Child_Application_View);

            this.reportViewer1.RefreshReport();
        }
    }
}
