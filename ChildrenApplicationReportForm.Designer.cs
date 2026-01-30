namespace DaycareApplication
{
    partial class ChildrenApplicationReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChildrenApplicationReportForm));
            this.childApplicationViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.daycareDataSet2 = new DaycareApplication.daycareDataSet2();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.child_Application_ViewTableAdapter = new DaycareApplication.daycareDataSet2TableAdapters.Child_Application_ViewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.childApplicationViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet2)).BeginInit();
            this.SuspendLayout();
            // 
            // childApplicationViewBindingSource
            // 
            this.childApplicationViewBindingSource.DataMember = "Child_Application_View";
            this.childApplicationViewBindingSource.DataSource = this.daycareDataSet2;
            // 
            // daycareDataSet2
            // 
            this.daycareDataSet2.DataSetName = "daycareDataSet2";
            this.daycareDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetChildrenApplication";
            reportDataSource1.Value = this.childApplicationViewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "DaycareApplication.ChildrenApplicationReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // child_Application_ViewTableAdapter
            // 
            this.child_Application_ViewTableAdapter.ClearBeforeFill = true;
            // 
            // ChildrenApplicationReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChildrenApplicationReportForm";
            this.Text = "Children Application Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ChildrenApplicationReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.childApplicationViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private daycareDataSet2 daycareDataSet2;
        private System.Windows.Forms.BindingSource childApplicationViewBindingSource;
        private daycareDataSet2TableAdapters.Child_Application_ViewTableAdapter child_Application_ViewTableAdapter;
    }
}