using guest.Models.Employee;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelativeInfo.Interfaces
{
    internal interface IEmployeeRepository
    {
        public List<Employee> ListAll(Int64 EmpId);
        bool AddEmployee(Employee employee);
    }
}
