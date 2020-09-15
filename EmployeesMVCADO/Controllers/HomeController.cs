using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DataAccessLayer;
using PagedList;
namespace EmployeesMVCADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataAccess _dataAccess;
        public HomeController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public ActionResult Index(SearchModel srch)
        {
            List<Employee> listEmp = new List<Employee>();
            try
            {
                listEmp = _dataAccess.GetEmployees(srch);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Somthing went Wrong,Please Try Again!" + ex.Message;
            }
            ViewBag.SearchModel = srch;
            return View(_dataAccess.GetPagedList(listEmp, srch));
        }
        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            Employee emp = new Employee();
            if (id < 0)
                throw new Exception("Somthing Went Wrong,Please Try Again!");
            try
            {
                if (id > 0)
                {
                    emp = _dataAccess.GetById(id);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            ViewBag.Emp = emp;
            return View();
        }
        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            try
            {
                if (_dataAccess.ExistingEmail(emp) is false)
                {
                    emp = _dataAccess.Save(emp);
                    return RedirectToAction("Index");
                }
                else throw new Exception();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message + "Please Be More Carefull!";
                ViewBag.Emp = emp;
                return View();
            }
        }
        public JsonResult Delete(int id)
        {
            try
            {

                var pass = _dataAccess.Delete(id);
                return Json(new { Success = pass });

            }
            catch
            {
                return Json(new { Success = false });
            }

        }

    }
}