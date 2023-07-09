using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class AddEditEmployeeForm : Form
    {
        private SqlConnection connection;
        private int employeeId;
        private bool isEditMode;

        public AddEditEmployeeForm(SqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            isEditMode = false;
        }

        public AddEditEmployeeForm(SqlConnection connection, int employeeId)
        {
            InitializeComponent();
            this.connection = connection;
            this.employeeId = employeeId;
            isEditMode = true;
        }

        private void AddEditEmployeeForm_Load(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                LoadEmployeeDetails();
                Text = "Edit Employee";
                buttonAddUpdate.Text = "Update";
            }
            else
            {
                Text = "Add Employee";
                buttonAddUpdate.Text = "Add";
            }
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

        private void buttonAddUpdate_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string lastName = textBoxLastName.Text.Trim();
            string department = textBoxDepartment.Text.Trim();
            decimal salary;

            if (!decimal.TryParse(textBoxSalary.Text.Trim(), out salary))
            {
                MessageBox.Show("Invalid salary value. Please enter a valid decimal number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isEditMode)
            {
                UpdateEmployee(firstName, lastName, department, salary);
            }
            else
            {
                AddEmployee(firstName, lastName, department, salary);
            }
        }

        private void AddEmployee(string firstName, string lastName, string department, decimal salary)
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO Employees (FirstName, LastName, Department, Salary) VALUES (@FirstName, @LastName, @Department, @Salary)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@Salary", salary);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Employee added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Employee not inserted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateEmployee(string firstName, string lastName, string department, decimal salary)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Department = @Department, Salary = @Salary WHERE EmployeeId = @EmployeeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Employee updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Employee update not completed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
