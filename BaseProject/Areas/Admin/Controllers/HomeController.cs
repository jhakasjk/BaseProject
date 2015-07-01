using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AJAD.Core.Models.Admin;
using AJAD.Core.Interfaces.Admin;
using AJAD.Core.Interfaces;
using AJAD.Core.Models;
using System.Web.Helpers;
using AJAD.Core.Enums;
using AJAD.Web.Attributes;
using System.Web.Script.Serialization;
using AJAD.Core.Classes;

namespace AJAD.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Variable Declaration
        private readonly IAdminHomeManager _adminHomeManager;
        private readonly IHomeManager _homeManager;
        private readonly IUserManager _userManager;
        private readonly IErrorLogManager _errorLogManager;
        #endregion

        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="adminHomeManager"></param>
        /// <param name="errorLogManager"></param>
        public HomeController(IAdminHomeManager adminHomeManager, IHomeManager homeManager, IUserManager userManager, IErrorLogManager errorLogManager)
            : base(errorLogManager)
        {
            _adminHomeManager = adminHomeManager;
            _homeManager = homeManager;
            _userManager = userManager;
        }


        /// <summary>
        /// Render Admin Login
        /// </summary>
        [Public]
        public ActionResult LogIn()
        {
            AdminLoginModal model = new AdminLoginModal();
            return View(model);
        }

        /// <summary>
        /// Check admin login credentials
        /// </summary>
        /// <param name="model"></param>
        [Public, HttpPost]
        public JsonResult LogIn(AdminLoginModal model)
        {
            ActionOutput<UserDetails> Result = _adminHomeManager.LoginAdmin(model, model.Email + "_WebsiteToken", Convert.ToInt32(RegisterVia.AdminWebsite));
            if (Result.Status == ActionStatus.Successfull)
            {
                CreateCustomAuthorisationCookie(model.Email, false, new JavaScriptSerializer().Serialize(Result.Object));
            }
            return Json(new ActionOutput { Message = Result.Message, Status = Result.Status, Results = new List<string> { Url.Action("DashBoard", "Home", new { Areas = "Admin" }) } });
        }

        /// <summary>
        ///  admin logout
        /// </summary>
        /// <param name="model"></param>
        [Public]
        public ActionResult Logout()
        {
            ActionOutput Result = _adminHomeManager.Logout(LOGGEDIN_USER.SessionId);
            if (Result.Status == ActionStatus.Successfull)
            {
                HttpCookie auth_cookie = Request.Cookies[Cookies.AdminAuthorizationCookie];
                if (auth_cookie != null)
                {
                    auth_cookie.Expires = DateTime.UtcNow.AddDays(-30);
                    Response.Cookies.Add(auth_cookie);
                }
                //return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("LogIn", "Home", new { Areas = "Admin" });
        }

        /// <summary>
        /// Render admin dashbord
        /// </summary>
        public ActionResult DashBoard()
        {
            return View();
        }

        /// <summary>
        /// Winners
        /// </summary>
        /// <returns></returns>
        public ActionResult Winners()
        {
            ViewBag.RewardTypes = _homeManager.RewardTypes().Results;
            return View(_adminHomeManager.GetWinners(1, 20));
        }
    }
}