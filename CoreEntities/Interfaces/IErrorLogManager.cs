using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Interfaces
{
    public interface IErrorLogManager
    {
        /// <summary>
        /// log the exception into database
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        string LogExceptionToDatabase(Exception ex, string FormData = "", string QueryData = "", string RouteData = "");

        /// <summary>
        /// log the exception into file, if LogExceptionToDatabase fails
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ex_message"></param>
        void LogExceptionToFile(String ex, String ex_message);
    }
}
