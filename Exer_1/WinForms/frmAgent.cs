using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using BLL;

namespace WinForms
{
    public class frmAgent : Form
    {
        private readonly AgentBLL _bll = new AgentBLL();
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtSearch;
        private DataGridView dgv;

        public frmAgent()
        {
            InitializeForm();
            InitializeControls();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "Manage Agents";
            Width = 800;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            var lblTitle = new Label { Text = "Manage Agents", Left = 20, Top = 15, Width = 200, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            var lblHint = new Label { Text = "Add, update, delete, or search agents.", Left = 20, Top = 42, Width = 260 };

            var lblId = new Label { Text = "Agent ID", Left = 20, Top = 83, Width = 80 };
            var lblName = new Label { Text = "Agent Name", Left = 20, Top = 118, Width = 80 };
            var lblAddress = new Label { Text = "Address", Left = 20, Top = 153, Width = 80 };
            var lblSearch = new Label { Text = "Search", Left = 470, Top = 83, Width = 60 };

            txtId = new TextBox { Left = 110, Top = 80, Width = 300, ReadOnly = true };
            txtName = new TextBox { Left = 110, Top = 115, Width = 300 };
            txtAddress = new TextBox { Left = 110, Top = 150, Width = 300 };
            txtSearch = new TextBox { Left = 540, Top = 80, Width = 220 };

            var btnAdd = new Button { Text = "Add", Left = 470, Top = 115, Width = 90, Height = 32 };
            var btnUpdate = new Button { Text = "Update", Left = 570, Top = 115, Width = 90, Height = 32 };
            var btnDelete = new Button { Text = "Delete", Left = 670, Top = 115, Width = 90, Height = 32 };
            var btnSearch = new Button { Text = "Search", Left = 540, Top = 150, Width = 100, Height = 32 };
            var btnClear = new Button { Text = "Clear", Left = 660, Top = 150, Width = 100, Height = 32 };
            var btnBack = new Button { Text = "Back to Main", Left = 20, Top = 430, Width = 120, Height = 32 };

            dgv = new DataGridView
            {
                Left = 20,
                Top = 200,
                Width = 740,
                Height = 210,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd.Click += (s, e) => SaveAgent(false);
            btnUpdate.Click += (s, e) => SaveAgent(true);
            btnDelete.Click += BtnDelete_Click;
            btnSearch.Click += (s, e) => SearchAgents();
            btnClear.Click += (s, e) => ClearForm(true);
            btnBack.Click += (s, e) => Close();
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchAgents(); };
            dgv.CellClick += Dgv_CellClick;

            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(lblId);
            Controls.Add(lblName);
            Controls.Add(lblAddress);
            Controls.Add(lblSearch);
            Controls.Add(txtId);
            Controls.Add(txtName);
            Controls.Add(txtAddress);
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
            if (dgv.Columns.Contains("AgentID")) dgv.Columns["AgentID"].HeaderText = "Agent ID";
            if (dgv.Columns.Contains("AgentName")) dgv.Columns["AgentName"].HeaderText = "Agent Name";
        }

        private void SearchAgents()
        {
            dgv.DataSource = _bll.Search(txtSearch.Text);
        }

        private void SaveAgent(bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter agent name.", "Agents", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter address.", "Agents", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            int id = 0;
            if (isUpdate && !int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("Please select an agent first.", "Agents", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = isUpdate
                    ? _bll.Update(id, txtName.Text, txtAddress.Text)
                    : _bll.Insert(txtName.Text, txtAddress.Text);

                MessageBox.Show(success
                    ? (isUpdate ? "Agent updated successfully." : "Agent added successfully.")
                    : "Operation failed.", "Agents");

                if (success)
                {
                    ClearForm(false);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Agents", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("Please select an agent first.", "Agents", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this agent?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bool success = _bll.Delete(id);
                MessageBox.Show(success ? "Agent deleted successfully." : "Delete failed.", "Agents");

                if (success)
                {
                    ClearForm(false);
                    LoadData();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("This agent already has orders and cannot be deleted.", "Agents", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Agents", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgv.Rows[e.RowIndex];
            txtId.Text = Convert.ToString(row.Cells["AgentID"].Value);
            txtName.Text = Convert.ToString(row.Cells["AgentName"].Value);
            txtAddress.Text = Convert.ToString(row.Cells["Address"].Value);
        }

        private void ClearForm(bool reloadData)
        {
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtSearch.Clear();

            if (reloadData)
                LoadData();

            txtName.Focus();
        }
    }
}
