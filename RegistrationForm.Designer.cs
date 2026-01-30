namespace DaycareApplication
{
    partial class RegistrationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistrationForm));
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.labelLogin = new System.Windows.Forms.Label();
            this.textBoxPassword1 = new System.Windows.Forms.TextBox();
            this.textBoxPassword2 = new System.Windows.Forms.TextBox();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.comboBoxParents = new System.Windows.Forms.ComboBox();
            this.labelPassword1 = new System.Windows.Forms.Label();
            this.labelPassword2 = new System.Windows.Forms.Label();
            this.labelChooseParent = new System.Windows.Forms.Label();
            this.buttonAddParent = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogin.Location = new System.Drawing.Point(39, 58);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(100, 24);
            this.textBoxLogin.TabIndex = 3;
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLogin.Location = new System.Drawing.Point(36, 38);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(71, 17);
            this.labelLogin.TabIndex = 7;
            this.labelLogin.Text = "Enter login:";
            // 
            // textBoxPassword1
            // 
            this.textBoxPassword1.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword1.Location = new System.Drawing.Point(39, 105);
            this.textBoxPassword1.Name = "textBoxPassword1";
            this.textBoxPassword1.Size = new System.Drawing.Size(100, 24);
            this.textBoxPassword1.TabIndex = 11;
            this.textBoxPassword1.UseSystemPasswordChar = true;
            // 
            // textBoxPassword2
            // 
            this.textBoxPassword2.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword2.Location = new System.Drawing.Point(39, 156);
            this.textBoxPassword2.Name = "textBoxPassword2";
            this.textBoxPassword2.Size = new System.Drawing.Size(100, 24);
            this.textBoxPassword2.TabIndex = 12;
            this.textBoxPassword2.UseSystemPasswordChar = true;
            // 
            // buttonRegister
            // 
            this.buttonRegister.BackColor = System.Drawing.Color.SeaGreen;
            this.buttonRegister.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRegister.Font = new System.Drawing.Font("Palatino Linotype", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRegister.ForeColor = System.Drawing.Color.MintCream;
            this.buttonRegister.Location = new System.Drawing.Point(39, 252);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(97, 31);
            this.buttonRegister.TabIndex = 13;
            this.buttonRegister.Text = "Save";
            this.buttonRegister.UseVisualStyleBackColor = false;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // comboBoxParents
            // 
            this.comboBoxParents.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxParents.FormattingEnabled = true;
            this.comboBoxParents.Location = new System.Drawing.Point(39, 203);
            this.comboBoxParents.Name = "comboBoxParents";
            this.comboBoxParents.Size = new System.Drawing.Size(97, 25);
            this.comboBoxParents.TabIndex = 14;
            // 
            // labelPassword1
            // 
            this.labelPassword1.AutoSize = true;
            this.labelPassword1.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword1.Location = new System.Drawing.Point(41, 85);
            this.labelPassword1.Name = "labelPassword1";
            this.labelPassword1.Size = new System.Drawing.Size(98, 17);
            this.labelPassword1.TabIndex = 16;
            this.labelPassword1.Text = "Enter password:";
            // 
            // labelPassword2
            // 
            this.labelPassword2.AutoSize = true;
            this.labelPassword2.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword2.Location = new System.Drawing.Point(41, 136);
            this.labelPassword2.Name = "labelPassword2";
            this.labelPassword2.Size = new System.Drawing.Size(108, 17);
            this.labelPassword2.TabIndex = 17;
            this.labelPassword2.Text = "Repeat password:";
            // 
            // labelChooseParent
            // 
            this.labelChooseParent.AutoSize = true;
            this.labelChooseParent.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChooseParent.Location = new System.Drawing.Point(45, 183);
            this.labelChooseParent.Name = "labelChooseParent";
            this.labelChooseParent.Size = new System.Drawing.Size(94, 17);
            this.labelChooseParent.TabIndex = 18;
            this.labelChooseParent.Text = "Choose parent:";
            // 
            // buttonAddParent
            // 
            this.buttonAddParent.FlatAppearance.BorderSize = 0;
            this.buttonAddParent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddParent.Image = global::DaycareApplication.Properties.Resources.add;
            this.buttonAddParent.Location = new System.Drawing.Point(136, 199);
            this.buttonAddParent.Name = "buttonAddParent";
            this.buttonAddParent.Size = new System.Drawing.Size(38, 34);
            this.buttonAddParent.TabIndex = 19;
            this.buttonAddParent.UseVisualStyleBackColor = true;
            this.buttonAddParent.Click += new System.EventHandler(this.labelAddParents_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DaycareApplication.Properties.Resources.DefaultPhoto;
            this.pictureBox1.Location = new System.Drawing.Point(193, 58);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(91, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(235)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(296, 306);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonAddParent);
            this.Controls.Add(this.labelChooseParent);
            this.Controls.Add(this.labelPassword2);
            this.Controls.Add(this.labelPassword1);
            this.Controls.Add(this.comboBoxParents);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textBoxPassword2);
            this.Controls.Add(this.textBoxPassword1);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.textBoxLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RegistrationForm";
            this.Text = "Registration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.TextBox textBoxPassword1;
        private System.Windows.Forms.TextBox textBoxPassword2;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.ComboBox comboBoxParents;
        private System.Windows.Forms.Label labelPassword1;
        private System.Windows.Forms.Label labelPassword2;
        private System.Windows.Forms.Label labelChooseParent;
        private System.Windows.Forms.Button buttonAddParent;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}