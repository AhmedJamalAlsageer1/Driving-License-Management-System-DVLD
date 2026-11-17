namespace DVLD.Police_Actions.Detain_License.Manage_Detained_License
{
    partial class frm_ManageDetainedLicense
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_DetainedLicenses = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowPersonDetailstoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowLicenseDetailstoolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowPersonLicenseHistorytoolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ReleaseDetainedLicensetoolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cbSearchType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReleaseDetainedLicense = new System.Windows.Forms.Button();
            this.btnDetainLicense = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbIsReleased = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.mtxtInput = new System.Windows.Forms.MaskedTextBox();
            this.lblRecord = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DetainedLicenses)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(490, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 34);
            this.label1.TabIndex = 23;
            this.label1.Text = "Mange Detained Licenses";
            // 
            // dgv_DetainedLicenses
            // 
            this.dgv_DetainedLicenses.AllowUserToAddRows = false;
            this.dgv_DetainedLicenses.AllowUserToDeleteRows = false;
            this.dgv_DetainedLicenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_DetainedLicenses.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv_DetainedLicenses.Location = new System.Drawing.Point(12, 350);
            this.dgv_DetainedLicenses.Name = "dgv_DetainedLicenses";
            this.dgv_DetainedLicenses.ReadOnly = true;
            this.dgv_DetainedLicenses.RowHeadersWidth = 51;
            this.dgv_DetainedLicenses.RowTemplate.Height = 26;
            this.dgv_DetainedLicenses.Size = new System.Drawing.Size(1336, 352);
            this.dgv_DetainedLicenses.TabIndex = 26;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowPersonDetailstoolStripMenuItem1,
            this.ShowLicenseDetailstoolStripMenuItem2,
            this.ShowPersonLicenseHistorytoolStripMenuItem3,
            this.ReleaseDetainedLicensetoolStripMenuItem4});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(281, 156);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // ShowPersonDetailstoolStripMenuItem1
            // 
            this.ShowPersonDetailstoolStripMenuItem1.Image = global::DVLD.Properties.Resources.patient_information_32;
            this.ShowPersonDetailstoolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowPersonDetailstoolStripMenuItem1.Name = "ShowPersonDetailstoolStripMenuItem1";
            this.ShowPersonDetailstoolStripMenuItem1.Size = new System.Drawing.Size(280, 38);
            this.ShowPersonDetailstoolStripMenuItem1.Text = "Show Person Details";
            this.ShowPersonDetailstoolStripMenuItem1.Click += new System.EventHandler(this.ShowPersonDetailstoolStripMenuItem1_Click);
            // 
            // ShowLicenseDetailstoolStripMenuItem2
            // 
            this.ShowLicenseDetailstoolStripMenuItem2.Image = global::DVLD.Properties.Resources.driver_license_32;
            this.ShowLicenseDetailstoolStripMenuItem2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowLicenseDetailstoolStripMenuItem2.Name = "ShowLicenseDetailstoolStripMenuItem2";
            this.ShowLicenseDetailstoolStripMenuItem2.Size = new System.Drawing.Size(280, 38);
            this.ShowLicenseDetailstoolStripMenuItem2.Text = "Show License Details";
            this.ShowLicenseDetailstoolStripMenuItem2.Click += new System.EventHandler(this.ShowLicenseDetailstoolStripMenuItem2_Click);
            // 
            // ShowPersonLicenseHistorytoolStripMenuItem3
            // 
            this.ShowPersonLicenseHistorytoolStripMenuItem3.Image = global::DVLD.Properties.Resources.icons8_driver_license_32;
            this.ShowPersonLicenseHistorytoolStripMenuItem3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowPersonLicenseHistorytoolStripMenuItem3.Name = "ShowPersonLicenseHistorytoolStripMenuItem3";
            this.ShowPersonLicenseHistorytoolStripMenuItem3.Size = new System.Drawing.Size(280, 38);
            this.ShowPersonLicenseHistorytoolStripMenuItem3.Text = "Show Person License History";
            this.ShowPersonLicenseHistorytoolStripMenuItem3.Click += new System.EventHandler(this.ShowPersonLicenseHistorytoolStripMenuItem3_Click);
            // 
            // ReleaseDetainedLicensetoolStripMenuItem4
            // 
            this.ReleaseDetainedLicensetoolStripMenuItem4.Image = global::DVLD.Properties.Resources.driving_license_32;
            this.ReleaseDetainedLicensetoolStripMenuItem4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ReleaseDetainedLicensetoolStripMenuItem4.Name = "ReleaseDetainedLicensetoolStripMenuItem4";
            this.ReleaseDetainedLicensetoolStripMenuItem4.Size = new System.Drawing.Size(280, 38);
            this.ReleaseDetainedLicensetoolStripMenuItem4.Text = "Release Detained License";
            this.ReleaseDetainedLicensetoolStripMenuItem4.Click += new System.EventHandler(this.ReleaseDetainedLicensetoolStripMenuItem4_Click);
            // 
            // cbSearchType
            // 
            this.cbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchType.FormattingEnabled = true;
            this.cbSearchType.Location = new System.Drawing.Point(113, 303);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(179, 24);
            this.cbSearchType.TabIndex = 25;
            this.cbSearchType.SelectedIndexChanged += new System.EventHandler(this.cbSearchType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 305);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "FindBy: ";
            // 
            // btnReleaseDetainedLicense
            // 
            this.btnReleaseDetainedLicense.BackColor = System.Drawing.SystemColors.Control;
            this.btnReleaseDetainedLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReleaseDetainedLicense.Image = global::DVLD.Properties.Resources.open_hand_64;
            this.btnReleaseDetainedLicense.Location = new System.Drawing.Point(1140, 250);
            this.btnReleaseDetainedLicense.Name = "btnReleaseDetainedLicense";
            this.btnReleaseDetainedLicense.Size = new System.Drawing.Size(88, 77);
            this.btnReleaseDetainedLicense.TabIndex = 28;
            this.btnReleaseDetainedLicense.UseVisualStyleBackColor = false;
            this.btnReleaseDetainedLicense.Click += new System.EventHandler(this.btnReleaseDetainedLicense_Click);
            // 
            // btnDetainLicense
            // 
            this.btnDetainLicense.BackColor = System.Drawing.SystemColors.Control;
            this.btnDetainLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetainLicense.Image = global::DVLD.Properties.Resources.hands_up_64;
            this.btnDetainLicense.Location = new System.Drawing.Point(1248, 250);
            this.btnDetainLicense.Name = "btnDetainLicense";
            this.btnDetainLicense.Size = new System.Drawing.Size(88, 77);
            this.btnDetainLicense.TabIndex = 27;
            this.btnDetainLicense.UseVisualStyleBackColor = false;
            this.btnDetainLicense.Click += new System.EventHandler(this.btnDetainLicense_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD.Properties.Resources.hand_265;
            this.pictureBox1.Location = new System.Drawing.Point(584, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(186, 187);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // cbIsReleased
            // 
            this.cbIsReleased.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIsReleased.FormattingEnabled = true;
            this.cbIsReleased.Location = new System.Drawing.Point(6, 5);
            this.cbIsReleased.Name = "cbIsReleased";
            this.cbIsReleased.Size = new System.Drawing.Size(82, 24);
            this.cbIsReleased.TabIndex = 29;
            this.cbIsReleased.SelectedIndexChanged += new System.EventHandler(this.cbIsReleased_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtInput);
            this.panel1.Controls.Add(this.cbIsReleased);
            this.panel1.Controls.Add(this.mtxtInput);
            this.panel1.Location = new System.Drawing.Point(297, 297);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 36);
            this.panel1.TabIndex = 30;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(6, 5);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(180, 24);
            this.txtInput.TabIndex = 31;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtInput_KeyDown);
            // 
            // mtxtInput
            // 
            this.mtxtInput.Location = new System.Drawing.Point(6, 5);
            this.mtxtInput.Name = "mtxtInput";
            this.mtxtInput.Size = new System.Drawing.Size(100, 24);
            this.mtxtInput.TabIndex = 32;
            this.mtxtInput.Click += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.Enter += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtInput_KeyDown);
            // 
            // lblRecord
            // 
            this.lblRecord.AutoSize = true;
            this.lblRecord.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecord.Location = new System.Drawing.Point(157, 714);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(21, 21);
            this.lblRecord.TabIndex = 32;
            this.lblRecord.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 714);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 21);
            this.label3.TabIndex = 31;
            this.label3.Text = "# Record: ";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(1207, 720);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 53);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frm_ManageDetainedLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 785);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblRecord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnReleaseDetainedLicense);
            this.Controls.Add(this.btnDetainLicense);
            this.Controls.Add(this.dgv_DetainedLicenses);
            this.Controls.Add(this.cbSearchType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frm_ManageDetainedLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_ManageDetainedLicense";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DetainedLicenses)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgv_DetainedLicenses;
        private System.Windows.Forms.ComboBox cbSearchType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDetainLicense;
        private System.Windows.Forms.Button btnReleaseDetainedLicense;
        private System.Windows.Forms.ComboBox cbIsReleased;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.MaskedTextBox mtxtInput;
        private System.Windows.Forms.Label lblRecord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ShowPersonDetailstoolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ShowLicenseDetailstoolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ShowPersonLicenseHistorytoolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ReleaseDetainedLicensetoolStripMenuItem4;
    }
}