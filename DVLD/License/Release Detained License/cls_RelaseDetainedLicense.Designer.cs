namespace DVLD.License.Release_Detained_License
{
    partial class frm_ReleaseDetainedLicense
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
            this.uctrl_ReleaseDetainedLicense_Info1 = new DVLD.User_Controls.Uctrl_ReleaseDetainedLicense_Info();
            this.uctrlLicenseInfo1 = new DVLD.User_Controls.uctrlLicenseInfo();
            this.panel1 = new System.Windows.Forms.Panel();
            this.llblShowNewLicensesInfo = new System.Windows.Forms.LinkLabel();
            this.llblShowLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.btnReleaseDetain = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtxtInput = new System.Windows.Forms.MaskedTextBox();
            this.btnSearchLicense = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uctrl_ReleaseDetainedLicense_Info1
            // 
            this.uctrl_ReleaseDetainedLicense_Info1.Location = new System.Drawing.Point(23, 636);
            this.uctrl_ReleaseDetainedLicense_Info1.Name = "uctrl_ReleaseDetainedLicense_Info1";
            this.uctrl_ReleaseDetainedLicense_Info1.Size = new System.Drawing.Size(990, 240);
            this.uctrl_ReleaseDetainedLicense_Info1.TabIndex = 0;
            // 
            // uctrlLicenseInfo1
            // 
            this.uctrlLicenseInfo1.Location = new System.Drawing.Point(23, 204);
            this.uctrlLicenseInfo1.Name = "uctrlLicenseInfo1";
            this.uctrlLicenseInfo1.Size = new System.Drawing.Size(951, 426);
            this.uctrlLicenseInfo1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(0, 30);
            this.panel1.Controls.Add(this.llblShowNewLicensesInfo);
            this.panel1.Controls.Add(this.llblShowLicensesHistory);
            this.panel1.Controls.Add(this.btnReleaseDetain);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.uctrl_ReleaseDetainedLicense_Info1);
            this.panel1.Controls.Add(this.uctrlLicenseInfo1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1065, 588);
            this.panel1.TabIndex = 2;
            // 
            // llblShowNewLicensesInfo
            // 
            this.llblShowNewLicensesInfo.AutoSize = true;
            this.llblShowNewLicensesInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblShowNewLicensesInfo.Location = new System.Drawing.Point(296, 870);
            this.llblShowNewLicensesInfo.Name = "llblShowNewLicensesInfo";
            this.llblShowNewLicensesInfo.Size = new System.Drawing.Size(166, 18);
            this.llblShowNewLicensesInfo.TabIndex = 17;
            this.llblShowNewLicensesInfo.TabStop = true;
            this.llblShowNewLicensesInfo.Text = "Show New Licenses Info";
            this.llblShowNewLicensesInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowNewLicensesInfo_LinkClicked);
            // 
            // llblShowLicensesHistory
            // 
            this.llblShowLicensesHistory.AutoSize = true;
            this.llblShowLicensesHistory.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblShowLicensesHistory.Location = new System.Drawing.Point(117, 870);
            this.llblShowLicensesHistory.Name = "llblShowLicensesHistory";
            this.llblShowLicensesHistory.Size = new System.Drawing.Size(151, 18);
            this.llblShowLicensesHistory.TabIndex = 16;
            this.llblShowLicensesHistory.TabStop = true;
            this.llblShowLicensesHistory.Text = "Show Licenses History";
            this.llblShowLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowLicensesHistory_LinkClicked);
            // 
            // btnReleaseDetain
            // 
            this.btnReleaseDetain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReleaseDetain.Image = global::DVLD.Properties.Resources.icons8_world_32;
            this.btnReleaseDetain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReleaseDetain.Location = new System.Drawing.Point(826, 898);
            this.btnReleaseDetain.Name = "btnReleaseDetain";
            this.btnReleaseDetain.Size = new System.Drawing.Size(173, 50);
            this.btnReleaseDetain.TabIndex = 15;
            this.btnReleaseDetain.Text = "Release Detain";
            this.btnReleaseDetain.UseVisualStyleBackColor = true;
            this.btnReleaseDetain.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::DVLD.Properties.Resources.close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(634, 898);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(173, 50);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(276, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(493, 45);
            this.label1.TabIndex = 6;
            this.label1.Text = "Release Detained License";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtxtInput);
            this.groupBox1.Controls.Add(this.btnSearchLicense);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(23, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 102);
            this.groupBox1.TabIndex = 7;
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
            this.mtxtInput.DoubleClick += new System.EventHandler(this.mtxtInput_DoubleClick);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "License ID: ";
            // 
            // frm_ReleaseDetainedLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 588);
            this.Controls.Add(this.panel1);
            this.Name = "frm_ReleaseDetainedLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relase Detained License";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private User_Controls.Uctrl_ReleaseDetainedLicense_Info uctrl_ReleaseDetainedLicense_Info1;
        private User_Controls.uctrlLicenseInfo uctrlLicenseInfo1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox mtxtInput;
        private System.Windows.Forms.Button btnSearchLicense;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel llblShowNewLicensesInfo;
        private System.Windows.Forms.LinkLabel llblShowLicensesHistory;
        private System.Windows.Forms.Button btnReleaseDetain;
        private System.Windows.Forms.Button btnClose;
    }
}