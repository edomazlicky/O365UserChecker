namespace O365UserChecker
{
    partial class FMain
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
            this.LAdminUsername = new System.Windows.Forms.Label();
            this.EAdminUsername = new System.Windows.Forms.TextBox();
            this.TLPAdminCredentials = new System.Windows.Forms.TableLayoutPanel();
            this.BResetUPN = new System.Windows.Forms.Button();
            this.LPassword = new System.Windows.Forms.Label();
            this.EAdminPassword = new System.Windows.Forms.TextBox();
            this.LStudentUPN = new System.Windows.Forms.Label();
            this.EStudentUPN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BVerifyStudent = new System.Windows.Forms.Button();
            this.EDomain = new System.Windows.Forms.TextBox();
            this.EVerifyStatus = new System.Windows.Forms.TextBox();
            this.BWVerifyUser = new System.ComponentModel.BackgroundWorker();
            this.BWResetUpn = new System.ComponentModel.BackgroundWorker();
            this.TLPAdminCredentials.SuspendLayout();
            this.SuspendLayout();
            // 
            // LAdminUsername
            // 
            this.LAdminUsername.AutoSize = true;
            this.LAdminUsername.Location = new System.Drawing.Point(3, 0);
            this.LAdminUsername.Name = "LAdminUsername";
            this.LAdminUsername.Size = new System.Drawing.Size(85, 13);
            this.LAdminUsername.TabIndex = 0;
            this.LAdminUsername.Text = "Admin username";
            // 
            // EAdminUsername
            // 
            this.EAdminUsername.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::O365UserChecker.Properties.Settings.Default, "AdminUsername", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EAdminUsername.Location = new System.Drawing.Point(3, 16);
            this.EAdminUsername.Name = "EAdminUsername";
            this.EAdminUsername.Size = new System.Drawing.Size(193, 20);
            this.EAdminUsername.TabIndex = 1;
            this.EAdminUsername.Text = global::O365UserChecker.Properties.Settings.Default.AdminUsername;
            // 
            // TLPAdminCredentials
            // 
            this.TLPAdminCredentials.ColumnCount = 2;
            this.TLPAdminCredentials.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.16638F));
            this.TLPAdminCredentials.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.83362F));
            this.TLPAdminCredentials.Controls.Add(this.BResetUPN, 1, 4);
            this.TLPAdminCredentials.Controls.Add(this.LPassword, 1, 0);
            this.TLPAdminCredentials.Controls.Add(this.LAdminUsername, 0, 0);
            this.TLPAdminCredentials.Controls.Add(this.EAdminUsername, 0, 1);
            this.TLPAdminCredentials.Controls.Add(this.EAdminPassword, 1, 1);
            this.TLPAdminCredentials.Controls.Add(this.LStudentUPN, 0, 2);
            this.TLPAdminCredentials.Controls.Add(this.EStudentUPN, 0, 3);
            this.TLPAdminCredentials.Controls.Add(this.label1, 1, 2);
            this.TLPAdminCredentials.Controls.Add(this.BVerifyStudent, 0, 4);
            this.TLPAdminCredentials.Controls.Add(this.EDomain, 1, 3);
            this.TLPAdminCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.TLPAdminCredentials.Location = new System.Drawing.Point(0, 0);
            this.TLPAdminCredentials.Name = "TLPAdminCredentials";
            this.TLPAdminCredentials.RowCount = 5;
            this.TLPAdminCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.81818F));
            this.TLPAdminCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.18182F));
            this.TLPAdminCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.TLPAdminCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.TLPAdminCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.TLPAdminCredentials.Size = new System.Drawing.Size(527, 130);
            this.TLPAdminCredentials.TabIndex = 2;
            // 
            // BResetUPN
            // 
            this.BResetUPN.Location = new System.Drawing.Point(219, 88);
            this.BResetUPN.Name = "BResetUPN";
            this.BResetUPN.Size = new System.Drawing.Size(98, 23);
            this.BResetUPN.TabIndex = 9;
            this.BResetUPN.Text = "Reset UPN";
            this.BResetUPN.UseVisualStyleBackColor = true;
            this.BResetUPN.Click += new System.EventHandler(this.BResetUPN_Click);
            // 
            // LPassword
            // 
            this.LPassword.AutoSize = true;
            this.LPassword.Location = new System.Drawing.Point(219, 0);
            this.LPassword.Name = "LPassword";
            this.LPassword.Size = new System.Drawing.Size(84, 13);
            this.LPassword.TabIndex = 2;
            this.LPassword.Text = "Admin password";
            // 
            // EAdminPassword
            // 
            this.EAdminPassword.Location = new System.Drawing.Point(219, 16);
            this.EAdminPassword.Name = "EAdminPassword";
            this.EAdminPassword.PasswordChar = '*';
            this.EAdminPassword.Size = new System.Drawing.Size(193, 20);
            this.EAdminPassword.TabIndex = 2;
            // 
            // LStudentUPN
            // 
            this.LStudentUPN.AutoSize = true;
            this.LStudentUPN.Location = new System.Drawing.Point(3, 43);
            this.LStudentUPN.Name = "LStudentUPN";
            this.LStudentUPN.Size = new System.Drawing.Size(84, 13);
            this.LStudentUPN.TabIndex = 4;
            this.LStudentUPN.Text = "O365 User UPN";
            // 
            // EStudentUPN
            // 
            this.EStudentUPN.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::O365UserChecker.Properties.Settings.Default, "StudentUPN", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EStudentUPN.Location = new System.Drawing.Point(3, 60);
            this.EStudentUPN.Name = "EStudentUPN";
            this.EStudentUPN.Size = new System.Drawing.Size(193, 20);
            this.EStudentUPN.TabIndex = 3;
            this.EStudentUPN.Text = global::O365UserChecker.Properties.Settings.Default.StudentUPN;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "User Domain";
            // 
            // BVerifyStudent
            // 
            this.BVerifyStudent.Location = new System.Drawing.Point(3, 88);
            this.BVerifyStudent.Name = "BVerifyStudent";
            this.BVerifyStudent.Size = new System.Drawing.Size(75, 23);
            this.BVerifyStudent.TabIndex = 5;
            this.BVerifyStudent.Text = "Verify User";
            this.BVerifyStudent.UseVisualStyleBackColor = true;
            this.BVerifyStudent.Click += new System.EventHandler(this.BVerifyStudent_Click);
            // 
            // EDomain
            // 
            this.EDomain.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::O365UserChecker.Properties.Settings.Default, "UserDomain", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EDomain.Location = new System.Drawing.Point(219, 60);
            this.EDomain.Name = "EDomain";
            this.EDomain.Size = new System.Drawing.Size(193, 20);
            this.EDomain.TabIndex = 4;
            this.EDomain.Text = global::O365UserChecker.Properties.Settings.Default.UserDomain;
            // 
            // EVerifyStatus
            // 
            this.EVerifyStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EVerifyStatus.Location = new System.Drawing.Point(0, 130);
            this.EVerifyStatus.Multiline = true;
            this.EVerifyStatus.Name = "EVerifyStatus";
            this.EVerifyStatus.Size = new System.Drawing.Size(527, 284);
            this.EVerifyStatus.TabIndex = 3;
            // 
            // BWVerifyUser
            // 
            this.BWVerifyUser.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BWVerifyUser_DoWork);
            this.BWVerifyUser.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BWVerifyUser_RunWorkerCompleted);
            // 
            // BWResetUpn
            // 
            this.BWResetUpn.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BWResetUpn_DoWork);
            this.BWResetUpn.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BWResetUpn_RunWorkerCompleted);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(527, 414);
            this.Controls.Add(this.EVerifyStatus);
            this.Controls.Add(this.TLPAdminCredentials);
            this.Name = "FMain";
            this.Text = "O365 User Checker";
            this.TLPAdminCredentials.ResumeLayout(false);
            this.TLPAdminCredentials.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LAdminUsername;
        private System.Windows.Forms.TextBox EAdminUsername;
        private System.Windows.Forms.TableLayoutPanel TLPAdminCredentials;
        private System.Windows.Forms.Label LPassword;
        private System.Windows.Forms.TextBox EAdminPassword;
        private System.Windows.Forms.Label LStudentUPN;
        private System.Windows.Forms.TextBox EStudentUPN;
        private System.Windows.Forms.Button BVerifyStudent;
        private System.Windows.Forms.TextBox EVerifyStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EDomain;
        private System.ComponentModel.BackgroundWorker BWVerifyUser;
        private System.Windows.Forms.Button BResetUPN;
        private System.ComponentModel.BackgroundWorker BWResetUpn;
    }
}

