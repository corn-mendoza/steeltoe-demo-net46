﻿using Autofac;
using Autofac.Integration.Mvc;
using MySql4.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MySql4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterMySqlConnection(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Initialize some data in MySql
            SampleData.InitializeMySqlData(container);
        }
    }
}
