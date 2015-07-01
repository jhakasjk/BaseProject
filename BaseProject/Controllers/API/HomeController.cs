using CoreEntities.Interfaces;
using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BusinessEntities.Managers;
using CoreEntities.Enums;
using CoreEntities.Classes;
using System.Drawing;
using System.Web.Hosting;
using System.IO;

namespace BaseProject.Controllers.API
{
    /// <summary>
    /// <para>Restfull Services for Unauthorized Users. </para>
    /// <para> Calling Uri: {BaseURL}/api/home/{method-name}</para>
    /// </summary>
    [RoutePrefix("api/home")]
    [EnableCors(origins: "http://www.mydomain.com", headers: "*", methods: "*")]
    public class HomeController : WebApiController
    {
        #region Properties & Constructor
        private readonly IHomeManager _homeManager;
        private readonly IUserManager _userManager;
        private readonly IPushNotificationManager _pushManager; 

        public HomeController() { }

        public HomeController(IHomeManager homeManager, IUserManager userManager, IPushNotificationManager pushManager)
        {
            _homeManager = homeManager;
            _userManager = userManager;
            _pushManager = pushManager;
        }
        #endregion

        /// <summary>
        /// Save User Information in Database.        
        /// </summary>
        /// <param name="User">User Json Object, which contains User Information.</param>
        /// <returns>Returns HttpResponseMessage object, which contains Status, Message and User Json Object.</returns>
        [Route("RegisterUser")]
        public HttpResponseMessage RegisterUser(APIUser User)
        {
            User.UserID = 0;
            string filename = string.Empty;
            if (!string.IsNullOrEmpty(User.ProfilePicture))
            {
                try
                {
                    string root = HostingEnvironment.MapPath(Config.UserImages + User.UserID);
                    if (!System.IO.Directory.Exists(root))
                        System.IO.Directory.CreateDirectory(root);
                    filename = Guid.NewGuid().ToString() + ".jpg";
                    var bytes = Convert.FromBase64String(User.ProfilePicture);
                    using (var imageFile = new FileStream(root + "\\" + filename, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = ActionStatus.Error,
                        Message = "Invalid Profile Pic: " + ex.Message,
                        JsonData = null
                    });
                }
            }
            UserDetails user = new UserDetails
            {
                UserID = 0,
                RoleID = (int)UserRoles.User,
                RegisterVia = User.RegisterVia,
                RegistrationIP = User.RegistrationIP,
                Email = User.Email,
                Password = Utility.GetEncryptedValue(User.Password),
                ResetPassword = false,
                PasswordResetCode = null,
                FullName = User.FullName,
                DisplayName = User.DisplayName,
                ProfilePicture = filename,
                CountryID = User.CountryID,
                StateID = User.StateID,
                OtherState = User.OtherState,
                City = User.City,
                OtherCity = User.OtherCity,
                ZipCode = User.ZipCode,
                FailedLoginAttempts = 0,
                CreatedOn = DateTime.Now,
            };
            ActionOutput<UserDetails> Result = _homeManager.SaveOrUpdateUser(user);
            //if (User.Subscribe && Result.Status == ActionStatus.Successfull)
                //_userManager.Subscribe(User.Email, (int)SubscriptionStatus.Subscribe, Result.Object.UserID);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message,
                JsonData = Result.Object
            });
        }

        /// <summary>
        /// Login User by passing Email & Password
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        [Route("Login")]
        public HttpResponseMessage Login(LoginModel Login)
        {
            string deviceToken = Request.Headers.GetValues("DeviceToken").FirstOrDefault();
            string deviceType = Request.Headers.GetValues("DeviceType").FirstOrDefault();

            ActionOutput<UserDetails> Result = _homeManager.LoginUser(Login, deviceToken, Convert.ToInt32(deviceType));
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message,
                JsonData = Result.Object
            });
        }

        /// <summary>
        /// Check for Registered User in FB login
        /// </summary>
        /// <param name="FacebookUserID"></param>
        /// <returns></returns>
        [Route("CheckFBUser")]
        public HttpResponseMessage CheckFBUser(string FacebookUserID)
        {
            string deviceToken = Request.Headers.GetValues("DeviceToken").FirstOrDefault();
            string deviceType = Request.Headers.GetValues("DeviceType").FirstOrDefault();

            ActionOutput Result = _homeManager.CheckFBUser(FacebookUserID);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message
            });
        }

        /// <summary>
        /// Login via facebook. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("FacebookLogin")]
        public HttpResponseMessage FacebookLogin(FaceBookUser model)
        {
            string deviceToken = Request.Headers.GetValues("DeviceToken").FirstOrDefault();
            string deviceType = Request.Headers.GetValues("DeviceType").FirstOrDefault();

            ActionOutput<UserDetails> Result = _homeManager.FacebookLogin(model, deviceToken, Convert.ToInt32(deviceType));
            // Subscribe
            if (model.Subscribe && Result.Status == ActionStatus.Successfull)
            {
                //_userManager.Subscribe(model.Email, (int)SubscriptionStatus.Subscribe, Result.Object.UserID);
            }
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message,
                JsonData = Result.Object
            });
        }

        /// <summary>
        /// Send Password reset code on user's email.        
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ForgotPassword")]
        public HttpResponseMessage ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                {
                    Status = ActionStatus.Error,
                    Message = "Invalid Email."
                });
            ActionOutput<UserDetails> Result = _homeManager.ForgotPassword(Email);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message,
                JsonData = Result.Object
            });
        }

        /// <summary>
        /// verify the reset code & reset the password if code verified        
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public HttpResponseMessage ResetPassword(ResetPasswordModel model)
        {
            ActionOutput Result = _homeManager.ResetPassword(model);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message
            });
        }

        /// <summary>
        /// Get user by Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [Route("GetUserInformation")]
        public HttpResponseMessage GetUserInformation(string Email)
        {
            ActionOutput<UserDetails> output = _userManager.GetUserInformation(Email);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = output.Status,
                Message = output.Message,
                JsonData = output.Object
            });
        }

        /// <summary>
        /// Get user by UserID
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [Route("GetUserInformation")]
        public HttpResponseMessage GetUserInformation(int UserID)
        {
            ActionOutput<UserDetails> output = _userManager.GetUserInformation(UserID);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = output.Status,
                Message = output.Message,
                JsonData = output.Object
            });
        }

        /// <summary>
        /// Test Push Notifications
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="DeviceToken"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        [Route("TestPush")]
        public HttpResponseMessage TestPush(string Message, string DeviceToken, int DeviceType)
        {
            //_pushManager.SendTest("hello", "2e0d345bdf93e03e49c5385052f79555eb81c84a1817d79430ef0be75ef0c9ee");
            _pushManager.SendTest(Message, DeviceToken, DeviceType);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = ActionStatus.Successfull,
                Message = "Done"
            });
        }
    }
}
