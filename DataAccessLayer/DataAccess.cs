using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Models;

namespace DataAccessLayer
{
    public class DataAccess : IDataAccess
    {
        //private string _connectionString = ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString;
        private string _connectionString;
        public DataAccess(string connectionStrng)
        {
            _connectionString = connectionStrng;
        }
        public IndexViewModel GetPagedList(List<Employee> listEmp, SearchModel srch)
        {
            IEnumerable<Employee> EmpForPerPages = listEmp.Skip((srch.Page - 1) * srch.PageSize).Take(srch.PageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = srch.Page, PageSize = srch.PageSize, TotalItems = listEmp.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Employees = EmpForPerPages };
            return ivm;
        }
        public Employee GetById(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" select * from Employee where  EmpId = @EmpId ", con);
                cmd.Parameters.AddWithValue("@EmpId", id);
                SqlDataReader read = cmd.ExecuteReader();
                if (read.HasRows)
                {
                    Employee emp = new Employee();
                    while (read.Read())
                    {
                        emp.EmpId = read.GetInt32(0);
                        emp.EmpName = read.GetString(1);
                        emp.EmpAge = read.GetInt32(2);
                        emp.EmpEmail = read.GetString(3);
                        emp.EmpSalary = read.GetInt32(4);
                    }
                    return emp;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public List<Employee> GetEmployees(SearchModel searchmodel)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sql = searchmodel.ToSqlQuery();
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandType = CommandType.Text;
                if (sql.Contains("@SearchByName") && sql.Contains("@Search"))
                {
                    command.Parameters.AddWithValue("@SearchByName", searchmodel.SearchBy);
                    command.Parameters.AddWithValue("@Search", searchmodel.Search);
                }
                con.Open();
                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        Employee emp = new Employee();
                        emp.EmpId = read.GetInt32(0);
                        emp.EmpName = read.GetString(1);
                        emp.EmpAge = read.GetInt32(2);
                        emp.EmpEmail = read.GetString(3);
                        emp.EmpSalary = read.GetInt32(4);
                        employees.Add(emp);
                    }
                    return employees;
                }
                else
                {
                    throw new Exception();
                }

            }
        }
        public Employee Save(Employee emp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("", con);
                con.Open();
                if (emp.EmpId == 0)
                {
                    sqlCommand.CommandText = "sp_AddEmployee";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@EmpName", emp.EmpName);
                    sqlCommand.Parameters.AddWithValue("@EmpAge", emp.EmpAge);
                    sqlCommand.Parameters.AddWithValue("@EmpEmail", emp.EmpEmail);
                    sqlCommand.Parameters.AddWithValue("@EmpSalary", emp.EmpSalary);
                }
                else if (emp.EmpId > 0)
                {
                    sqlCommand.CommandText = "sp_EditEmployee";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@EmpId", emp.EmpId);
                    sqlCommand.Parameters.AddWithValue("@EmpName", emp.EmpName);
                    sqlCommand.Parameters.AddWithValue("@EmpAge", emp.EmpAge);
                    sqlCommand.Parameters.AddWithValue("@EmpEmail", emp.EmpEmail);
                    sqlCommand.Parameters.AddWithValue("@EmpSalary", emp.EmpSalary);
                }
                else
                {
                    throw new Exception();
                }

                SqlDataReader read = sqlCommand.ExecuteReader();
                if (read.HasRows)
                {
                    Employee employee = new Employee();
                    while (read.Read())
                    {
                        employee.EmpId = read.GetInt32(0);
                        employee.EmpName = read.GetString(1);
                        employee.EmpAge = read.GetInt32(2);
                        employee.EmpEmail = read.GetString(3);
                        employee.EmpSalary = read.GetInt32(4);
                    }
                    return employee;
                }
                else
                {
                    throw new Exception("Somthing Went Wrong.Please,try again!");
                }
            }
        }
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("sp_DeleteProc", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EmpId", id);
                int affectedRow = sqlCommand.ExecuteNonQuery();
                if (affectedRow > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception();
                }

            }
        }
        public bool ExistingEmail(Employee emp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("", con);
                con.Open();
                if (emp.EmpId == 0)
                {
                    sqlCommand.CommandText = "select * from Employee where EmpEmail = @EmpEmail";
                    sqlCommand.Parameters.AddWithValue("@EmpEmail", emp.EmpEmail);
                    if (sqlCommand.ExecuteScalar() != null)
                    {
                        return true;
                    }
                    else return false;
                }
                else if (emp.EmpId > 0)
                {
                    sqlCommand.CommandText = "select * from Employee where EmpId != @EmpId and EmpEmail = @EmpEmail";
                    sqlCommand.Parameters.AddWithValue("@EmpId", emp.EmpId);
                    sqlCommand.Parameters.AddWithValue("@EmpEmail", emp.EmpEmail);
                    if (sqlCommand.ExecuteScalar() != null)
                    {
                        return true;
                    }
                    else return false;
                }
                else throw new Exception();
            }
        }
        public Employee GetByLastId()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employee WHERE EmpId = ( SELECT MAX(EmpId) FROM Employee)";
                SqlCommand sqlCommand = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader read = sqlCommand.ExecuteReader();
                if (read.HasRows)
                {
                    Employee emp = new Employee();
                    while (read.Read())
                    {
                        emp.EmpId = read.GetInt32(0);
                        emp.EmpName = read.GetString(1);
                        emp.EmpAge = read.GetInt32(2);
                        emp.EmpEmail = read.GetString(3);
                        emp.EmpSalary = read.GetInt32(4);
                    }
                    return emp;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
}

