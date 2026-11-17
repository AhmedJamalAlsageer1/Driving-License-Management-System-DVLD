namespace DVLD.License.Replacement_Or_Demage
{
    partial class frmReplacementForLostOrDamagedLicense
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtxtInput = new System.Windows.Forms.MaskedTextBox();
            this.btnSearchLicense = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gbReplacementFor = new System.Windows.Forms.GroupBox();
            this.rbLostLicense = new System.Windows.Forms.RadioButton();
            this.rbDemagedLicense = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.llblShowNewLicensesInfo = new System.Windows.Forms.LinkLabel();
            this.llblShowLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnIssueReplacement = new System.Windows.Forms.Button();
            this.uctrl_ApplicationForLicenseReplacement1 = new DVLD.User_Controls.uctrl_ApplicationForLicenseReplacement();
            this.uctrlLicenseInfo1 = new DVLD.User_Controls.uctrlLicenseInfo();
            this.groupBox1.SuspendLayout();
            this.gbReplacementFor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(236, 48);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(671, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Replacement For Damaged License";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtxtInput);
            this.groupBox1.Controls.Add(this.btnSearchLicense);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 102);
            this.groupBox1.TabIndex = 4;
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
            // gbReplacementFor
            // 
            this.gbReplacementFor.Controls.Add(this.rbLostLicense);
            this.gbReplacementFor.Controls.Add(this.rbDemagedLicense);
            this.gbReplacementFor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbReplacementFor.Location = new System.Drawing.Point(617, 135);
            this.gbReplacementFor.Name = "gbReplacementFor";
            this.gbReplacementFor.Size = new System.Drawing.Size(271, 102);
            this.gbReplacementFor.TabIndex = 5;
            this.gbReplacementFor.TabStop = false;
            this.gbReplacementFor.Text = "Replacement For";
            // 
            // rbLostLicense
            // 
            this.rbLostLicense.AutoSize = true;
            this.rbLostLicense.Location = new System.Drawing.Point(7, 69);
            this.rbLostLicense.Name = "rbLostLicense";
            this.rbLostLicense.Size = new System.Drawing.Size(108, 22);
            this.rbLostLicense.TabIndex = 1;
            this.rbLostLicense.TabStop = true;
            this.rbLostLicense.Text = "Lost License";
            this.rbLostLicense.UseVisualStyleBackColor = true;
            this.rbLostLicense.CheckedChanged += new System.EventHandler(this.rbLostLicense_CheckedChanged);
            // 
            // rbDemagedLicense
            // 
            this.rbDemagedLicense.AutoSize = true;
            this.rbDemagedLicense.Location = new System.Drawing.Point(7, 34);
            this.rbDemagedLicense.Name = "rbDemagedLicense";
            this.rbDemagedLicense.Size = new System.Drawing.Size(144, 22);
            this.rbDemagedLicense.TabIndex = 0;
            this.rbDemagedLicense.TabStop = true;
            this.rbDemagedLicense.Text = "Demaged License";
            this.rbDemagedLicense.UseVisualStyleBackColor = true;
            this.rbDemagedLicense.CheckedChanged += new System.EventHandler(this.rbDemagedLicense_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(0, 30);
            this.panel1.Controls.Add(this.llblShowNewLicensesInfo);
            this.panel1.Controls.Add(this.llblShowLicensesHistory);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnIssueReplacement);
            this.panel1.Controls.Add(this.uctrl_ApplicationForLicenseReplacement1);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gbReplacementFor);
            this.panel1.Controls.Add(this.uctrlLicenseInfo1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1073, 750);
            this.panel1.TabIndex = 7;
            // 
            // llblShowNewLicensesInfo
            // 
            this.llblShowNewLicensesInfo.AutoSize = true;
            this.llblShowNewLicensesInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblShowNewLicensesInfo.Location = new System.Drawing.Point(270, 871);
            this.llblShowNewLicensesInfo.Name = "llblShowNewLicensesInfo";
            this.llblShowNewLicensesInfo.Size = new System.Drawing.Size(166, 18);
            this.llblShowNewLicensesInfo.TabIndex = 11;
            this.llblShowNewLicensesInfo.TabStop = true;
            this.llblShowNewLicensesInfo.Text = "Show New Licenses Info";
            this.llblShowNewLicensesInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowNewLicensesInfo_LinkClicked);
            // 
            // llblShowLicensesHistory
            // 
            this.llblShowLicensesHistory.AutoSize = true;
            this.llblShowLicensesHistory.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblShowLicensesHistory.Location = new System.Drawing.Point(91, 871);
            this.llblShowLicensesHistory.Name = "llblShowLicensesHistory";
            this.llblShowLicensesHistory.Size = new System.Drawing.Size(151, 18);
            this.llblShowLicensesHistory.TabIndex = 10;
            this.llblShowLicensesHistory.TabStop = true;
            this.llblShowLicensesHistory.Text = "Show Licenses History";
            this.llblShowLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowLicensesHistory_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::DVLD.Properties.Resources.close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(617, 862);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(145, 51);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnIssueReplacement
            // 
            this.btnIssueReplacement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssueReplacement.Image = global::DVLD.Properties.Resources.driver_license_32;
            this.btnIssueReplacement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIssueReplacement.Location = new System.Drawing.Point(803, 862);
            this.btnIssueReplacement.Name = "btnIssueReplacement";
            this.btnIssueReplacement.Size = new System.Drawing.Size(236, 51);
            this.btnIssueReplacement.TabIndex = 8;
            this.btnIssueReplacement.Text = "Issue Replacement";
            this.btnIssueReplacement.UseVisualStyleBackColor = true;
            this.btnIssueReplacement.Click += new System.EventHandler(this.btnIssueReplacement_Click);
            // 
            // uctrl_ApplicationForLicenseReplacement1
            // 
            this.uctrl_ApplicationForLicenseReplacement1.Location = new System.Drawing.Point(12, 670);
            this.uctrl_ApplicationForLicenseReplacement1.Name = "uctrl_ApplicationForLicenseReplacement1";
            this.uctrl_ApplicationForLicenseReplacement1.Size = new System.Drawing.Size(993, 176);
            this.uctrl_ApplicationForLicenseReplacement1.TabIndex = 7;
            // 
            // uctrlLicenseInfo1
            // 
            this.uctrlLicenseInfo1.Location = new System.Drawing.Point(12, 243);
            this.uctrlLicenseInfo1.Name = "uctrlLicenseInfo1";
            this.uctrlLicenseInfo1.Size = new System.Drawing.Size(953, 421);
            this.uctrlLicenseInfo1.TabIndex = 6;
            // 
            // frmReplacementForLostOrDamagedLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 750);
            this.Controls.Add(this.panel1);
            this.Name = "frmReplacementForLostOrDamagedLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmReplacementForLostOrDamagedLicense";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbReplacementFor.ResumeLayout(false);
            this.gbReplacementFor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox mtxtInput;
        private System.Windows.Forms.Button btnSearchLicense;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbReplacementFor;
        private System.Windows.Forms.RadioButton rbLostLicense;
        private System.Windows.Forms.RadioButton rbDemagedLicense;
        private User_Controls.uctrlLicenseInfo uctrlLicenseInfo1;
        private System.Windows.Forms.Panel panel1;
        private User_Controls.uctrl_ApplicationForLicenseReplacement uctrl_ApplicationForLicenseReplacement1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnIssueReplacement;
        private System.Windows.Forms.LinkLabel llblShowNewLicensesInfo;
        private System.Windows.Forms.LinkLabel llblShowLicensesHistory;
    }
}