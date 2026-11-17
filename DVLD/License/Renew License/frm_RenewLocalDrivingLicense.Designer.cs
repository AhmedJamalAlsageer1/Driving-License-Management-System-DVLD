namespace DVLD.FormsApplications
{
    partial class frm_RenewLocalDrivingLicense
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtxtInput = new System.Windows.Forms.MaskedTextBox();
            this.btnSearchLicense = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uctrlLicenseInfo1 = new DVLD.User_Controls.uctrlLicenseInfo();
            this.llblShowNewLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.llblShowLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRenew = new System.Windows.Forms.Button();
            this.uctrl_RenewLicense1 = new DVLD.User_Controls.uctrl_RenewLicense();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtxtInput);
            this.groupBox1.Controls.Add(this.btnSearchLicense);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(53, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 98);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // mtxtInput
            // 
            this.mtxtInput.Location = new System.Drawing.Point(125, 50);
            this.mtxtInput.Mask = "000000000000000000";
            this.mtxtInput.Name = "mtxtInput";
            this.mtxtInput.PromptChar = ' ';
            this.mtxtInput.Size = new System.Drawing.Size(271, 26);
            this.mtxtInput.TabIndex = 4;
            this.mtxtInput.Click += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.Enter += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtInput_KeyDown);
            // 
            // btnSearchLicense
            // 
            this.btnSearchLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchLicense.Image = global::DVLD.Properties.Resources.driving_license_32;
            this.btnSearchLicense.Location = new System.Drawing.Point(510, 34);
            this.btnSearchLicense.Name = "btnSearchLicense";
            this.btnSearchLicense.Size = new System.Drawing.Size(65, 56);
            this.btnSearchLicense.TabIndex = 3;
            this.btnSearchLicense.UseVisualStyleBackColor = true;
            this.btnSearchLicense.Click += new System.EventHandler(this.btnSearchLicense_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "License ID: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(329, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(419, 36);
            this.label2.TabIndex = 4;
            this.label2.Text = "Renew License Application";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.panel1.Controls.Add(this.uctrlLicenseInfo1);
            this.panel1.Controls.Add(this.llblShowNewLicenseInfo);
            this.panel1.Controls.Add(this.llblShowLicensesHistory);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRenew);
            this.panel1.Controls.Add(this.uctrl_RenewLicense1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(977, 820);
            this.panel1.TabIndex = 6;
            // 
            // uctrlLicenseInfo1
            // 
            this.uctrlLicenseInfo1.Location = new System.Drawing.Point(12, 158);
            this.uctrlLicenseInfo1.Name = "uctrlLicenseInfo1";
            this.uctrlLicenseInfo1.Size = new System.Drawing.Size(951, 426);
            this.uctrlLicenseInfo1.TabIndex = 11;
            // 
            // llblShowNewLicenseInfo
            // 
            this.llblShowNewLicenseInfo.AutoSize = true;
            this.llblShowNewLicenseInfo.Location = new System.Drawing.Point(264, 921);
            this.llblShowNewLicenseInfo.Name = "llblShowNewLicenseInfo";
            this.llblShowNewLicenseInfo.Size = new System.Drawing.Size(148, 17);
            this.llblShowNewLicenseInfo.TabIndex = 10;
            this.llblShowNewLicenseInfo.TabStop = true;
            this.llblShowNewLicenseInfo.Text = "Show New License Info";
            this.llblShowNewLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowNewLicenseInfo_LinkClicked);
            // 
            // llblShowLicensesHistory
            // 
            this.llblShowLicensesHistory.AutoSize = true;
            this.llblShowLicensesHistory.Location = new System.Drawing.Point(90, 921);
            this.llblShowLicensesHistory.Name = "llblShowLicensesHistory";
            this.llblShowLicensesHistory.Size = new System.Drawing.Size(143, 17);
            this.llblShowLicensesHistory.TabIndex = 9;
            this.llblShowLicensesHistory.TabStop = true;
            this.llblShowLicensesHistory.Text = "Show Licenses History";
            this.llblShowLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowLicensesHistory_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::DVLD.Properties.Resources.close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(595, 921);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(132, 48);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRenew
            // 
            this.btnRenew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenew.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenew.Image = global::DVLD.Properties.Resources.Save_32;
            this.btnRenew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenew.Location = new System.Drawing.Point(747, 921);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(132, 48);
            this.btnRenew.TabIndex = 7;
            this.btnRenew.Text = "Renew";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // uctrl_RenewLicense1
            // 
            this.uctrl_RenewLicense1.Location = new System.Drawing.Point(12, 590);
            this.uctrl_RenewLicense1.Name = "uctrl_RenewLicense1";
            this.uctrl_RenewLicense1.Size = new System.Drawing.Size(867, 327);
            this.uctrl_RenewLicense1.TabIndex = 6;
            // 
            // frm_RenewLocalDrivingLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 820);
            this.Controls.Add(this.panel1);
            this.Name = "frm_RenewLocalDrivingLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renew Local Driving License";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox mtxtInput;
        private System.Windows.Forms.Button btnSearchLicense;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private User_Controls.uctrl_RenewLicense uctrl_RenewLicense1;
        private System.Windows.Forms.LinkLabel llblShowNewLicenseInfo;
        private System.Windows.Forms.LinkLabel llblShowLicensesHistory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRenew;
        private User_Controls.uctrlLicenseInfo uctrlLicenseInfo1;
    }
}