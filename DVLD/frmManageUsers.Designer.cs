namespace DVLD
{
    partial class frmManageUsers
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showUserInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateUserInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFindUserBy = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mtxtSearch = new System.Windows.Forms.MaskedTextBox();
            this.panelInputs = new System.Windows.Forms.Panel();
            this.cbSearchActive = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelInputs.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(0, 611);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1661, 361);
            this.dataGridView1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showUserInformationToolStripMenuItem,
            this.updateUserInformationToolStripMenuItem,
            this.deleteUserToolStripMenuItem,
            this.changePasswordToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(360, 156);
            // 
            // showUserInformationToolStripMenuItem
            // 
            this.showUserInformationToolStripMenuItem.Image = global::DVLD.Properties.Resources.user_Information_16;
            this.showUserInformationToolStripMenuItem.Name = "showUserInformationToolStripMenuItem";
            this.showUserInformationToolStripMenuItem.Size = new System.Drawing.Size(359, 38);
            this.showUserInformationToolStripMenuItem.Text = "Show User Information";
            this.showUserInformationToolStripMenuItem.Click += new System.EventHandler(this.showUserInformationToolStripMenuItem_Click);
            // 
            // updateUserInformationToolStripMenuItem
            // 
            this.updateUserInformationToolStripMenuItem.Image = global::DVLD.Properties.Resources.Edit_User_16;
            this.updateUserInformationToolStripMenuItem.Name = "updateUserInformationToolStripMenuItem";
            this.updateUserInformationToolStripMenuItem.Size = new System.Drawing.Size(359, 38);
            this.updateUserInformationToolStripMenuItem.Text = "Update User Information";
            this.updateUserInformationToolStripMenuItem.Click += new System.EventHandler(this.updateUserInformationToolStripMenuItem_Click);
            // 
            // deleteUserToolStripMenuItem
            // 
            this.deleteUserToolStripMenuItem.Image = global::DVLD.Properties.Resources.user_close_16;
            this.deleteUserToolStripMenuItem.Name = "deleteUserToolStripMenuItem";
            this.deleteUserToolStripMenuItem.Size = new System.Drawing.Size(359, 38);
            this.deleteUserToolStripMenuItem.Text = "Delete User";
            this.deleteUserToolStripMenuItem.Click += new System.EventHandler(this.deleteUserToolStripMenuItem_Click);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Image = global::DVLD.Properties.Resources.password_16;
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(359, 38);
            this.changePasswordToolStripMenuItem.Text = "ChangePassword";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(573, 359);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 89);
            this.label1.TabIndex = 2;
            this.label1.Text = "Manage Users";
            // 
            // cbFindUserBy
            // 
            this.cbFindUserBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFindUserBy.FormattingEnabled = true;
            this.cbFindUserBy.Items.AddRange(new object[] {
            "None",
            "UserID",
            "PersonID",
            "FullName",
            "UserName",
            "Active"});
            this.cbFindUserBy.Location = new System.Drawing.Point(154, 450);
            this.cbFindUserBy.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.cbFindUserBy.Name = "cbFindUserBy";
            this.cbFindUserBy.Size = new System.Drawing.Size(299, 33);
            this.cbFindUserBy.TabIndex = 3;
            this.cbFindUserBy.SelectedIndexChanged += new System.EventHandler(this.cbFindUserBy_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 453);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Filter By:";
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::DVLD.Properties.Resources.activate_profile_60;
            this.btnAdd.Location = new System.Drawing.Point(1503, 394);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(137, 116);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::DVLD.Properties.Resources.activate_profile_180;
            this.pictureBox1.Location = new System.Drawing.Point(662, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 308);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // mtxtSearch
            // 
            this.mtxtSearch.Location = new System.Drawing.Point(7, 6);
            this.mtxtSearch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.mtxtSearch.Mask = "000000000000000";
            this.mtxtSearch.Name = "mtxtSearch";
            this.mtxtSearch.PromptChar = ' ';
            this.mtxtSearch.Size = new System.Drawing.Size(361, 33);
            this.mtxtSearch.TabIndex = 7;
            this.mtxtSearch.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtxtSearch.Visible = false;
            this.mtxtSearch.Click += new System.EventHandler(this.mtxtSearch_Click);
            this.mtxtSearch.Enter += new System.EventHandler(this.mtxtSearch_Enter);
            this.mtxtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtSearch_KeyDown);
            this.mtxtSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mtxtSearch_MouseDown);
            // 
            // panelInputs
            // 
            this.panelInputs.Controls.Add(this.mtxtSearch);
            this.panelInputs.Controls.Add(this.cbSearchActive);
            this.panelInputs.Controls.Add(this.txtSearch);
            this.panelInputs.Location = new System.Drawing.Point(470, 448);
            this.panelInputs.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelInputs.Name = "panelInputs";
            this.panelInputs.Size = new System.Drawing.Size(408, 50);
            this.panelInputs.TabIndex = 9;
            // 
            // cbSearchActive
            // 
            this.cbSearchActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchActive.FormattingEnabled = true;
            this.cbSearchActive.Items.AddRange(new object[] {
            "All",
            "Yes",
            "No"});
            this.cbSearchActive.Location = new System.Drawing.Point(7, 6);
            this.cbSearchActive.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.cbSearchActive.Name = "cbSearchActive";
            this.cbSearchActive.Size = new System.Drawing.Size(237, 33);
            this.cbSearchActive.TabIndex = 7;
            this.cbSearchActive.Visible = false;
            this.cbSearchActive.SelectedIndexChanged += new System.EventHandler(this.cbSearchActive_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(7, 6);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(331, 33);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.Visible = false;
            // 
            // frmManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1661, 972);
            this.Controls.Add(this.panelInputs);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbFindUserBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "frmManageUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmManageUsers";
            this.Load += new System.EventHandler(this.frmManageUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelInputs.ResumeLayout(false);
            this.panelInputs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFindUserBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showUserInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateUserInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.MaskedTextBox mtxtSearch;
        private System.Windows.Forms.Panel panelInputs;
        private System.Windows.Forms.ComboBox cbSearchActive;
        private System.Windows.Forms.TextBox txtSearch;
    }
}