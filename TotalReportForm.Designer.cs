namespace DaycareApplication
{
    partial class TotalReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalReportForm));
            this.totalViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.daycareDataSet1 = new DaycareApplication.daycareDataSet1();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.total_ViewTableAdapter = new DaycareApplication.daycareDataSet1TableAdapters.Total_ViewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.totalViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // totalViewBindingSource
            // 
            this.totalViewBindingSource.DataMember = "Total_View";
            this.totalViewBindingSource.DataSource = this.daycareDataSet1;
            // 
            // daycareDataSet1
            // 
            this.daycareDataSet1.DataSetName = "daycareDataSet1";
            this.daycareDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.BackColor = System.Drawing.Color.Honeydew;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.totalViewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "DaycareApplication.TotalReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(1, -2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(697, 384);
            this.reportViewer1.TabIndex = 0;
            // 
            // total_ViewTableAdapter
            // 
            this.total_ViewTableAdapter.ClearBeforeFill = true;
            // 
            // TotalReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 374);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TotalReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TotalReportForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TotalReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.totalViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.daycareDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private daycareDataSet1 daycareDataSet1;
        private System.Windows.Forms.BindingSource totalViewBindingSource;
        private daycareDataSet1TableAdapters.Total_ViewTableAdapter total_ViewTableAdapter;
    }
}