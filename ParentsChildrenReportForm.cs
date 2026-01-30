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
    public partial class ParentsChildrenReportForm : Form
    {
        public ParentsChildrenReportForm()
        {
            InitializeComponent();
        }

        private void ParentsChildrenReportForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'daycareDataSet.Child_Parent_Application_View' table. You can move, or remove it, as needed.
            this.child_Parent_Application_ViewTableAdapter.Fill(this.daycareDataSet.Child_Parent_Application_View);

            this.reportViewer1.RefreshReport();
        }
    }
}
