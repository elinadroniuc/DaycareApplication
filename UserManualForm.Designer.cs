namespace DaycareApplication
{
    partial class UserManualForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManualForm));
            this.richTextBoxManual = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxManual
            // 
            this.richTextBoxManual.BackColor = System.Drawing.Color.MintCream;
            this.richTextBoxManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxManual.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxManual.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.richTextBoxManual.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxManual.Name = "richTextBoxManual";
            this.richTextBoxManual.ReadOnly = true;
            this.richTextBoxManual.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxManual.Size = new System.Drawing.Size(800, 450);
            this.richTextBoxManual.TabIndex = 0;
            this.richTextBoxManual.Text = "";
            // 
            // UserManualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxManual);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserManualForm";
            this.Text = "UserManualForm1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxManual;
    }
}