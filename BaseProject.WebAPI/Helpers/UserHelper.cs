using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BaseProject.WebAPI.Helpers
{
    public static class UserHelper
    {
        public static string GetCulture()
        {
            string URL = HttpContext.Current.Request.Url.AbsolutePath.Split(new char[] { '/' })[1];
            if (URL.Length == 2 || (URL.Length == 5 && URL[2] == '-'))
                return URL + "/";
            else
                return string.Empty;
            //if(culture.ToLower()=="en-us"
            //var url = HttpContext.Current.Request.Url;
            //if (url.Segments.Length >= 4 || url.Segments.Length == 2)
            //    return !GetLanguageCodes().Any(x => x.Contains(url.Segments[1])) ? string.Empty : url.Segments[1] + "/";
            //return string.Empty;
        }

        public static IList<string> GetLanguageCodes()
        {
            return new List<string>
            {
                "da","nl","en","fr","de","it","ja","pl","pt","ru","es","se","tr","en-US"
            };
        }


        /// <summary>
        /// Get the formatted url relative to the root of the current site.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrl(string url)
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext).Content(url);
        }

    }
}
