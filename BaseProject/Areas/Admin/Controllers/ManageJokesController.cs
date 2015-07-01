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

namespace AJAD.Areas.Admin.Controllers
{
    public class ManageJokesController : AdminBaseController
    {


        #region Variable Declaration
        private readonly IManageJokesManager _JokeManager;
        private readonly IErrorLogManager _errorLogManager;
        #endregion

        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="homeManager"></param>
        /// <param name="errorLogManager"></param>
        public ManageJokesController(IManageJokesManager JokeManager, IErrorLogManager errorLogManager)
            : base(errorLogManager)
        {
            _JokeManager = JokeManager;
        }


        /// <summary>
        /// Render Manage Jokes view
        /// </summary>
        public ActionResult Index()
        {
            var model = _JokeManager.GetManageJokesPageingResult(new AdminPagingModel { PageNo = 1, RecordsPerPage = 10 });
            return View(model);
        }

        /// <summary>
        /// Manage jokes listing for manage joke grid
        /// </summary>
        /// <param name="model"></param>
        [AjaxOnly, HttpPost]
        public JsonResult GetManageJokePagingList(AdminPagingModel model)
        {
            var JokesListing = _JokeManager.GetManageJokesPageingResult(model);
            List<string> resultString = new List<string>();
            resultString.Add(RenderRazorViewToString("Partials/_JokesListing", JokesListing));
            resultString.Add(JokesListing.TotalCount.ToString());
            return Json(new ActionOutput { Status = ActionStatus.Successfull, Message = "", Results = resultString }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render add and edit view
        /// </summary>
        /// <param name="JokeId"></param>
        [Public]
        public ActionResult AddEditJoke(int JokeId)
        {
            if (JokeId > 0)
            {
                ViewBag.JokeCategory = _JokeManager.JokeCategoryListing().Select(x => new SelectListItem { Value = x.CategoryID.ToString(), Text = x.Name }).ToList();
                var Model = _JokeManager.GetJokeDetails(JokeId);
                return View(Model);
            }
            else
            {
                ViewBag.JokeCategory = _JokeManager.JokeCategoryListing().Select(x => new SelectListItem { Value = x.CategoryID.ToString(), Text = x.Name }).ToList();
                JokeAddEditModel Model = new JokeAddEditModel();
                return View(Model);
            }
        }

        /// <summary>
        /// add and update for Joke
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public JsonResult AddUpdateJoke(JokeAddEditModel model)
        {
            var result = _JokeManager.AddUpdateJoke(model);
            if (result.Object != null)
            {
                return Json(new ActionOutput
                {
                    Status = result.Status,
                    Message = result.Message,
                    Results = new List<string> { Url.Action("Index", "ManageJokes", new { Area = "Admin" }) }
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
        /// Delete joke form manage jokes listing
        /// </summary>
        /// <param name="UserId"></param>
        [HttpPost, AjaxOnly]
        public JsonResult DeleteJoke(int JokeId)
        {
            var Output = _JokeManager.DeleteJoke(JokeId);
            return Json(new ActionOutput { Status = Output.Status, Message = Output.Message }, JsonRequestBehavior.AllowGet);
        }



    }
}