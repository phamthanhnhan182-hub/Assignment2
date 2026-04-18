using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms
{
    public class frmMain : Form
    {
        public frmMain()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            Text = "Main";
            Width = 500;
            Height = 360;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label
            {
                Text = "Order Management System",
                Left = 30,
                Top = 20,
                Width = 300,
                Font = new Font("Segoe UI", 13, FontStyle.Bold)
            };

            var lblHint = new Label
            {
                Text = "Choose a function below.",
                Left = 30,
                Top = 50,
                Width = 220
            };

            var btnItem = new Button { Text = "Manage Items", Left = 40, Top = 90, Width = 180, Height = 40 };
            var btnAgent = new Button { Text = "Manage Agents", Left = 250, Top = 90, Width = 180, Height = 40 };
            var btnOrder = new Button { Text = "Manage Orders", Left = 40, Top = 145, Width = 180, Height = 40 };
            var btnReport = new Button { Text = "Order Report", Left = 250, Top = 145, Width = 180, Height = 40 };
            var btnStatistics = new Button { Text = "Statistics", Left = 40, Top = 200, Width = 180, Height = 40 };
            var btnClose = new Button { Text = "Close", Left = 250, Top = 200, Width = 180, Height = 40 };

            btnItem.Click += (s, e) => new frmItem().ShowDialog();
            btnAgent.Click += (s, e) => new frmAgent().ShowDialog();
            btnOrder.Click += (s, e) => new frmOrder().ShowDialog();
            btnReport.Click += (s, e) => new frmOrderReport().ShowDialog();
            btnStatistics.Click += (s, e) => new frmStatistics().ShowDialog();
            btnClose.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(btnItem);
            Controls.Add(btnAgent);
            Controls.Add(btnOrder);
            Controls.Add(btnReport);
            Controls.Add(btnStatistics);
            Controls.Add(btnClose);
        }
    }
}
