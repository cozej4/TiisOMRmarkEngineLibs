using GIIS.DataLayer;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;

namespace GIIS.ScanForms.UserInterface
{
    partial class VaccineCorrection
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
            this.pnlView = new System.Windows.Forms.Panel();
            this.pbBarcode = new System.Windows.Forms.PictureBox();
            this.chkVaccA = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSendA = new System.Windows.Forms.CheckBox();
            this.dtpVaccA = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkSendB = new System.Windows.Forms.CheckBox();
            this.dtpVaccB = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.chkVaccB = new System.Windows.Forms.CheckedListBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblChildName = new System.Windows.Forms.Label();
            this.chkOutreachA = new System.Windows.Forms.CheckBox();
            this.chkOutreachB = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlView
            // 
            this.pnlView.AutoScroll = true;
            this.pnlView.Controls.Add(this.pbBarcode);
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlView.Location = new System.Drawing.Point(0, 38);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(606, 184);
            this.pnlView.TabIndex = 10;
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
            // chkVaccA
            // 
            this.chkVaccA.FormattingEnabled = true;
            this.chkVaccA.Location = new System.Drawing.Point(5, 67);
            this.chkVaccA.Name = "chkVaccA";
            this.chkVaccA.Size = new System.Drawing.Size(189, 64);
            this.chkVaccA.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(0, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(606, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "The following image represents what the scanner \"sees\". You may use it as a refer" +
    "ence when correcting the data below.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblChildName);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 280);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(197, 157);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Child Information";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(8, 57);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(186, 20);
            this.txtBarcode.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Barcode ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Child Name:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(606, 19);
            this.label2.TabIndex = 13;
            this.label2.Text = "Scanned Data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkOutreachA);
            this.groupBox2.Controls.Add(this.chkSendA);
            this.groupBox2.Controls.Add(this.dtpVaccA);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkVaccA);
            this.groupBox2.Location = new System.Drawing.Point(203, 280);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(197, 157);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vaccination Appointment 1";
            // 
            // chkSendA
            // 
            this.chkSendA.AutoSize = true;
            this.chkSendA.Checked = true;
            this.chkSendA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSendA.Location = new System.Drawing.Point(7, 21);
            this.chkSendA.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkSendA.Name = "chkSendA";
            this.chkSendA.Size = new System.Drawing.Size(77, 17);
            this.chkSendA.TabIndex = 18;
            this.chkSendA.Text = "Send Data";
            this.chkSendA.UseVisualStyleBackColor = true;
            this.chkSendA.CheckedChanged += new System.EventHandler(this.chkSendA_CheckedChanged);
            // 
            // dtpVaccA
            // 
            this.dtpVaccA.Location = new System.Drawing.Point(40, 45);
            this.dtpVaccA.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpVaccA.Name = "dtpVaccA";
            this.dtpVaccA.Size = new System.Drawing.Size(153, 20);
            this.dtpVaccA.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 45);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Date:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkOutreachB);
            this.groupBox3.Controls.Add(this.chkSendB);
            this.groupBox3.Controls.Add(this.dtpVaccB);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.chkVaccB);
            this.groupBox3.Location = new System.Drawing.Point(404, 280);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(197, 157);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vaccination Appointment 2";
            // 
            // chkSendB
            // 
            this.chkSendB.AutoSize = true;
            this.chkSendB.Location = new System.Drawing.Point(7, 21);
            this.chkSendB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkSendB.Name = "chkSendB";
            this.chkSendB.Size = new System.Drawing.Size(77, 17);
            this.chkSendB.TabIndex = 18;
            this.chkSendB.Text = "Send Data";
            this.chkSendB.UseVisualStyleBackColor = true;
            this.chkSendB.CheckedChanged += new System.EventHandler(this.chkSendB_CheckedChanged);
            // 
            // dtpVaccB
            // 
            this.dtpVaccB.Enabled = false;
            this.dtpVaccB.Location = new System.Drawing.Point(40, 45);
            this.dtpVaccB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpVaccB.Name = "dtpVaccB";
            this.dtpVaccB.Size = new System.Drawing.Size(153, 20);
            this.dtpVaccB.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Date:";
            // 
            // chkVaccB
            // 
            this.chkVaccB.Enabled = false;
            this.chkVaccB.FormattingEnabled = true;
            this.chkVaccB.Location = new System.Drawing.Point(5, 67);
            this.chkVaccB.Name = "chkVaccB";
            this.chkVaccB.Size = new System.Drawing.Size(189, 64);
            this.chkVaccB.TabIndex = 11;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(522, 443);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 21);
            this.btnSubmit.TabIndex = 20;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(444, 443);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 21);
            this.button2.TabIndex = 21;
            this.button2.Text = "Skip";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblChildName
            // 
            this.lblChildName.AutoSize = true;
            this.lblChildName.Location = new System.Drawing.Point(74, 21);
            this.lblChildName.Name = "lblChildName";
            this.lblChildName.Size = new System.Drawing.Size(35, 13);
            this.lblChildName.TabIndex = 17;
            this.lblChildName.Text = "label7";
            // 
            // chkOutreachA
            // 
            this.chkOutreachA.AutoSize = true;
            this.chkOutreachA.Location = new System.Drawing.Point(7, 135);
            this.chkOutreachA.Name = "chkOutreachA";
            this.chkOutreachA.Size = new System.Drawing.Size(70, 17);
            this.chkOutreachA.TabIndex = 19;
            this.chkOutreachA.Text = "Outreach";
            this.chkOutreachA.UseVisualStyleBackColor = true;
            // 
            // chkOutreachB
            // 
            this.chkOutreachB.AutoSize = true;
            this.chkOutreachB.Enabled = false;
            this.chkOutreachB.Location = new System.Drawing.Point(5, 135);
            this.chkOutreachB.Name = "chkOutreachB";
            this.chkOutreachB.Size = new System.Drawing.Size(70, 17);
            this.chkOutreachB.TabIndex = 20;
            this.chkOutreachB.Text = "Outreach";
            this.chkOutreachB.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(606, 19);
            this.label7.TabIndex = 22;
            this.label7.Text = "Scanned Data";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(0, 241);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(606, 34);
            this.label8.TabIndex = 23;
            this.label8.Text = "Use the area below to correct the form data interpreted from the scanner above. W" +
    "hen you are satisfied with the corrected data, pressing SUBMIT will send the dat" +
    "a to TIIS.";
            // 
            // VaccineCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 475);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VaccineCorrection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vaccination Event Verification";
            this.TopMost = true;
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.PictureBox pbBarcode;
        private System.Windows.Forms.CheckedListBox chkVaccA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSendA;
        private System.Windows.Forms.DateTimePicker dtpVaccA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkSendB;
        private System.Windows.Forms.DateTimePicker dtpVaccB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox chkVaccB;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblChildName;
        private System.Windows.Forms.CheckBox chkOutreachA;
        private System.Windows.Forms.CheckBox chkOutreachB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}