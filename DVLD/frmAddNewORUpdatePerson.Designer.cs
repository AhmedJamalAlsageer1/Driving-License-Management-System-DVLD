namespace DVLD.Person_Forms
{
    partial class frmAddNewOrUpdatePerson
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
            this.llMode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llPersonID = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uctrlAddUpdatePerson1 = new DVLD.uctrlAddUpdatePerson();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // llMode
            // 
            this.llMode.AutoSize = true;
            this.llMode.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llMode.Location = new System.Drawing.Point(357, 22);
            this.llMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llMode.Name = "llMode";
            this.llMode.Size = new System.Drawing.Size(367, 57);
            this.llMode.TabIndex = 0;
            this.llMode.Text = "Add New Person";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "PersonID: ";
            // 
            // llPersonID
            // 
            this.llPersonID.AutoSize = true;
            this.llPersonID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llPersonID.Location = new System.Drawing.Point(211, 98);
            this.llPersonID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llPersonID.Name = "llPersonID";
            this.llPersonID.Size = new System.Drawing.Size(39, 18);
            this.llPersonID.TabIndex = 0;
            this.llPersonID.Text = "N/A";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD.Properties.Resources.application_32;
            this.pictureBox1.Location = new System.Drawing.Point(138, 91);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // uctrlAddUpdatePerson1
            // 
            this.uctrlAddUpdatePerson1.Location = new System.Drawing.Point(29, 123);
            this.uctrlAddUpdatePerson1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.uctrlAddUpdatePerson1.Name = "uctrlAddUpdatePerson1";
            this.uctrlAddUpdatePerson1.Size = new System.Drawing.Size(951, 358);
            this.uctrlAddUpdatePerson1.TabIndex = 2;
            // 
            // frmAddNewOrUpdatePerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 491);
            this.Controls.Add(this.uctrlAddUpdatePerson1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.llPersonID);
            this.Controls.Add(this.llMode);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmAddNewOrUpdatePerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddNewPerson";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label llMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label llPersonID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DVLD.uctrlAddUpdatePerson uctrlAddUpdatePerson1;
    }
}