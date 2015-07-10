using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Classes
{
    public static class Cookies
    {
        public const string AuthorizationCookie = "_DGUFDU2463";
        public const string AdminAuthorizationCookie = "HJKHDUEHG387";
        public const string UserOffSetCookie = "e_qweqwe";
        public const string TimezoneOffset = "erw3r2353534";
    }

    public static class Sessions
    {
        public const string VisitorSessions = "3bj4b86834b";
    }

    public static class Constants
    {
        public const string DefaultUserPic = "default-user.png";
    }

    public static class ImageMimes
    {
        public static List<String> ForImages = new List<string>
        {
            "image/jpeg",
            "image/gif",
            "image/png",
            "image/bmp"
        };
    }
}
