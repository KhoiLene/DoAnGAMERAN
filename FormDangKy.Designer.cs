partial class FormRegister
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.Label lblMessage;

    private void InitializeComponent()
    {
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblUsername = new System.Windows.Forms.Label();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.btnRegister = new System.Windows.Forms.Button();
        this.lblMessage = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // lblTitle
        // 
        this.lblTitle.Text = "Đăng ký tài khoản";
        this.lblTitle.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
        this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
        this.lblTitle.AutoSize = true;
        this.lblTitle.Location = new System.Drawing.Point(100, 20);
        // 
        // lblUsername
        // 
        this.lblUsername.Text = "Tên đăng nhập:";
        this.lblUsername.Location = new System.Drawing.Point(50, 70);
        this.lblUsername.AutoSize = true;
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(180, 70);
        this.txtUsername.Width = 150;
        // 
        // lblPassword
        // 
        this.lblPassword.Text = "Mật khẩu:";
        this.lblPassword.Location = new System.Drawing.Point(50, 110);
        this.lblPassword.AutoSize = true;
        // 
        // txtPassword
        // 
        this.txtPassword.Location = new System.Drawing.Point(180, 110);
        this.txtPassword.Width = 150;
        this.txtPassword.UseSystemPasswordChar = true;
        // 
        // btnRegister
        // 
        this.btnRegister.Text = "Đăng ký";
        this.btnRegister.Location = new System.Drawing.Point(150, 160);
        this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
        // 
        // lblMessage
        // 
        this.lblMessage.Location = new System.Drawing.Point(50, 200);
        this.lblMessage.ForeColor = System.Drawing.Color.Red;
        this.lblMessage.AutoSize = true;
        // 
        // FormRegister
        // 
        this.ClientSize = new System.Drawing.Size(400, 250);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblUsername);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.btnRegister);
        this.Controls.Add(this.lblMessage);
        this.Text = "Đăng ký";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}