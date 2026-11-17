namespace DVLD
{
    partial class frmUserInformation
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
            this.uctrlUserInfo1 = new DVLD.User_Controls.uctrlUserInfo();
            this.SuspendLayout();
            // 
            // uctrlUserInfo1
            // 
            this.uctrlUserInfo1.Location = new System.Drawing.Point(0, 12);
            this.uctrlUserInfo1.Name = "uctrlUserInfo1";
            this.uctrlUserInfo1.Size = new System.Drawing.Size(785, 443);
            this.uctrlUserInfo1.TabIndex = 0;
            // 
            // frmUserInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 459);
            this.Controls.Add(this.uctrlUserInfo1);
            this.Name = "frmUserInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Information";
            this.ResumeLayout(false);

        }

        #endregion

        private User_Controls.uctrlUserInfo uctrlUserInfo1;
    }
}