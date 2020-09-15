using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Models;
using System.Configuration;
using System.Collections.Generic;

namespace DataAcessLayerTests
{
    [TestClass]
    public class DataAccessTest
    {
        private readonly IDataAccess _dataAccess;
        public DataAccessTest()
        {
            _dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString);
        }
        [TestMethod]
        public void GetById_OK()
        {
            int id = _dataAccess.GetByLastId().EmpId;
            Assert.IsNotNull(id);
            Assert.IsTrue(id > 0);
        }
        [TestMethod]
        public void GetById_NotOk_ThrowsException()
        {
            var exception = Assert.ThrowsException<Exception>(() => _dataAccess.GetById(-2));
            Assert.IsNotNull(exception);
        }
        [TestMethod]
        public void GetEmployees_Ok()
        {
            List<Employee> employees = _dataAccess.GetEmployees(new SearchModel());
            Assert.IsNotNull(employees);
            Assert.IsTrue(employees.Count > 0);
        }
        [TestMethod]
        public void GetEmployee_NotOk()
        {
            SearchModel search = new SearchModel { Search = "Sona", SearchBy = "EmpId" };
            var exception = Assert.ThrowsException<Exception>(() => _dataAccess.GetEmployees(search));
            Assert.IsNotNull(exception);
        }
        [TestMethod]
        public void Save_Add_OK()
        {
            var employee = _dataAccess.Save(new Employee { EmpId = 0, EmpName = "Sona", EmpAge = 23, EmpEmail = "ex@mail.ru", EmpSalary = 500000 });
            Assert.IsNotNull(employee);
            Assert.IsTrue(employee.EmpId > 0);
        }
        [TestMethod]
        public void Save_Edit_OK()
        {
            var employee = _dataAccess.Save(_dataAccess.GetByLastId());
            employee.EmpName = "Shushanik";
            var updatedEmployee = _dataAccess.Save(employee);
            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual(updatedEmployee.EmpName , "Shushanik");
        }
        [TestMethod]
        public void Save_NotOk_ID_DoesntExist()
        {
            var employee = new Employee { EmpId = 1, EmpName = "Sona", EmpAge = 23, EmpEmail = "ex@mail.ru", EmpSalary = 500000 };
            var exeception = Assert.ThrowsException<Exception>(() => _dataAccess.Save(employee));
            Assert.IsNotNull(exeception);
        }
        [TestMethod]
        public void Delete_OK()
        {
            Assert.IsTrue(_dataAccess.Delete(_dataAccess.GetByLastId().EmpId) is true);
        }
        [TestMethod]
        public void Delete_NotOk_ID_DoesntExist()
        {
            var exception = Assert.ThrowsException<Exception>(()=>_dataAccess.Delete(1));
            Assert.IsNotNull(exception);
        }
        [TestMethod]
        public void ExistingEmail_OK()
        {
            var employee = _dataAccess.GetByLastId();
            var Result = _dataAccess.ExistingEmail(employee);
            Assert.IsTrue(Result is false);
        }
        [TestMethod]
        public void ExistingEmail_NotOk()
        {
            var exception = Assert.ThrowsException<Exception>(() => _dataAccess.ExistingEmail(new Employee { EmpId = -2, EmpAge = 12, EmpEmail = "ex@mail.ru", EmpName = "Sona" , EmpSalary = 40000}));
            Assert.IsNotNull(exception);
               
        }
        [TestMethod]
        public void GetByLastId_Ok()
        {
            var employee = _dataAccess.GetByLastId();
            Assert.IsTrue(employee.EmpId > 0);
            Assert.IsNotNull(employee);
        }
 
    }
}
