using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp
{
    internal class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }

        public Employee(int employeeid, string firstname, string lastname, string department, decimal salary)
        {
            EmployeeID = employeeid;
            FirstName = firstname;
            LastName = lastname;
            Department = department;
            Salary = salary;

        }
    }
}
