using CoreEntities.Interfaces;
using DataAccessLayer.Model.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BusinessEntities.Managers
{
    public class ErrorLogManager : BaseManager, IErrorLogManager
    {
        string IErrorLogManager.LogExceptionToDatabase(Exception ex, string FormData = "", string QueryData = "", string RouteData = "")
        {
            ErrorLog obj_errorlog = null;
            String ex_text = String.Empty;
            String ex_message = ex.Message;
            try
            {
                obj_errorlog = new ErrorLog();
                obj_errorlog.Message = ex.Message;
                obj_errorlog.StackTrace = ex.StackTrace;
                obj_errorlog.InnerException = ex.InnerException == null ? "" : ex.InnerException.Message;
                obj_errorlog.LoggedInDetails = "";
                obj_errorlog.LoggedAt = DateTime.Now;

                obj_errorlog.FormData = FormData;
                obj_errorlog.QueryData = QueryData;
                obj_errorlog.RouteData = RouteData;
                //=======================================================================

                ex_text = JsonConvert.SerializeObject(obj_errorlog);
                Context.ErrorLogs.Add(obj_errorlog);
                SaveChanges();
                return obj_errorlog.ErrorLogID.ToString();

            }
            catch (Exception)
            {
                System.IO.StreamWriter sw = null;
                if (!File.Exists(HostingEnvironment.MapPath("~/ErrorLog.txt")))
                    File.Create(HostingEnvironment.MapPath("~/ErrorLog.txt"));
                sw = new StreamWriter(HostingEnvironment.MapPath("~/ErrorLog.txt"), true);
                sw.WriteLine(ex_message);
                sw.WriteLine("/");
                sw.WriteLine(ex); sw.WriteLine(""); sw.WriteLine("");
                sw.Close();
                return "0";
            }
        }

        void IErrorLogManager.LogExceptionToFile(string ex, string ex_message)
        {
            System.IO.StreamWriter sw = null;
            if (!File.Exists(HostingEnvironment.MapPath("~/ErrorLog.txt")))
                File.Create(HostingEnvironment.MapPath("~/ErrorLog.txt"));
            sw = new StreamWriter(HostingEnvironment.MapPath("~/ErrorLog.txt"), true);
            sw.WriteLine(ex_message);
            sw.WriteLine("/");
            sw.WriteLine(ex); sw.WriteLine(""); sw.WriteLine("");
            sw.Close();
        }        
    }
}
