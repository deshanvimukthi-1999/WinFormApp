using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class MainForm : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESHAN\\SQLEXPRESS;Initial Catalog=EmployeeDB;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM Employees";
                dataAdapter = new SqlDataAdapter(query, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employees: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonAddEmployee_Click(object sender, EventArgs e)
        {
            AddEditEmployeeForm addEmployeeForm = new AddEditEmployeeForm(connection);
            if (addEmployeeForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployees();
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;
            if (selectedRowIndex >= 0)
            {
                int employeeId = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells["EmployeeId"].Value);
                AddEditEmployeeForm editEmployeeForm = new AddEditEmployeeForm(connection, employeeId);
                if (editEmployeeForm.ShowDialog() == DialogResult.OK)
                {
                    LoadEmployees();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;
            if (selectedRowIndex >= 0)
            {
                int employeeId = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells["EmployeeId"].Value);
                EmployeeDetailsForm employeeDetailsForm = new EmployeeDetailsForm(connection, employeeId);
                employeeDetailsForm.ShowDialog();
            }
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            string departmentFilter = textBoxDepartmentFilter.Text;
            dataTable.DefaultView.RowFilter = $"Department = '{departmentFilter}'";
        }

        private void buttonClearFilter_Click(object sender, EventArgs e)
        {
            textBoxDepartmentFilter.Text = string.Empty;
            dataTable.DefaultView.RowFilter = string.Empty;
        }

    }
}
