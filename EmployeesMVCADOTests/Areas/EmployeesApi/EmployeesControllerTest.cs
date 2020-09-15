using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataAccessLayer;
using EmployeesMVCADO.Areas.EmployeesApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;

namespace EmployeesMVCADOTests.Areas.EmployeesApi
{
    [TestClass]
    public class EmployeesControllerTest
    {

        private readonly Mock<IDataAccess> _mockDataAccess;
        private readonly IDataAccess _dataAccess;
        public EmployeesControllerTest()
        {
            _mockDataAccess = new Mock<IDataAccess>();
            _dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString);
        }
        [TestMethod]
        public void Get_Ok()
        {
            //Arrange
            var controller = new EmployeesController(_dataAccess);

            //Act
            var result = controller.Get(new SearchModel());
            var okResult = result as OkNegotiatedContentResult<List<Employee>>;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Content.Count > 0);
        }
        [TestMethod]
        public void Should_Get_BadRequest_When_SearchModel_Is_Invalid()
        {
            //Arrange
            var controller = new EmployeesController(_dataAccess);

            //Act
            var result = controller.Get(new SearchModel() { SearchBy = "EmpId", Search = "1000" });
            var badResult = result as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(badResult);
            Assert.IsTrue(badResult.Message == "Somthing went Wrong,Please Try Again!");
        }
        [TestMethod]
        public void Should_Get_BadRequest_When_GetEmployees_ThrowsException()
        {
            //Arrange
            var searchmodel = new SearchModel() { SearchBy = "EmpId", Search = "1000" };
            _mockDataAccess.Setup(x => x.GetEmployees(searchmodel)).Throws(new Exception());
            var controller = new EmployeesController(_mockDataAccess.Object);
            //Act
            var result = controller.Get(searchmodel);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);
            Assert.IsTrue(badResult.Message == "Somthing went Wrong,Please Try Again!");
        }
        [TestMethod]
        public void Save_Add_Ok()
        {
            //Arrange
            var employee = new Employee { EmpId = 0, EmpName = "Artak", EmpAge = 21, EmpEmail = "art@mail.ru", EmpSalary = 1098888 };
            //Act
            var controller = new EmployeesController(_dataAccess);
            var result = controller.Save(employee);
            var okResult = result as OkNegotiatedContentResult<Employee>;
            //Assert            
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            //Assert.AreNotEqual(employee.EmpId, okResult.Content.EmpId);
            var newObj = _dataAccess.GetById(okResult.Content.EmpId);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.EmpName == employee.EmpName && newObj.EmpEmail == employee.EmpEmail);
            //_dataAccess.Delete(okResult.Content.EmpId);
        }
        [TestMethod]
        public void Save_Edit_Ok()
        {
            //Arrange
            var employee = _dataAccess.GetByLastId();
            //Act
            var controller = new EmployeesController(_dataAccess);
            int age = employee.EmpAge;
            employee.EmpAge = 155;
            var result = controller.Save(employee);
            var okResult = result as OkNegotiatedContentResult<Employee>;
            //Assert
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(employee.EmpId, okResult.Content.EmpId);
            //employee.EmpAge = age;
            //_dataAccess.Save(employee);
        }
        [TestMethod]
        public void Save_BadRequest_When_Employee_ISNull()
        {
            //Arrange            
            var controller = new EmployeesController(_dataAccess);
            //Act
            var result = controller.Save(null);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);
            Assert.IsTrue(badResult.Message == "Somthing Went Wrong, Please Try Again!");
        }
        [TestMethod]
        public void Save_BadReaquest_When_Save_ThrowsException()
        {
            //Arrange
            var emploee = new Employee { EmpId = 0, EmpAge = 52, EmpName = "Sona", EmpEmail = "Ss@mail.ru", EmpSalary = 900000 };
            _mockDataAccess.Setup(x => x.Save(emploee)).Throws(new Exception());
            var controller = new EmployeesController(_mockDataAccess.Object);
            //Act
            var result = controller.Save(emploee);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);
        }
        [TestMethod]
        public void Save_Add_BadRequet_When_ExistingEmail_IsTrue_ThrowException()
        {
            //Arrange
            var emploee = new Employee { EmpId = 0, EmpAge = 52, EmpName = "Sona", EmpEmail = "Sona@mail.ru", EmpSalary = 900000 };
            _mockDataAccess.Setup(x => x.ExistingEmail(emploee)).Throws(new Exception());
            var controller = new EmployeesController(_mockDataAccess.Object);
            //Act
            var result = controller.Save(emploee);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);
        }
        [TestMethod]
        public void Save_Edit_BadRequest_When_ExistingEmail_IsTrue_ThrowException()
        {
            //Arrange
            var emploee = _dataAccess.GetByLastId();
            _mockDataAccess.Setup(x => x.ExistingEmail(emploee)).Throws(new Exception());
            var controller = new EmployeesController(_mockDataAccess.Object);
            //Act
            var result = controller.Save(emploee);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);

        }
        [TestMethod]
        public void Delete_Ok()
        {
            //Arrange
            var employee = _dataAccess.GetByLastId();
            var controller = new EmployeesController(_dataAccess);
            //Act
            var result = controller.Delete(employee.EmpId);
            var goofResult = result as OkNegotiatedContentResult<string>;
            //Assert
            Assert.IsNotNull(goofResult.Content);
            Assert.AreEqual(goofResult.Content, "Your Data is Delted");
        }
        [TestMethod]
        public void Delete_BadRequest_When_Delete_ThrowException()
        {
            //Arrange
            var employee = _dataAccess.GetByLastId();
            _mockDataAccess.Setup(x => x.Delete(employee.EmpId)).Throws(new Exception());
            var controller = new EmployeesController(_mockDataAccess.Object);
            //Act
            var result = controller.Delete(employee.EmpId);
            var badResult = result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(badResult);
        }
        [TestMethod]
        public void Delete_BadRequest_When_ID_IsInValid()
        {
            //Arrange
            var employee = new Employee { EmpId = -2, EmpName = "Sona", EmpEmail = "So@mail.ru", EmpAge = 21 };
            var controller = new EmployeesController(_dataAccess);
            //Act
            var Result = controller.Delete(employee.EmpId);
            var basResult = Result as BadRequestErrorMessageResult;
            //Assert
            Assert.IsNotNull(basResult.Message);
        }

    }
}
