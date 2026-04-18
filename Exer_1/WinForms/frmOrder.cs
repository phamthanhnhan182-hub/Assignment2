using System;
using System.Data;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmOrder : Form
    {
        private readonly OrderBLL _bll = new OrderBLL();
        private ComboBox cboAgent;
        private DateTimePicker dtpOrderDate;
        private ComboBox cboItem;
        private TextBox txtQuantity;
        private TextBox txtUnitAmount;
        private DataGridView dgv;
        private Label lblTotal;
        private Button btnSave;
        private readonly DataTable _details = new DataTable();

        public frmOrder()
        {
            InitializeForm();
            InitializeControls();
            InitializeDetailTable();
            LoadComboboxes();
            UpdateTotal();
            UpdateSaveButton();
        }

        private void InitializeForm()
        {
            Text = "Manage Orders";
            Width = 930;
            Height = 610;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label { Text = "Manage Orders", Left = 20, Top = 15, Width = 200, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            var lblHint = new Label { Text = "Select an agent, add items, then save the order.", Left = 20, Top = 42, Width = 320 };

            var lblAgent = new Label { Text = "Agent", Left = 20, Top = 83, Width = 90 };
            var lblOrderDate = new Label { Text = "Order Date", Left = 20, Top = 118, Width = 90 };
            var lblItem = new Label { Text = "Item", Left = 20, Top = 173, Width = 90 };
            var lblQuantity = new Label { Text = "Quantity", Left = 390, Top = 173, Width = 70 };
            var lblUnitAmount = new Label { Text = "Unit Price", Left = 560, Top = 173, Width = 80 };

            cboAgent = new ComboBox { Left = 120, Top = 80, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            dtpOrderDate = new DateTimePicker { Left = 120, Top = 115, Width = 250, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
            cboItem = new ComboBox { Left = 120, Top = 170, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            txtQuantity = new TextBox { Left = 470, Top = 170, Width = 80 };
            txtUnitAmount = new TextBox { Left = 650, Top = 170, Width = 90 };

            var btnAddItem = new Button { Text = "Add Item", Left = 760, Top = 168, Width = 110, Height = 32 };
            var btnRemoveItem = new Button { Text = "Remove Selected", Left = 560, Top = 470, Width = 120, Height = 32 };
            var btnClear = new Button { Text = "Clear", Left = 690, Top = 470, Width = 80, Height = 32 };
            btnSave = new Button { Text = "Save Order", Left = 780, Top = 470, Width = 90, Height = 32 };
            var btnBack = new Button { Text = "Back to Main", Left = 20, Top = 520, Width = 120, Height = 32 };

            dgv = new DataGridView
            {
                Left = 20,
                Top = 220,
                Width = 850,
                Height = 220,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            lblTotal = new Label { Left = 20, Top = 475, Width = 250, Height = 25, Text = "Grand Total: 0.00" };

            btnAddItem.Click += BtnAddItem_Click;
            btnRemoveItem.Click += BtnRemoveItem_Click;
            btnClear.Click += (s, e) => ResetForm();
            btnSave.Click += BtnSave_Click;
            btnBack.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(lblAgent);
            Controls.Add(lblOrderDate);
            Controls.Add(lblItem);
            Controls.Add(lblQuantity);
            Controls.Add(lblUnitAmount);
            Controls.Add(cboAgent);
            Controls.Add(dtpOrderDate);
            Controls.Add(cboItem);
            Controls.Add(txtQuantity);
            Controls.Add(txtUnitAmount);
            Controls.Add(btnAddItem);
            Controls.Add(dgv);
            Controls.Add(lblTotal);
            Controls.Add(btnRemoveItem);
            Controls.Add(btnClear);
            Controls.Add(btnSave);
            Controls.Add(btnBack);
        }

        private void InitializeDetailTable()
        {
            _details.Columns.Add("ItemID", typeof(int));
            _details.Columns.Add("ItemName", typeof(string));
            _details.Columns.Add("Quantity", typeof(int));
            _details.Columns.Add("UnitAmount", typeof(decimal));
            _details.Columns.Add("LineTotal", typeof(decimal), "Quantity * UnitAmount");

            dgv.DataSource = _details;
            dgv.Columns["ItemID"].Visible = false;
            dgv.Columns["UnitAmount"].HeaderText = "Unit Price";
            dgv.Columns["UnitAmount"].DefaultCellStyle.Format = "N2";
            dgv.Columns["LineTotal"].HeaderText = "Line Total";
            dgv.Columns["LineTotal"].DefaultCellStyle.Format = "N2";
        }

        private void LoadComboboxes()
        {
            cboAgent.DataSource = _bll.GetAgents();
            cboAgent.DisplayMember = "AgentName";
            cboAgent.ValueMember = "AgentID";

            cboItem.DataSource = _bll.GetItems();
            cboItem.DisplayMember = "ItemName";
            cboItem.ValueMember = "ItemID";
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (cboItem.SelectedValue == null)
            {
                MessageBox.Show("Please select an item.", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0.", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            decimal unitAmount;
            if (!decimal.TryParse(txtUnitAmount.Text, out unitAmount) || unitAmount < 0)
            {
                MessageBox.Show("Unit price must be 0 or greater.", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitAmount.Focus();
                return;
            }

            int itemId = Convert.ToInt32(cboItem.SelectedValue);

            foreach (DataRow row in _details.Rows)
            {
                if (Convert.ToInt32(row["ItemID"]) == itemId)
                {
                    row["Quantity"] = Convert.ToInt32(row["Quantity"]) + quantity;
                    row["UnitAmount"] = unitAmount;
                    ClearItemInputs();
                    UpdateTotal();
                    UpdateSaveButton();
                    return;
                }
            }

            _details.Rows.Add(itemId, cboItem.Text, quantity, unitAmount);
            ClearItemInputs();
            UpdateTotal();
            UpdateSaveButton();
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0 || dgv.CurrentRow.Index >= _details.Rows.Count)
            {
                MessageBox.Show("Please select a detail row to remove.", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Remove selected detail line?", "Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _details.Rows.RemoveAt(dgv.CurrentRow.Index);
            UpdateTotal();
            UpdateSaveButton();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_details.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one item.", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int orderId = _bll.SaveOrder(dtpOrderDate.Value, Convert.ToInt32(cboAgent.SelectedValue), _details);
                MessageBox.Show("Order saved successfully. New Order ID = " + orderId, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearItemInputs()
        {
            txtQuantity.Clear();
            txtUnitAmount.Clear();
            cboItem.Focus();
        }

        private void ResetForm()
        {
            if (cboAgent.Items.Count > 0) cboAgent.SelectedIndex = 0;
            if (cboItem.Items.Count > 0) cboItem.SelectedIndex = 0;
            dtpOrderDate.Value = DateTime.Now;
            txtQuantity.Clear();
            txtUnitAmount.Clear();
            _details.Clear();
            UpdateTotal();
            UpdateSaveButton();
            cboAgent.Focus();
        }

        private void UpdateSaveButton()
        {
            btnSave.Enabled = _details.Rows.Count > 0;
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in _details.Rows)
                total += Convert.ToDecimal(row["LineTotal"]);

            lblTotal.Text = "Grand Total: " + total.ToString("N2");
        }
    }
}
