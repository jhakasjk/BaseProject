using AJAD.Core.Enums;
using AJAD.Core.Interfaces;
using AJAD.Core.Interfaces.Admin;
using AJAD.Core.Models;
using AJAD.Core.Models.Admin;
using AJAD.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AJAD.Areas.Admin.Controllers
{
    public class ManageUsersController : AdminBaseController
    {
        #region Variable Declaration
        private readonly IAdminHomeManager _homeManager;
        private readonly IErrorLogManager _errorLogManager;
        #endregion

        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="homeManager"></param>
        /// <param name="errorLogManager"></param>
        public ManageUsersController(IAdminHomeManager homeManager, IErrorLogManager errorLogManager)
            : base(errorLogManager)
        {
            _homeManager = homeManager;
        }


        /// <summary>
        /// Render Manage Users view
        /// </summary>
        public ActionResult Index()
        {
            var model = _homeManager.GetManageUsersPageingResult(new AdminPagingModel { PageNo = 1, RecordsPerPage = 10 });
            return View(model);
        }

        /// <summary>
        /// Manage users listing for manage user grid
        /// </summary>
        /// <param name="model"></param>
        [AjaxOnly, HttpPost]
        public JsonResult GetManageUsersPagingList(AdminPagingModel model)
        {
            var UsersListing = _homeManager.GetManageUsersPageingResult(model);
            List<string> resultString = new List<string>();
            resultString.Add(RenderRazorViewToString("Partials/_UsersListing", UsersListing));
            resultString.Add(UsersListing.TotalCount.ToString());
            return Json(new ActionOutput { Status = ActionStatus.Successfull, Message = "", Results = resultString }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Manage users listing for manage user grid
        /// </summary>
        /// <param name="model"></param>
        [HttpPost, AjaxOnly]
        public JsonResult DeleteUser(int UserId)
        {
            var Output = _homeManager.DeleteUser(UserId);
            return Json(new ActionOutput { Status = Output.Status, Message = Output.Message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Render add and edit view
        /// </summary>
        /// <param name="model"></param>
        [Public]
        public ActionResult AddEditUser(int UserId)
        {
            if (UserId > 0)
            {
                var Model = _homeManager.GetUserDetails(UserId);
                var list = new List<SelectListItem>();
                ViewBag.Country = _homeManager.CountryListing().Select(x => new SelectListItem { Text = x.CountryName, Value = x.CountryID.ToString() }).ToList();
                ViewBag.State = Model.Country == null ? new List<SelectListItem> { new SelectListItem { Text = "Selecte State", Value = null } } : _homeManager.StateListing(Int32.Parse(Model.Country.ToString())).Select(x => new SelectListItem { Text = x.StateName, Value = x.StateID.ToString() }).ToList();
                ViewBag.City = Model.State == null ? new List<SelectListItem> { new SelectListItem { Text = "Selecte City", Value = null } } : _homeManager.CityListing(Int32.Parse(Model.State.ToString())).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityID.ToString() }).ToList();
                return View(Model);
            }
            else
            {
                UserAddEditModel Model = new UserAddEditModel();
                ViewBag.Country = _homeManager.CountryListing().Select(x => new SelectListItem { Text = x.CountryName, Value = x.CountryID.ToString() }).ToList();
                ViewBag.State = new List<SelectListItem> { new SelectListItem { Text = "Selecte State", Value = null } };
                ViewBag.City = new List<SelectListItem> { new SelectListItem { Text = "Selecte City", Value = null } };
                return View(Model);
            }


        }

        /// <summary>
        /// add and update for user
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public JsonResult AddUpdateUser(UserAddEditModel model)
        {
            var result = _homeManager.AddUpdateUser(model);
            if (result.Object != null)
            {
                return Json(new ActionOutput
                {
                    Status = result.Status,
                    Message = result.Message,
                    Results = new List<string> { Url.Action("Index", "ManageUsers", new { Area = "Admin" }) }
                });
            }
            else
            {
                return Json(new ActionOutput
                {
                    Status = result.Status,
                    Message = result.Message

                });
            }
        }


        /// <summary>
        /// To get state lists by country id
        /// </summary>
        [Public, HttpPost]
        public JsonResult GetStateList(int country)
        {
            var list = _homeManager.StateListing(country).Select(x => new SelectListItem { Text = x.StateName, Value = x.StateID.ToString() }).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var userStates = new SelectList(list, "Value", "Text");
            return Json(new ActionOutput { Status = ActionStatus.Successfull, Results = new List<string> { js.Serialize(userStates) } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// To get Citys lists by state id
        /// </summary>
        [Public, HttpPost]
        public JsonResult GetCitysList(int State)
        {
            var list = _homeManager.CityListing(State).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityID.ToString() }).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var userStates = new SelectList(list, "Value", "Text");
            return Json(new ActionOutput { Status = ActionStatus.Successfull, Results = new List<string> { js.Serialize(userStates) } }, JsonRequestBehavior.AllowGet);
        }



    }
}