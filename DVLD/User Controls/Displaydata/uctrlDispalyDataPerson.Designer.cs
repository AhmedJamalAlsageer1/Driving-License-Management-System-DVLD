namespace DVLD.User_Controls.Displaydata
{
    partial class uctrlDispalyDataPerson
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mtxtInput = new System.Windows.Forms.MaskedTextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnAddNewPerson = new System.Windows.Forms.Button();
            this.btnFindBy = new System.Windows.Forms.Button();
            this.cbFindBy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btnAddNewPerson);
            this.groupBox1.Controls.Add(this.btnFindBy);
            this.groupBox1.Controls.Add(this.cbFindBy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(697, 75);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.mtxtInput);
            this.panel1.Controls.Add(this.txtInput);
            this.panel1.Location = new System.Drawing.Point(301, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 29);
            this.panel1.TabIndex = 5;
            // 
            // mtxtInput
            // 
            this.mtxtInput.Location = new System.Drawing.Point(0, 0);
            this.mtxtInput.Mask = "0000000000000000000000000000000000";
            this.mtxtInput.Name = "mtxtInput";
            this.mtxtInput.PromptChar = ' ';
            this.mtxtInput.Size = new System.Drawing.Size(284, 26);
            this.mtxtInput.TabIndex = 1;
            this.mtxtInput.Click += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.DoubleClick += new System.EventHandler(this.mtxtInput_DoubleClick);
            this.mtxtInput.Enter += new System.EventHandler(this.mtxtInput_Click);
            this.mtxtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtInput_KeyDown);
            this.mtxtInput.Leave += new System.EventHandler(this.ControlInputs_Leave);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(0, 0);
            this.txtInput.Margin = new System.Windows.Forms.Padding(4);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(284, 26);
            this.txtInput.TabIndex = 1;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtInput_KeyDown);
            this.txtInput.Leave += new System.EventHandler(this.ControlInputs_Leave);
            // 
            // btnAddNewPerson
            // 
            this.btnAddNewPerson.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddNewPerson.FlatAppearance.BorderSize = 0;
            this.btnAddNewPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewPerson.Image = global::DVLD.Properties.Resources.administratorAdd_32;
            this.btnAddNewPerson.Location = new System.Drawing.Point(631, 27);
            this.btnAddNewPerson.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddNewPerson.Name = "btnAddNewPerson";
            this.btnAddNewPerson.Size = new System.Drawing.Size(33, 34);
            this.btnAddNewPerson.TabIndex = 4;
            this.btnAddNewPerson.UseVisualStyleBackColor = false;
            this.btnAddNewPerson.Click += new System.EventHandler(this.btnAddNewPerson_Click);
            // 
            // btnFindBy
            // 
            this.btnFindBy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFindBy.FlatAppearance.BorderSize = 0;
            this.btnFindBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindBy.Image = global::DVLD.Properties.Resources.administrator__30;
            this.btnFindBy.Location = new System.Drawing.Point(592, 27);
            this.btnFindBy.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindBy.Name = "btnFindBy";
            this.btnFindBy.Size = new System.Drawing.Size(33, 34);
            this.btnFindBy.TabIndex = 3;
            this.btnFindBy.UseVisualStyleBackColor = false;
            this.btnFindBy.Click += new System.EventHandler(this.btnFindBy_Click);
            // 
            // cbFindBy
            // 
            this.cbFindBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFindBy.FormattingEnabled = true;
            this.cbFindBy.Items.AddRange(new object[] {
            "Person ID",
            "National No",
            "Name"});
            this.cbFindBy.Location = new System.Drawing.Point(108, 32);
            this.cbFindBy.Margin = new System.Windows.Forms.Padding(4);
            this.cbFindBy.Name = "cbFindBy";
            this.cbFindBy.Size = new System.Drawing.Size(184, 26);
            this.cbFindBy.TabIndex = 2;
            this.cbFindBy.SelectedIndexChanged += new System.EventHandler(this.cbFindBy_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find By:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // uctrlDispalyDataPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "uctrlDispalyDataPerson";
            this.Size = new System.Drawing.Size(715, 85);
            this.Load += new System.EventHandler(this.uctrlDispalyDataPerson_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddNewPerson;
        private System.Windows.Forms.Button btnFindBy;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.ComboBox cbFindBy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mtxtInput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
