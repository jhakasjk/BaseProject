using CoreEntities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace CoreEntities.Classes
{
    public static class ExtensionMethods
    {
        public static string ToEnumDescription(this Enum en) //ext method
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static string ToEnumWordify(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            string pascalCaseString = memInfo[0].Name;
            Regex r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            return r.Replace(pascalCaseString, " ${x}");
        }

        //public static string ToTraceString<T>(this IQueryable<T> t)
        //{
        //    string sql = "";
        //    ObjectQuery<T> oqt = t as ObjectQuery<T>;
        //    if (oqt != null)
        //        sql = oqt.ToTraceString();
        //    return sql;
        //}

        public static IEnumerable<long> ToLongList(this string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }

        public static string GetEnumValues(this string value)
        {
            var type = typeof(MimeTypesEnum);
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                false);
            var description = ((DescriptionAttribute)attributes[0]).Description;
            return description;
        }

        public static string ReplaceSpecialCharacters(this string str)
        {
            string s = Regex.Replace(Regex.Replace(str, "[\\\\/]", "-"), @"[^0-9a-zA-Z\.-]", "-");
            s = s.Replace("---", "-");
            return s.Replace("--", "-");
        }

        public static string ToFieldName(this string str)
        {
            return Regex.Replace(str, @"[^0-9a-zA-Z\.-]", "_");
        }

        public static string Substring(this string str, string StartString, string EndString = null)
        {
            if (str.Contains(StartString))
            {
                int iStart = str.IndexOf(StartString) + StartString.Length;
                int iEnd = !string.IsNullOrEmpty(EndString) ? str.IndexOf(EndString, iStart) : str.Length;
                return str.Substring(iStart, (iEnd - iStart));
            }
            return null;
        }

        public static string Substring(this string str, int StartIndex, string EndString = null)
        {
            if (str.Length > StartIndex)
            {
                int iEnd = !string.IsNullOrEmpty(EndString) ? str.IndexOf(EndString) : str.Length - 1;
                if (iEnd > 0)
                    return str.Substring(StartIndex, (iEnd - StartIndex));
                return str;
            }
            return null;
        }

        public static bool Contains(this List<string> list, string value, bool ignoreCase = false)
        {
            return ignoreCase ? list.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase)) : list.Contains(value);
        }

        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool ParseBool(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if ((new List<string> { "on", "off" }).Contains(value, true))
            {
                return value.Equals("on", StringComparison.OrdinalIgnoreCase) ? true : false;
            }

            bool res = false;
            if (bool.TryParse(value, out res)) return res;
            else return false;
        }

        public static bool IsNumeric(this string value)
        {
            return value.All(c => char.IsDigit(c));
        }

        

        public static string AppendQueryString(this string queryString, string text, string value)
        {
            if (string.IsNullOrEmpty(queryString)) return queryString + text + "=" + value;
            else return queryString + "&" + text + "=" + value;
        }

        public static string GetTimeZoneNameByOffset(int offset)
        {
            var zones = TimeZoneInfo.GetSystemTimeZones();
            var list = zones.Select(m => m.GetUtcOffset(DateTime.Now).TotalSeconds).ToList();
            return zones.Where(o => o.GetUtcOffset(DateTime.Now).TotalSeconds.Equals(offset)).FirstOrDefault().Id;
        }

       

        public static string RemoveAllWhiteSpaces(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            else return str.Replace(" ", "");
        }

        public static string FileExtention(this string filename)
        {
            return filename.Substring(filename.LastIndexOf('.') + 1);
        }

        public static string ToTitleCase(this string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new ArgumentException("ARGH!");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(text);
        }

        public static string PascalToWords(this string str)
        {
            Regex r = new Regex("([A-Z]+[a-z]+)");
            return r.Replace(str, m => (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + " ");
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IDictionary<string, string> ToDictionary(this Type enumType, bool IntValues = true)
        {
            return Enum.GetValues(enumType)
            .Cast<object>()
            .ToDictionary(v => ((Enum)v).ToEnumDescription(), k => (IntValues ? ((int)k).ToString() : k.ToString()));
        }

        public static string ToFormatCell(this string CellPhone)
        {
            if (!string.IsNullOrEmpty(CellPhone))
            {
                CellPhone = CellPhone.Replace(".", "");
                CellPhone = CellPhone.Replace("-", "");
                Int64 FormatNumber;

                if (long.TryParse(CellPhone, out   FormatNumber) == false)
                {
                    string formatted = string.Format("{0:+1-###-###-####}", FormatNumber);
                    CellPhone = formatted;
                }

            }
            else { CellPhone = "No Phone Nubmer"; }
            return CellPhone;
        }

        public static string SiteUrl()
        {
            var context = HttpContext.Current;
            string result = GetStoreHost();
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            if (context != null && context.Request != null)
                result = result + context.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        public static string GetStoreHost()
        {
            var urlReferer = HttpContext.Current.Request.UrlReferrer;
            string httpHost = "";
            if (urlReferer != null)
                httpHost = HttpContext.Current.Request.UrlReferrer.Authority;
            else
                httpHost = HttpContext.Current.Request.Url.Authority;
            var result = "";
            if (!String.IsNullOrEmpty(httpHost))
            {
                result = HttpContext.Current.Request.Url.Scheme + "://" + httpHost;
            }
            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }


        public static string ReadFileText(this string file)
        {
            return File.ReadAllText(file);
        }

        public static HttpContext FakeHttpContext
        {
            get
            {
                HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
                return HttpContext.Current;
            }
        }
        public static void RemoveFile(this string Filename, string pathToSave)
        {
            pathToSave = pathToSave.CreateFolderIfMissing();
            if (!Filename.StartsWith("http"))
                pathToSave = HttpContext.Current.Server.MapPath(pathToSave + "\\" + Filename);
            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
            }
        }

        public static string GiveSpaceCamalCase(this string name)
        {
            return Regex.Replace(name, "(\\B[A-Z])", " $1");
        }
        public static string CreateFolderIfMissing(this string path)
        {
            bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath(path));
            if (!folderExists)
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            return path;
        }


        /// <summary>
        /// Replacing all the urls of a string with an “a href” link
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceUrlToAnchor(this string str)
        {
            Regex urlregex = new Regex(@"(^|[\n ])(?<url>(www|ftp)\.[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            str = urlregex.Replace(str, " <a href=\"http://${url}\" target=\"_blank\">${url}</a>");
            Regex httpurlregex = new Regex(@"(^|[\n ])(?<url>(http://www\.|http://|https://)[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            str = httpurlregex.Replace(str, " <a href=\"${url}\" target=\"_blank\">${url}</a>");
            Regex emailregex = new Regex(@"(?<url>[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+\s)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            str = emailregex.Replace(str, " <a href=\"mailto:${url}\">${url}</a> ");
            return str;
        }
        public static string PhoneNumberFormat(this string phoneNumber)
        {
            string formattedPhoneNumber = null;
            Int64 phone = 0;
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                formattedPhoneNumber = phoneNumber.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+1", "").Replace("+", "");
                Int64.TryParse(formattedPhoneNumber, out phone);
                if (phone > 0)
                    formattedPhoneNumber = String.Format("{0:###-###-####}", phone);
            }

            return formattedPhoneNumber;
        }
        public static string DoubleToFraction(this double num, double epsilon = 0.0001, int maxIterations = 20)
        {
            double[] d = new double[maxIterations + 2];
            d[1] = 1;
            double z = num;
            double n = 1;
            int t = 1;

            int wholeNumberPart = (int)num;
            double decimalNumberPart = num - Convert.ToDouble(wholeNumberPart);

            while (t < maxIterations && Math.Abs(n / d[t] - num) > epsilon)
            {
                t++;
                z = 1 / (z - (int)z);
                d[t] = d[t - 1] * (int)z + d[t - 2];
                n = (int)(decimalNumberPart * d[t] + 0.5);
            }

            return string.Format((wholeNumberPart > 0 ? wholeNumberPart.ToString() + " " : "") + "{0}/{1}",
                                 n.ToString(),
                                 d[t].ToString()
                                );
        }


    }

    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, List<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }
    }
}