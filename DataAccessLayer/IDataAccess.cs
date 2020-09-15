using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer
{
    public interface IDataAccess
    {
        Employee GetById(int id);
        List<Employee> GetEmployees(SearchModel searchmodel);
        Employee Save(Employee emp);
        bool Delete(int id);
        bool ExistingEmail(Employee emp);
        IndexViewModel GetPagedList(List<Employee> listEmp, SearchModel srch);
        Employee GetByLastId();

    }
}
