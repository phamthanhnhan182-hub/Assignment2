using System.Data;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmStatistics : Form
    {
        private readonly StatisticBLL _bll = new StatisticBLL();

        public frmStatistics()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            Text = "Statistics";
            Width = 980;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label { Text = "Statistics", Left = 20, Top = 15, Width = 200, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            var lblHint = new Label { Text = "View sales and customer purchase summaries.", Left = 20, Top = 42, Width = 300 };
            var btnBack = new Button { Text = "Back to Main", Left = 20, Top = 530, Width = 120, Height = 32 };

            var tabControl = new TabControl { Left = 20, Top = 80, Width = 920, Height = 430 };
            tabControl.TabPages.Add(CreateTab("Best Items", _bll.GetBestItems()));
            tabControl.TabPages.Add(CreateTab("Items Purchased", _bll.GetItemsPurchasedByCustomers()));
            tabControl.TabPages.Add(CreateTab("Customer Summary", _bll.GetCustomerPurchaseSummary()));

            btnBack.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(tabControl);
            Controls.Add(btnBack);
        }

        private TabPage CreateTab(string title, DataTable dataSource)
        {
            var page = new TabPage(title);
            var btnRefresh = new Button { Text = "Refresh", Left = 10, Top = 10, Width = 80, Height = 30 };
            var dgv = new DataGridView
            {
                Left = 10,
                Top = 50,
                Width = 880,
                Height = 320,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = dataSource
            };

            btnRefresh.Click += (s, e) =>
            {
                if (title == "Best Items")
                    dgv.DataSource = _bll.GetBestItems();
                else if (title == "Items Purchased")
                    dgv.DataSource = _bll.GetItemsPurchasedByCustomers();
                else
                    dgv.DataSource = _bll.GetCustomerPurchaseSummary();

                FormatGrid(dgv);
            };

            page.Controls.Add(btnRefresh);
            page.Controls.Add(dgv);
            FormatGrid(dgv);
            return page;
        }

        private void FormatGrid(DataGridView dgv)
        {
            if (dgv.Columns.Contains("OrderDate")) dgv.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgv.Columns.Contains("UnitAmount")) dgv.Columns["UnitAmount"].DefaultCellStyle.Format = "N2";
        }
    }
}
