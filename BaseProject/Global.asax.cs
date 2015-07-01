using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Extensions.Conventions;
using BaseProject.App_Start;

namespace BaseProject
{
    //public class MvcApplication : NinjectHttpApplication
    //{

    //    protected override IKernel CreateKernel()
    //    {
    //        var kernel = new StandardKernel();
    //        kernel.Load(Assembly.GetExecutingAssembly());


    //        kernel.Bind(x => x
    //        .FromAssembliesMatching("BusinessEntities.dll")
    //        .SelectAllClasses()
    //        .BindDefaultInterface());




    //        // kernel.Bind<ITestService>().To<TestService>();
    //        return kernel;
    //    }
    //    protected override void OnApplicationStarted()
    //    {
    //        base.OnApplicationStarted();
    //        AreaRegistration.RegisterAllAreas();
    //        GlobalConfiguration.Configure(WebApiConfig.Register);
    //        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        BundleConfig.RegisterBundles(BundleTable.Bundles);
    //    }
    //}

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //  NinjectWebCommon.Start();



        }
    }
}
