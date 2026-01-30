namespace DaycareApplication
{
    partial class ParentsChildrenReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParentsChildrenReportForm));
            this.childParentApplicationViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.daycareDataSet = new DaycareApplication.daycareDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.child_Parent_Application_ViewTableAdapter = new DaycareApplication.daycareDataSetTableAdapters.Child_Parent_Application_ViewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.childParentApplicationViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // childParentApplicationViewBindingSource
            // 
            this.childParentApplicationViewBindingSource.DataMember = "Child_Parent_Application_View";
            this.childParentApplicationViewBindingSource.DataSource = this.daycareDataSet;
            // 
            // daycareDataSet
            // 
            this.daycareDataSet.DataSetName = "daycareDataSet";
            this.daycareDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.childParentApplicationViewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "DaycareApplication.ParentChildrenReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(756, 397);
            this.reportViewer1.TabIndex = 0;
            // 
            // child_Parent_Application_ViewTableAdapter
            // 
            this.child_Parent_Application_ViewTableAdapter.ClearBeforeFill = true;
            // 
            // ParentsChildrenReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 397);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParentsChildrenReportForm";
            this.Text = "Parents Children Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ParentsChildrenReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.childParentApplicationViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private daycareDataSet daycareDataSet;
        private System.Windows.Forms.BindingSource childParentApplicationViewBindingSource;
        private daycareDataSetTableAdapters.Child_Parent_Application_ViewTableAdapter child_Parent_Application_ViewTableAdapter;
    }
}