namespace DVLD.User_Controls
{
    partial class uctrlUserInfo
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
            this.uctrlPersonInfo1 = new DVLD.Person_Forms.uctrlPersonInfo();
            this.uctrlLoginInformation1 = new DVLD.User_Controls.uctrlLoginInformation();
            this.SuspendLayout();
            // 
            // uctrlPersonInfo1
            // 
            this.uctrlPersonInfo1.Location = new System.Drawing.Point(0, 0);
            this.uctrlPersonInfo1.Margin = new System.Windows.Forms.Padding(4);
            this.uctrlPersonInfo1.Name = "uctrlPersonInfo1";
            this.uctrlPersonInfo1.Size = new System.Drawing.Size(777, 320);
            this.uctrlPersonInfo1.TabIndex = 0;
            // 
            // uctrlLoginInformation1
            // 
            this.uctrlLoginInformation1.Location = new System.Drawing.Point(45, 327);
            this.uctrlLoginInformation1.Name = "uctrlLoginInformation1";
            this.uctrlLoginInformation1.Size = new System.Drawing.Size(745, 110);
            this.uctrlLoginInformation1.TabIndex = 1;
            // 
            // uctrlUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uctrlLoginInformation1);
            this.Controls.Add(this.uctrlPersonInfo1);
            this.Name = "uctrlUserInfo";
            this.Size = new System.Drawing.Size(793, 449);
            this.ResumeLayout(false);

        }

        #endregion

        private Person_Forms.uctrlPersonInfo uctrlPersonInfo1;
        private uctrlLoginInformation uctrlLoginInformation1;
    }
}
