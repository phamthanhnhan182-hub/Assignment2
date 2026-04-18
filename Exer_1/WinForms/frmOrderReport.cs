using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmOrderReport : Form
    {
        private readonly ReportBLL _bll = new ReportBLL();
        private TextBox txtOrderId;
        private DataGridView dgv;
        private Label lblHeader;
        private DataTable _currentTable;
        private readonly PrintDocument _printDocument = new PrintDocument();

        public frmOrderReport()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            Text = "Order Report";
            Width = 930;
            Height = 590;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label { Text = "Order Report", Left = 20, Top = 15, Width = 200, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var lblHint = new Label { Text = "Enter an order ID to view and print the report.", Left = 20, Top = 42, Width = 300 };
            var lblOrderId = new Label { Text = "Order ID", Left = 20, Top = 83, Width = 70 };

            txtOrderId = new TextBox { Left = 100, Top = 80, Width = 140 };
            var btnLoad = new Button { Text = "Load", Left = 250, Top = 78, Width = 80, Height = 32 };
            var btnPrint = new Button { Text = "Print", Left = 340, Top = 78, Width = 80, Height = 32 };
            var btnClear = new Button { Text = "Clear", Left = 430, Top = 78, Width = 80, Height = 32 };
            var btnBack = new Button { Text = "Back to Main", Left = 20, Top = 500, Width = 120, Height = 32 };

            lblHeader = new Label { Left = 20, Top = 125, Width = 860, Height = 25, Text = "Order information will appear here" };

            dgv = new DataGridView
            {
                Left = 20,
                Top = 160,
                Width = 860,
                Height = 320,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnLoad.Click += BtnLoad_Click;
            btnPrint.Click += BtnPrint_Click;
            btnClear.Click += (s, e) => ClearReport();
            btnBack.Click += (s, e) => Close();
            txtOrderId.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLoad_Click(s, e); };
            _printDocument.PrintPage += PrintDocument_PrintPage;

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(lblOrderId);
            Controls.Add(txtOrderId);
            Controls.Add(btnLoad);
            Controls.Add(btnPrint);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(lblHeader);
            Controls.Add(dgv);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            int orderId;
            if (!int.TryParse(txtOrderId.Text, out orderId) || orderId <= 0)
            {
                MessageBox.Show("Please enter a valid order ID.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _currentTable = _bll.GetOrderReport(orderId);
            dgv.DataSource = _currentTable;
            FormatGrid();

            if (_currentTable.Rows.Count == 0)
            {
                lblHeader.Text = "Order not found.";
                return;
            }

            DataRow firstRow = _currentTable.Rows[0];
            lblHeader.Text = string.Format(
                "Order #{0} | Date: {1:dd/MM/yyyy HH:mm} | Agent: {2}",
                firstRow["OrderID"],
                firstRow["OrderDate"],
                firstRow["AgentName"]);
        }

        private void FormatGrid()
        {
            if (dgv.Columns.Contains("OrderDate")) dgv.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgv.Columns.Contains("UnitAmount")) dgv.Columns["UnitAmount"].DefaultCellStyle.Format = "N2";
            if (dgv.Columns.Contains("LineTotal")) dgv.Columns["LineTotal"].DefaultCellStyle.Format = "N2";
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (_currentTable == null || _currentTable.Rows.Count == 0)
            {
                MessageBox.Show("Please load an order first.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = _printDocument;
                preview.Width = 1000;
                preview.Height = 700;
                preview.ShowDialog();
            }
        }

        private void ClearReport()
        {
            txtOrderId.Clear();
            _currentTable = null;
            dgv.DataSource = null;
            lblHeader.Text = "Order information will appear here";
            txtOrderId.Focus();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_currentTable == null || _currentTable.Rows.Count == 0)
                return;

            float y = 40;
            var normalFont = new Font("Arial", 10);
            var boldFont = new Font("Arial", 12, FontStyle.Bold);
            DataRow firstRow = _currentTable.Rows[0];

            e.Graphics.DrawString("ORDER REPORT", boldFont, Brushes.Black, 40, y); y += 30;
            e.Graphics.DrawString("Order ID: " + firstRow["OrderID"], normalFont, Brushes.Black, 40, y); y += 20;
            e.Graphics.DrawString("Order Date: " + Convert.ToDateTime(firstRow["OrderDate"]).ToString("dd/MM/yyyy HH:mm"), normalFont, Brushes.Black, 40, y); y += 20;
            e.Graphics.DrawString("Agent: " + firstRow["AgentName"], normalFont, Brushes.Black, 40, y); y += 20;
            e.Graphics.DrawString("Address: " + firstRow["Address"], normalFont, Brushes.Black, 40, y); y += 30;

            e.Graphics.DrawString("Item", boldFont, Brushes.Black, 40, y);
            e.Graphics.DrawString("Qty", boldFont, Brushes.Black, 300, y);
            e.Graphics.DrawString("Unit", boldFont, Brushes.Black, 380, y);
            e.Graphics.DrawString("Total", boldFont, Brushes.Black, 480, y);
            y += 25;

            decimal grandTotal = 0;
            foreach (DataRow row in _currentTable.Rows)
            {
                decimal lineTotal = Convert.ToDecimal(row["LineTotal"]);
                grandTotal += lineTotal;

                e.Graphics.DrawString(Convert.ToString(row["ItemName"]), normalFont, Brushes.Black, 40, y);
                e.Graphics.DrawString(Convert.ToString(row["Quantity"]), normalFont, Brushes.Black, 300, y);
                e.Graphics.DrawString(Convert.ToDecimal(row["UnitAmount"]).ToString("N2"), normalFont, Brushes.Black, 380, y);
                e.Graphics.DrawString(lineTotal.ToString("N2"), normalFont, Brushes.Black, 480, y);
                y += 20;
            }

            y += 20;
            e.Graphics.DrawString("Grand Total: " + grandTotal.ToString("N2"), boldFont, Brushes.Black, 40, y);
        }
    }
}
