namespace DoAnNhom_GAMERAN_
{
    partial class OTPDangKy
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
            this.lblOTP = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.Back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTP.Location = new System.Drawing.Point(42, 68);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(60, 25);
            this.lblOTP.TabIndex = 0;
            this.lblOTP.Text = "OTP";
            // 
            // btnContinue
            // 
            this.btnContinue.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(47, 125);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(300, 53);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Xác nhận";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // txtOTP
            // 
            this.txtOTP.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOTP.Location = new System.Drawing.Point(108, 65);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(239, 34);
            this.txtOTP.TabIndex = 2;
            // 
            // Back
            // 
            this.Back.BackColor = System.Drawing.SystemColors.Control;
            this.Back.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Back.Location = new System.Drawing.Point(12, 11);
            this.Back.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(80, 25);
            this.Back.TabIndex = 10;
            this.Back.Text = "Back";
            this.Back.UseVisualStyleBackColor = false;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // OTPDangKy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 223);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblOTP);
            this.Name = "OTPDangKy";
            this.Text = "OTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button Back;
    }
}