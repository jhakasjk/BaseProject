using CoreEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Filters;

namespace BaseProject.WebAPI.Attributes
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IHomeManager _homeManager;

        public ApiExceptionAttribute() { }

        public ApiExceptionAttribute(IHomeManager homeManager)
        {
            _homeManager = homeManager;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            base.OnException(context);
            var ex = context.Exception as HttpResponseException;
            var error_id = LogExceptionToDatabase(context.Exception);
            if (ex != null)
            {
                if (ex.Response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    context.Response = new HttpResponseMessage(HttpStatusCode.OK);
                    context.Response.Content = new StringContent(ex.ToString());

                    //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);

                }
            }
        }



        /** 
        * Purpose  :   logges the exception in the database
        * Inputs   :   ex – details of the exception
        */
        public String LogExceptionToDatabase(Exception ex)
        {
            String ex_text = String.Empty;
            String ex_message = ex.ToString();
            try
            {   
                return _homeManager.LogExceptionToDatabase(ex);

            }
            catch (Exception)
            {
                LogExceptionToFile(ex_text, ex_message);
                return "0";
            }
        }

        /// <summary>
        /// This Will be used to log exception to a file
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ex_message"></param>
        public static void LogExceptionToFile(String ex, String ex_message)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                if (!System.IO.Directory.Exists(HostingEnvironment.MapPath("/Logs")))
                    System.IO.Directory.CreateDirectory(HostingEnvironment.MapPath("/Logs"));
                if (!System.IO.File.Exists(HostingEnvironment.MapPath("/Logs/logs.txt")))
                    System.IO.File.Create(HostingEnvironment.MapPath("/Logs/logs.txt"));

                sw = new StreamWriter(HostingEnvironment.MapPath("/Logs/logs.txt"), true);
                sw.WriteLine("/************************************ " + DateTime.Now + " ***********************************************");
                sw.WriteLine(ex_message);
                sw.WriteLine(ex); sw.WriteLine(""); sw.WriteLine("");
            }
            catch { }
            finally { sw.Close(); }
        }
    }
}
