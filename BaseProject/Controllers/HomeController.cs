using CoreEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreEntities.Models;
using BaseProject.Attributes;
using CoreEntities.Enums;
using System.Web.Script.Serialization;
using CoreEntities.Classes;
using System.Web.Hosting;
using System.IO;
using System.Drawing;
using ASPSnippets.FaceBookAPI;
using System.Net;
using Newtonsoft.Json;

namespace BaseProject.Controllers
{
    /// <summary>
    /// Home Controller 
    /// </summary>
    public class HomeController : BaseController
    {
        #region Variable Declaration
        private readonly IHomeManager _homeManager;
        private readonly IUserManager _userManager;
        private readonly IPushNotificationManager _pushNotificationManager;
        #endregion

        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="homeManager"></param>
        /// <param name="userManager"></param>
        /// <param name="errorLogManager"></param>
        /// <param name="pushNotificationManager"></param>
        public HomeController(IHomeManager homeManager, IUserManager userManager, IErrorLogManager errorLogManager, IPushNotificationManager pushNotificationManager)
            : base(errorLogManager)
        {
            _homeManager = homeManager;
            _userManager = userManager;
            _pushNotificationManager = pushNotificationManager;
        }

        /// <summary>
        /// Home page if the website
        /// </summary>
        /// <returns></returns>
        [Public]
        public ActionResult Index(string id)
        {
            //List<UserSessions> ExistingVisitorSession = Session[Sessions.VisitorSessions] as List<UserSessions>;
            List<UserPageView> ExistingVisitorSession = System.Web.HttpContext.Current.Application[Sessions.VisitorSessions] as List<UserPageView>;
            ViewBag.ExistingVisitorSession = ExistingVisitorSession;
            return View();
        }

        /// <summary>
        /// Login Page View
        /// </summary>
        /// <returns></returns>
        [Public]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// login to front end
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Public]
        public ActionResult DoLogin(LoginModel model)
        {
            ActionOutput<UserDetails> Result = _homeManager.LoginUser(model, model.Email + "_WebsiteToken", Convert.ToInt32(RegisterVia.Website));
            if (Result.Status == ActionStatus.Successfull)
            {
                CreateCustomAuthorisationCookie(model.Email, false, new JavaScriptSerializer().Serialize(Result.Object));
            }
            return Json(new ActionOutput { Message = Result.Message, Status = Result.Status });
        }

        [Public]
        public ActionResult LoginViaFacebook()
        {
            string sSchemePrefix = string.Empty;
            string sPrimaryDomain = string.Empty;
            string sLocalPath = "Home/Facebook_Access";
            string sUrl = string.Empty;
            FaceBookConnect.API_Key = Config.FacebookAPPID;
            FaceBookConnect.API_Secret = Config.FacebookAppSecret;
            string url = HttpContext.Request.Url.AbsoluteUri;
            if (url.Contains("localhost"))
            {
                sUrl = ExtensionMethods.SiteUrl() + sLocalPath;
            }
            else
            {
                //sSchemePrefix = Request.Url.Scheme + "://" + "www.";
                sPrimaryDomain = "http://www.mydomain.com/";// LIve site base url;
                sUrl = sSchemePrefix + sPrimaryDomain + sLocalPath;
            }
            FaceBookConnect.Authorize("user_photos,email", sUrl);
            return null;
        }

        [Public]
        public ActionResult Facebook_Access()
        {
            if (Request.QueryString["error"] == "access_denied")
            {
                return null;
            }
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                string data = "";
                try
                {
                    data = FaceBookConnect.Fetch(code, "me");
                }
                catch (WebException wex)
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                    dynamic obj = JsonConvert.DeserializeObject(resp);
                    var messageFromServer = obj.error.message;
                }
                FaceBookUserWeb faceBookUser = new JavaScriptSerializer().Deserialize<FaceBookUserWeb>(data);
                faceBookUser.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture", faceBookUser.Id);

                FaceBookUser fbUserModel = new FaceBookUser
                {
                    CountryID = null,
                    DisplayName = faceBookUser.Name,
                    Email = faceBookUser.Email,
                    FacebookUserID = faceBookUser.Id,
                    FullName = faceBookUser.Name,
                    PictureUrl = faceBookUser.PictureUrl,
                    RegisterVia = (int)RegisterVia.WebsiteFacebook,
                    RegistrationIP = Request.UserHostAddress,
                    Subscribe = false
                };

                ActionOutput<UserDetails> Result = _homeManager.FacebookLogin(fbUserModel, "Website-User-" + faceBookUser.Id, (int)RegisterVia.WebsiteFacebook);
                if (Result.Status == ActionStatus.Successfull && Result.Object != null)
                {
                    CreateCustomAuthorisationCookie(fbUserModel.Email, false, new JavaScriptSerializer().Serialize(Result.Object));
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { id = Result.Message.Replace(" ", "-").Replace(".#_=_", "") });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// logout from frontend
        /// </summary>
        /// <returns></returns>
        [Public]
        public ActionResult Logout()
        {
            ActionOutput Result = _userManager.Logout(LOGGEDIN_USER.SessionId);
            if (Result.Status == ActionStatus.Successfull)
            {
                HttpCookie auth_cookie = Request.Cookies[Cookies.AuthorizationCookie];
                if (auth_cookie != null)
                {
                    auth_cookie.Expires = DateTime.UtcNow.AddDays(-30);
                    Response.Cookies.Add(auth_cookie);
                }
                //return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
               
        /// <summary>
        /// User signup
        /// </summary>
        /// <returns></returns>
        [Public]
        public ActionResult Signup()
        {
            //ViewBag.Countries = _homeManager.GetCountries().Results;
            return View(new WebUser());
        }

        /// <summary>
        /// reguster a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Public]
        public ActionResult Register(WebUser model)
        {
            UserDetails user = new UserDetails
            {
                UserID = 0,
                RoleID = (int)UserRoles.User,
                RegisterVia = (int)RegisterVia.Website,
                RegistrationIP = Request.UserHostAddress,
                Email = model.Email,
                Password = Utility.GetEncryptedValue(model.Password),
                ResetPassword = false,
                PasswordResetCode = null,
                FullName = model.FullName,
                DisplayName = model.DisplayName,
                ProfilePicture = string.IsNullOrEmpty(model.ProfilePicture) ? Constants.DefaultUserPic : model.ProfilePicture,
                CountryID = model.CountryID,
                StateID = model.StateID,
                OtherState = model.OtherState,
                City = model.City,
                OtherCity = model.OtherCity,
                ZipCode = model.ZipCode,
                FailedLoginAttempts = 0,
                CreatedOn = DateTime.Now,
            };
            ActionOutput<UserDetails> Result = _homeManager.SaveOrUpdateUser(user);
            //if (model.ProfilePicture != null && model.ProfilePicture != Constants.DefaultUserPic)
            //{
            //    // Move file from temp to actual folder
            //    if (!System.IO.Directory.Exists(HostingEnvironment.MapPath(Config.UserImages + LOGGEDIN_USER.UserID)))
            //        System.IO.Directory.CreateDirectory(HostingEnvironment.MapPath(Config.UserImages + LOGGEDIN_USER.UserID));
            //    System.IO.File.Move(HostingEnvironment.MapPath("~/Temp/" + model.ProfilePicture), HostingEnvironment.MapPath(Config.UserImages + LOGGEDIN_USER.UserID + "/" + model.ProfilePicture));
            //}
            //if (model.Subscribe && Result.Status == ActionStatus.Successfull)
            //    _userManager.Subscribe(model.Email, (int)SubscriptionStatus.Subscribe, Result.Object.UserID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
                
        /// <summary>
        /// Change Password Popup
        /// </summary>
        /// <returns></returns>        
        public ActionResult ChangePassword()
        {
            int[] regvia = { (int)RegisterVia.Android, (int)RegisterVia.Website, (int)RegisterVia.IPhone };
            if (!regvia.Contains(LOGGEDIN_USER.RegisterVia))
                return Json(new ActionOutput
                {
                    Message = "Password can not be changed for facebook user",
                    Status = ActionStatus.Error
                },
                    JsonRequestBehavior.AllowGet);
            ChangePasswordWebsiteModel model = new ChangePasswordWebsiteModel();
            PopupModel popup = new PopupModel();
            popup.Title = "Change Password";
            popup.Body = RenderRazorViewToString("_ChangePassword", model);

            string popupString = RenderRazorViewToString("_LayoutPopup", popup);
            return Json(new ActionOutput { Results = new List<string> { popupString }, Status = ActionStatus.Successfull }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdatePassword(ChangePasswordWebsiteModel model)
        {
            return Json(_userManager.UpdatePassword(model, LOGGEDIN_USER.UserID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Forgot Password Popup
        /// </summary>
        /// <returns></returns>
        [Public]
        public ActionResult ForgotPassword()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            PopupModel popup = new PopupModel();
            popup.Title = "Forgot Password";
            popup.Body = RenderRazorViewToString("_ForgotPassword", model);

            string popupString = RenderRazorViewToString("_LayoutPopup", popup);
            return Json(new ActionOutput { Results = new List<string> { popupString }, Status = ActionStatus.Successfull }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// send password reset code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Public]
        public ActionResult SendForgotPassword(ForgotPasswordModel model)
        {
            return Json(_homeManager.ForgotPassword(model.UserEmail), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Password Reset Popup
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [Public]
        public ActionResult ResetPasswordPopup(string Email)
        {
            ResetPasswordModelWebsite model = new ResetPasswordModelWebsite();
            model.UserEmail = Email;
            PopupModel popup = new PopupModel();
            popup.Title = "Reset Password";
            popup.Body = RenderRazorViewToString("_ResetPasswordPopup", model);

            string popupString = RenderRazorViewToString("_LayoutPopup", popup);
            return Json(new ActionOutput { Results = new List<string> { popupString }, Status = ActionStatus.Successfull }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Public]
        public ActionResult ResetPassword(ResetPasswordModelWebsite model)
        {
            ResetPasswordModel reset = new ResetPasswordModel { NewPassword = model.Password, ResetCode = model.ResetCode, UserEmail = model.UserEmail };
            return Json(_homeManager.ResetPassword(reset), JsonRequestBehavior.AllowGet);
        }
    }
}
