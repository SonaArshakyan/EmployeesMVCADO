using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DataAccessLayer;

namespace EmployeesMVCADO.App_Start
{
    public class AutofacConfig
    {

        public static void Initilize(HttpConfiguration httpConfiguration)
        {
            var container = Initilize(httpConfiguration, new ContainerBuilder());
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static IContainer Initilize(HttpConfiguration httpConfiguration, ContainerBuilder container)
        {
            container.RegisterControllers(typeof(MvcApplication).Assembly);
            container.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //container.Register((ctx) =>
            //                {
            //                    return new DataAccess(ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString);
            //                })
            //         .As<IDataAccess>();

            container.RegisterType<DataAccess>()
                     .As<IDataAccess>()
                     .WithParameter(new NamedParameter("connectionStrng", ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString));
            return container.Build();
        }


    }
}