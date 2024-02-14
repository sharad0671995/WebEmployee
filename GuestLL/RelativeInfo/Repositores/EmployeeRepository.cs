using guest.Models.Employee;
using Microsoft.Extensions.Configuration;
using RelativeInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelativeInfo.Repositores
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly IConfiguration Configuration;
        string? cs = "";

        public EmployeeRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            //cs = Configuration["ConnectionStrings:UnioteqCheckSheetDBContext"];
            cs = Configuration.GetConnectionString("UnioteqCheckSheetDBConnection");
        }
        public List<Employee> ListAll(Int64 UserId)
        {
            List<Employee> lst = new List<Employee>();
            //lst.Add(new UserEntity());
            using (var con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("SP_Employee", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "SelectAll");
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();
                //Bind EmpModel generic list using dataRow     
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(
                           new Employee
                           {
                               EmpId = Convert.ToInt64(dr["EmpId"]),
                               Name = dr["Name"].ToString(),
                               Address = dr["Address"].ToString(),
                               ImagePath = dr["ImagePath"].ToString(),
                               
                           }
                        );
                }
            }
            return lst;
        }

        public bool AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SP_Employee", con);
                    SqlParameter outputIdParam = new SqlParameter("@EmpId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.AddWithValue("@Action", "SelectAll");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(outputIdParam);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Address", employee.Address);
                    cmd.Parameters.AddWithValue("@ImagePath", employee.ImagePath);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    int id = Convert.ToInt32(outputIdParam.Value);
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
       
    }

}
