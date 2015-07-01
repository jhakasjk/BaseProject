using System.Web.Mvc;

namespace PosWeb.Areas.CorporateSiteAdmin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_login",
                "Admin",
                new { action = "LogIn", controller = "Home", id = UrlParameter.Optional }, new string[] { "AJAD.Areas.Admin.Controllers" }
            );

          
            context.MapRoute(
                "AJADSiteAdmin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "{action}", controller = "{controller}", id = UrlParameter.Optional }, new string[] { "AJAD.Areas.Admin.Controllers" }
            );

        }
    }
}
