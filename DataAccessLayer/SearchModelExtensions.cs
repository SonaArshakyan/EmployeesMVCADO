using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer
{
    public static class SearchModelExtensions
    {
        private enum _Columns { EmpId, EmpName, EmpAge, EmpEmail, EmpSalary }

        public static string ToSqlQuery(this SearchModel searchmodel)
        {
            if (searchmodel.Search == null && searchmodel.SearchOrderBy == null)
            {
                return "select * from Employee ";
            }
            else if (searchmodel.Search != null && searchmodel.SearchOrderBy == null)
            {
                if (Enum.IsDefined(typeof(_Columns), searchmodel.SearchBy))
                {
                    return $"SELECT * FROM dbo.SearchModel(@SearchByName,@Search)";
                }
                else
                {
                    return " ";
                }
            }
            else if (searchmodel.Search != null && searchmodel.SearchOrderBy != null)
            {
                if (Enum.IsDefined(typeof(_Columns), searchmodel.SearchOrderBy) && Enum.IsDefined(typeof(SortDirection), searchmodel.sort)
                    && Enum.IsDefined(typeof(_Columns), searchmodel.SearchBy))
                {
                    return $"SELECT * FROM dbo.SearchModel(@SearchByName,@Search) order by {searchmodel.SearchOrderBy} {searchmodel.sort}";
                }
                else
                {
                    return " ";
                }
            }
            else if (searchmodel.SearchOrderBy != null && searchmodel.Search == null)
            {
                if (Enum.IsDefined(typeof(_Columns), searchmodel.SearchOrderBy) && Enum.IsDefined(typeof(SortDirection), searchmodel.sort))
                {
                    return $"select * from Employee order By {searchmodel.SearchOrderBy} {searchmodel.sort} ";
                }
                else
                {
                    return " ";
                }

            }
            else
            {
                return " ";
            }

        }

    }
}
