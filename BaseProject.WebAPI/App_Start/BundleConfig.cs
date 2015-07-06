using System.Web;
using System.Web.Optimization;

namespace BaseProject.WebAPI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            #region Admin panel CSS and JS files

            bundles.Add(new StyleBundle("~/Content/AdminCss").Include(
                "~/Content/Admin/CSS/bootstrap.css",
                "~/Content/Admin/CSS/font-awesome.css",
                      "~/Content/Admin/CSS/ace-fonts.css",
                      "~/Content/Admin/CSS/ace.css",
                      "~/Content/Admin/CSS/main.css",
                      "~/Content/css/paging.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Content/Admin/CSS/main.js",
                    "~/Content/Admin/CSS/ace.js",
                    "~/Scripts/html5.js",
                    "~/Scripts/jquery-1.9.1.js",
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/jquery-migrate-1.2.1.js",                    
                    "~/Scripts/jquery.unobtrusive*",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/ajax-form.js",
                    "~/Scripts/UserDefinedScripts/Common.js",
                    "~/Scripts/UserDefinedScripts/paging.js",
                    "~/Scripts/modernizr-*"
             ));
            bundles.Add(new ScriptBundle("~/Content/Admin/ManageUsersManager").Include(
               "~/Scripts/UserDefinedScripts/Admin/ManageUsersManager.js"
                     ));

            bundles.Add(new ScriptBundle("~/Content/Admin/ManageJokesManager").Include(
                "~/Scripts/UserDefinedScripts/Admin/ManageJokesManager.js"
                      ));

            #endregion

        }
    }
}
