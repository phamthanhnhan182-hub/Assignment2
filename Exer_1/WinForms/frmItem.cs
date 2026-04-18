using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmItem : Form
    {
        private readonly ItemBLL _bll = new ItemBLL();
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtSize;
        private TextBox txtSearch;
        private DataGridView dgv;

        public frmItem()
        {
            InitializeForm();
            InitializeControls();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "Manage Items";
            Width = 760;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label { Text = "Manage Items", Left = 20, Top = 15, Width = 200, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            var lblHint = new Label { Text = "Add, update, delete, or search items.", Left = 20, Top = 42, Width = 250 };

            var lblId = new Label { Text = "Item ID", Left = 20, Top = 83, Width = 80 };
            var lblName = new Label { Text = "Item Name", Left = 20, Top = 118, Width = 80 };
            var lblSize = new Label { Text = "Size", Left = 20, Top = 153, Width = 80 };
            var lblSearch = new Label { Text = "Search", Left = 430, Top = 83, Width = 60 };

            txtId = new TextBox { Left = 110, Top = 80, Width = 250, ReadOnly = true };
            txtName = new TextBox { Left = 110, Top = 115, Width = 250 };
            txtSize = new TextBox { Left = 110, Top = 150, Width = 250 };
            txtSearch = new TextBox { Left = 500, Top = 80, Width = 200 };

            var btnAdd = new Button { Text = "Add", Left = 430, Top = 115, Width = 80, Height = 32 };
            var btnUpdate = new Button { Text = "Update", Left = 520, Top = 115, Width = 80, Height = 32 };
            var btnDelete = new Button { Text = "Delete", Left = 610, Top = 115, Width = 90, Height = 32 };
            var btnSearch = new Button { Text = "Search", Left = 500, Top = 150, Width = 90, Height = 32 };
            var btnClear = new Button { Text = "Clear", Left = 610, Top = 150, Width = 90, Height = 32 };
            var btnBack = new Button { Text = "Back to Main", Left = 20, Top = 430, Width = 120, Height = 32 };

            dgv = new DataGridView
            {
                Left = 20,
                Top = 200,
                Width = 680,
                Height = 210,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd.Click += (s, e) => SaveItem(false);
            btnUpdate.Click += (s, e) => SaveItem(true);
            btnDelete.Click += BtnDelete_Click;
            btnSearch.Click += (s, e) => SearchItems();
            btnClear.Click += (s, e) => ClearForm(true);
            btnBack.Click += (s, e) => Close();
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchItems(); };
            dgv.CellClick += Dgv_CellClick;

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(lblId);
            Controls.Add(lblName);
            Controls.Add(lblSize);
            Controls.Add(lblSearch);
            Controls.Add(txtId);
            Controls.Add(txtName);
            Controls.Add(txtSize);
            Controls.Add(txtSearch);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnSearch);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(dgv);
        }

        private void LoadData()
        {
            dgv.DataSource = _bll.GetAll();
            if (dgv.Columns.Contains("ItemID")) dgv.Columns["ItemID"].HeaderText = "Item ID";
            if (dgv.Columns.Contains("ItemName")) dgv.Columns["ItemName"].HeaderText = "Item Name";
        }

        private void SearchItems()
        {
            dgv.DataSource = _bll.Search(txtSearch.Text);
        }

        private void SaveItem(bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter item name.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSize.Text))
            {
                MessageBox.Show("Please enter size.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSize.Focus();
                return;
            }

            int id = 0;
            if (isUpdate && !int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("Please select an item first.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = isUpdate
                    ? _bll.Update(id, txtName.Text, txtSize.Text)
                    : _bll.Insert(txtName.Text, txtSize.Text);

                MessageBox.Show(success
                    ? (isUpdate ? "Item updated successfully." : "Item added successfully.")
                    : "Operation failed.", "Items");

                if (success)
                {
                    ClearForm(false);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Items", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("Please select an item first.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this item?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bool success = _bll.Delete(id);
                MessageBox.Show(success ? "Item deleted successfully." : "Delete failed.", "Items");

                if (success)
                {
                    ClearForm(false);
                    LoadData();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("This item is already used in orders and cannot be deleted.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Items", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgv.Rows[e.RowIndex];
            txtId.Text = Convert.ToString(row.Cells["ItemID"].Value);
            txtName.Text = Convert.ToString(row.Cells["ItemName"].Value);
            txtSize.Text = Convert.ToString(row.Cells["Size"].Value);
        }

        private void ClearForm(bool reloadData)
        {
            txtId.Clear();
            txtName.Clear();
            txtSize.Clear();
            txtSearch.Clear();

            if (reloadData)
                LoadData();

            txtName.Focus();
        }
    }
}
