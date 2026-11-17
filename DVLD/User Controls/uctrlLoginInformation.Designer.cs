namespace DVLD.User_Controls
{
    partial class uctrlLoginInformation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llActivity = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.llUserName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.llUserID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.llActivity);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.llUserName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.llUserID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 99);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login Information";
            // 
            // llActivity
            // 
            this.llActivity.AutoSize = true;
            this.llActivity.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llActivity.Location = new System.Drawing.Point(591, 48);
            this.llActivity.Name = "llActivity";
            this.llActivity.Size = new System.Drawing.Size(35, 18);
            this.llActivity.TabIndex = 5;
            this.llActivity.Text = "???";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(510, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Activity: ";
            // 
            // llUserName
            // 
            this.llUserName.AutoSize = true;
            this.llUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llUserName.Location = new System.Drawing.Point(386, 48);
            this.llUserName.Name = "llUserName";
            this.llUserName.Size = new System.Drawing.Size(35, 18);
            this.llUserName.TabIndex = 3;
            this.llUserName.Text = "???";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(285, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "UserName: ";
            // 
            // llUserID
            // 
            this.llUserID.AutoSize = true;
            this.llUserID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llUserID.Location = new System.Drawing.Point(161, 48);
            this.llUserID.Name = "llUserID";
            this.llUserID.Size = new System.Drawing.Size(35, 18);
            this.llUserID.TabIndex = 1;
            this.llUserID.Text = "???";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(89, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserID:";
            // 
            // uctrlLoginInformation
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "uctrlLoginInformation";
            this.Size = new System.Drawing.Size(669, 102);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label llActivity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label llUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label llUserID;
        private System.Windows.Forms.Label label1;
    }
}
