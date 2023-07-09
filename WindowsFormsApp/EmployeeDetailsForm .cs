using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class EmployeeDetailsForm : Form
    {
        private SqlConnection connection;
        private int employeeId;

        public EmployeeDetailsForm(SqlConnection connection, int employeeId)
        {
            InitializeComponent();
            this.connection = connection;
            this.employeeId = employeeId;
        }

        private void EmployeeDetailsForm_Load(object sender, EventArgs e)
        {
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            try
            {
                connection.Open();

                string query = $"SELECT * FROM Employees WHERE EmployeeId = {employeeId}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBoxFirstName.Text = reader["FirstName"].ToString();
                    textBoxLastName.Text = reader["LastName"].ToString();
                    textBoxDepartment.Text = reader["Department"].ToString();
                    textBoxSalary.Text = reader["Salary"].ToString();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee details: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
