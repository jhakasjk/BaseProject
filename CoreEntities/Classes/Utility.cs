using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace CoreEntities.Classes
{
    public class Utility
    {
        /// <summary>
        /// generates the hash code from the string passed
        /// </summary>
        public static string HashCode(string str)
        {
            SHA1 hash = SHA1CryptoServiceProvider.Create();
            byte[] plainTextBytes = Encoding.ASCII.GetBytes(str);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            string localChecksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            return localChecksum;
        }


        public static string CreateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        public static string ReadFileText(string file)
        {
            return File.ReadAllText(HostingEnvironment.MapPath(file));
        }

        public static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }

        public static byte[] GetEncryptedValue(string value)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(value.Trim()));

            return hashedBytes;
        }

        public static Image Base64StringToImage(string picture_string)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(picture_string);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
                return image;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the first day of the month for
        /// any full date submitted
        /// </summary>
        /// <param name="dtDate"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime dtDate)
        {
            // set return value to the first day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime dtFrom = dtDate;

            // remove all of the days in the month
            // except the first day and set the
            // variable to hold that date
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            // return the first day of the month
            return dtFrom;
        }

        /// <summary>
        /// Get the first day of the month for a
        /// month passed by it's integer value
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(int iMonth)
        {
            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime dtFrom = new DateTime(DateTime.Now.Year, iMonth, 1);

            // remove all of the days in the month
            // except the first day and set the
            // variable to hold that date
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            // return the first day of the month
            return dtFrom;
        }

        /// <summary>
        /// Get the last day of the month for any
        /// full date
        /// </summary>
        /// <param name="dtDate"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime dtDate)
        {
            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime dtTo = dtDate;

            // overshoot the date by a month
            dtTo = dtTo.AddMonths(1);

            // remove all of the days in the next month
            // to get bumped down to the last day of the
            // previous month
            dtTo = dtTo.AddDays(-(dtTo.Day));

            // return the last day of the month
            return dtTo;
        }

        /// <summary>
        /// Get the last day of a month expressed by it's
        /// integer value
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(int iMonth)
        {

            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime dtTo = new DateTime(DateTime.Now.Year, iMonth, 1);

            // overshoot the date by a month
            dtTo = dtTo.AddMonths(1);

            // remove all of the days in the next month
            // to get bumped down to the last day of the
            // previous month
            dtTo = dtTo.AddDays(-(dtTo.Day));

            // return the last day of the month
            return dtTo;

        }

        public static Bitmap ResizeImage(Stream imageStream, int maxWidth, int maxHeight)
        {
            Image image = Image.FromStream(imageStream);
            //a holder for the result
            //Bitmap result = new Bitmap(width, height);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, newImage.Width, newImage.Height);
            }

            //return the resulting bitmap
            return newImage;
        }

        /// <summary>
        /// Get Start & End date of the given date
        /// </summary>
        /// <param name="now"></param>
        /// <param name="begining"></param>
        /// <param name="end"></param>
        public static void GetWeek(DateTime now, out DateTime begining, out DateTime end)
        {
            if (now == null)
                throw new ArgumentNullException("now");
            
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            int offset = firstDayOfWeek - now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                begining = weekStart;
                end = endOfWeek;
            }
            else
            {
                begining = now.AddDays(-6);
                end = now;
            }
        }
    }
}
