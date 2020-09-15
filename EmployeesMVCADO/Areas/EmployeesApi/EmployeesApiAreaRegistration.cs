using System.Web.Http;
using System.Web.Mvc;

namespace EmployeesMVCADO.Areas.EmployeesApi
{
    public class EmployeesApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EmployeesApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/EmployeesApi/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}