namespace GIIS.ScanForms.UserInterface
{
    partial class ChildSearch
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbBarcode = new System.Windows.Forms.PictureBox();
            this.pnlView = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lsvResults = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpDob = new System.Windows.Forms.DateTimePicker();
            this.cbxGender = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxVillage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGiven = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.txtFamily = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpDobTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDob = new System.Windows.Forms.CheckBox();
            this.chkDobTo = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).BeginInit();
            this.pnlView.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(384, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use the information below to search for an appropriate child demographic record";
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(420, 138);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pbBarcode
            // 
            this.pbBarcode.Location = new System.Drawing.Point(3, 3);
            this.pbBarcode.Name = "pbBarcode";
            this.pbBarcode.Size = new System.Drawing.Size(177, 157);
            this.pbBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBarcode.TabIndex = 2;
            this.pbBarcode.TabStop = false;
            // 
            // pnlView
            // 
            this.pnlView.AutoScroll = true;
            this.pnlView.Controls.Add(this.pbBarcode);
            this.pnlView.Location = new System.Drawing.Point(15, 25);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(506, 201);
            this.pnlView.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(420, 134);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lsvResults);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Location = new System.Drawing.Point(15, 401);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 167);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Results";
            // 
            // lsvResults
            // 
            this.lsvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lsvResults.FullRowSelect = true;
            this.lsvResults.HideSelection = false;
            this.lsvResults.Location = new System.Drawing.Point(9, 19);
            this.lsvResults.MultiSelect = false;
            this.lsvResults.Name = "lsvResults";
            this.lsvResults.Size = new System.Drawing.Size(486, 113);
            this.lsvResults.TabIndex = 0;
            this.lsvResults.UseCompatibleStateImageBehavior = false;
            this.lsvResults.View = System.Windows.Forms.View.Details;
            this.lsvResults.SelectedIndexChanged += new System.EventHandler(this.lsvResults_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID#";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Family Name";
            this.columnHeader2.Width = 116;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Given Name";
            this.columnHeader3.Width = 104;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Gender";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Village";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Date of Birth";
            this.columnHeader6.Width = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDobTo);
            this.groupBox2.Controls.Add(this.chkDob);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpDobTo);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.dtpDob);
            this.groupBox2.Controls.Add(this.cbxGender);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbxVillage);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtGiven);
            this.groupBox2.Controls.Add(this.txtBarcode);
            this.groupBox2.Controls.Add(this.txtFamily);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Location = new System.Drawing.Point(15, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(506, 163);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Criteria";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 61);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Date of Birth";
            // 
            // dtpDob
            // 
            this.dtpDob.Enabled = false;
            this.dtpDob.Location = new System.Drawing.Point(96, 60);
            this.dtpDob.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDob.Name = "dtpDob";
            this.dtpDob.Size = new System.Drawing.Size(136, 20);
            this.dtpDob.TabIndex = 14;
            // 
            // cbxGender
            // 
            this.cbxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGender.FormattingEnabled = true;
            this.cbxGender.Items.AddRange(new object[] {
            "Female",
            "Male"});
            this.cbxGender.Location = new System.Drawing.Point(76, 84);
            this.cbxGender.Margin = new System.Windows.Forms.Padding(2);
            this.cbxGender.Name = "cbxGender";
            this.cbxGender.Size = new System.Drawing.Size(136, 21);
            this.cbxGender.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 84);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Gender";
            // 
            // cbxVillage
            // 
            this.cbxVillage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxVillage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxVillage.FormattingEnabled = true;
            this.cbxVillage.Location = new System.Drawing.Point(76, 108);
            this.cbxVillage.Margin = new System.Windows.Forms.Padding(2);
            this.cbxVillage.Name = "cbxVillage";
            this.cbxVillage.Size = new System.Drawing.Size(419, 21);
            this.cbxVillage.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 108);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Village";
            // 
            // txtGiven
            // 
            this.txtGiven.Location = new System.Drawing.Point(325, 38);
            this.txtGiven.Margin = new System.Windows.Forms.Padding(2);
            this.txtGiven.Name = "txtGiven";
            this.txtGiven.Size = new System.Drawing.Size(171, 20);
            this.txtGiven.TabIndex = 13;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(76, 17);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(2);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(171, 20);
            this.txtBarcode.TabIndex = 11;
            // 
            // txtFamily
            // 
            this.txtFamily.Location = new System.Drawing.Point(76, 38);
            this.txtFamily.Margin = new System.Windows.Forms.Padding(2);
            this.txtFamily.Name = "txtFamily";
            this.txtFamily.Size = new System.Drawing.Size(171, 20);
            this.txtFamily.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Barcode";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(254, 40);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Given Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 40);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Family Name";
            // 
            // dtpDobTo
            // 
            this.dtpDobTo.Enabled = false;
            this.dtpDobTo.Location = new System.Drawing.Point(277, 60);
            this.dtpDobTo.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDobTo.Name = "dtpDobTo";
            this.dtpDobTo.Size = new System.Drawing.Size(136, 20);
            this.dtpDobTo.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "to";
            // 
            // chkDob
            // 
            this.chkDob.AutoSize = true;
            this.chkDob.Location = new System.Drawing.Point(76, 63);
            this.chkDob.Name = "chkDob";
            this.chkDob.Size = new System.Drawing.Size(15, 14);
            this.chkDob.TabIndex = 24;
            this.chkDob.UseVisualStyleBackColor = true;
            this.chkDob.CheckedChanged += new System.EventHandler(this.chkDob_CheckedChanged);
            // 
            // chkDobTo
            // 
            this.chkDobTo.AutoSize = true;
            this.chkDobTo.Location = new System.Drawing.Point(257, 63);
            this.chkDobTo.Name = "chkDobTo";
            this.chkDobTo.Size = new System.Drawing.Size(15, 14);
            this.chkDobTo.TabIndex = 25;
            this.chkDobTo.UseVisualStyleBackColor = true;
            this.chkDobTo.CheckedChanged += new System.EventHandler(this.chkDobTo_CheckedChanged);
            // 
            // ChildSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 580);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChildSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Child Search";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).EndInit();
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbBarcode;
        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lsvResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpDob;
        private System.Windows.Forms.ComboBox cbxGender;
        private System.Windows.Forms.ComboBox cbxVillage;
        private System.Windows.Forms.TextBox txtGiven;
        private System.Windows.Forms.TextBox txtFamily;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpDobTo;
        private System.Windows.Forms.CheckBox chkDobTo;
        private System.Windows.Forms.CheckBox chkDob;
    }
}