using System;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmLogin : Form
    {
        private readonly UserBLL _userBLL = new UserBLL();
        private TextBox txtUserName;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;

        public frmLogin()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            Text = "Login";
            Width = 420;
            Height = 230;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label
            {
                Text = "Login",
                Left = 30,
                Top = 15,
                Width = 200,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };

            var lblHint = new Label
            {
                Text = "Enter your account to continue.",
                Left = 30,
                Top = 42,
                Width = 250
            };

            var lblUserName = new Label { Text = "Username", Left = 30, Top = 78, Width = 90 };
            var lblPassword = new Label { Text = "Password", Left = 30, Top = 113, Width = 90 };

            txtUserName = new TextBox { Left = 130, Top = 75, Width = 220 };
            txtPassword = new TextBox { Left = 130, Top = 110, Width = 220, PasswordChar = '*' };

            btnLogin = new Button { Text = "Login", Left = 130, Top = 150, Width = 100, Height = 32 };
            btnExit = new Button { Text = "Exit", Left = 250, Top = 150, Width = 100, Height = 32 };

            btnLogin.Click += BtnLogin_Click;
            btnExit.Click += (s, e) => Close();

            AcceptButton = btnLogin;
            CancelButton = btnExit;

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(lblUserName);
            Controls.Add(lblPassword);
            Controls.Add(txtUserName);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            bool isValid = _userBLL.Login(txtUserName.Text, txtPassword.Text);

            if (!isValid)
            {
                MessageBox.Show("Invalid username/password or account is locked.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            Hide();
            using (var form = new frmMain())
            {
                form.ShowDialog();
            }
            Show();
            txtPassword.Clear();
            txtUserName.Focus();
        }
    }
}
