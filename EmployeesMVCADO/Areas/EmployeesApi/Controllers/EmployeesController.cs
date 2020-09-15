using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer;
using Models;

namespace EmployeesMVCADO.Areas.EmployeesApi.Controllers
{
    public class EmployeesController : ApiController
    {
        private readonly IDataAccess _dataAccess;
        public EmployeesController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public IHttpActionResult Get([FromUri]SearchModel model)
        {
            List<Employee> listEmp = new List<Employee>();
            try
            {
                listEmp = _dataAccess.GetEmployees(model);
            }
            catch
            {
                return BadRequest("Somthing went Wrong,Please Try Again!");
            }
            return Ok(listEmp);
        }
        [HttpPost]
        public IHttpActionResult Save(Employee employee)
        {
            if (employee == null)
                return BadRequest("Somthing Went Wrong, Please Try Again!");
            try
            {
                if (_dataAccess.ExistingEmail(employee) is false)
                    return Ok(_dataAccess.Save(employee));
                else throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception();

                if (_dataAccess.Delete(id) is true)
                    return Ok("Your Data is Delted");
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

