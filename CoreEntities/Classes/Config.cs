using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Classes
{
    public static class Config
    {
        public static string AdminEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();
            }
        }

        public static string FacebookAPPID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["FacebookAPPID"].ToString();
            }
        }

        public static string FacebookAppSecret
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"].ToString();
            }
        }

        public static string UserImages
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserImages"].ToString();
            }
        }
    }
}
