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
            this.btnOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkVaccines = new System.Windows.Forms.CheckedListBox();
            this.pnlView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlView
            // 
            this.pnlView.AutoScroll = true;
            this.pnlView.Controls.Add(this.pbBarcode);
            this.pnlView.Location = new System.Drawing.Point(5, 53);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(267, 229);
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
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(197, 401);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 285);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Vaccines GIVEN to child:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 41);
            this.label1.TabIndex = 6;
            this.label1.Text = "The following vaccination appointment was marked as having \"exceptions\". Please c" +
    "orrect the vaccinations given to the child and press OK";
            // 
            // chkVaccines
            // 
            this.chkVaccines.FormattingEnabled = true;
            this.chkVaccines.Location = new System.Drawing.Point(5, 301);
            this.chkVaccines.Name = "chkVaccines";
            this.chkVaccines.Size = new System.Drawing.Size(267, 94);
            this.chkVaccines.TabIndex = 11;
            // 
            // VaccineCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 431);
            this.Controls.Add(this.chkVaccines);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VaccineCorrection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vaccination Exception";
            this.TopMost = true;
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.PictureBox pbBarcode;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chkVaccines;
    }
}