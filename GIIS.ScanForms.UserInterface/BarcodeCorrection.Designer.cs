namespace GIIS.ScanForms.UserInterface
{
    partial class BarcodeCorrection
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbBarcode = new System.Windows.Forms.PictureBox();
            this.pnlView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).BeginInit();
            this.pnlView.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "There was an error reading the barcode hilighted below:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please type the data located above or below the barcode:";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(12, 273);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(270, 20);
            this.txtBarcode.TabIndex = 3;
            this.txtBarcode.TextChanged += new System.EventHandler(this.txtBarcode_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(207, 299);
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
            this.pnlView.Size = new System.Drawing.Size(267, 229);
            this.pnlView.TabIndex = 5;
            // 
            // BarcodeCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 334);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BarcodeCorrection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Barcode Correction";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbBarcode)).EndInit();
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbBarcode;
        private System.Windows.Forms.Panel pnlView;
    }
}