using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BaseProject.WebAPI.Attributes
{
    public class AuthenticateUser : Attribute { }

    public class Public : Attribute { }

    public class MemberAccess : Attribute { }

    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }

    public class ModuleAccessAttribute : ActionMethodSelectorAttribute
    {
        private int id { get; set; }

        public ModuleAccessAttribute(int id)
        {
            this.id = id;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }

    /// <summary>
    /// This will be used to set No Cache For Controller Actions
    /// </summary>
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            base.OnResultExecuting(filterContext);
        }
    }

    /// <summary>
    /// This will be used to skip model validations
    /// </summary>
    public class IgnoreModelErrorsAttribute : ActionFilterAttribute
    {
        private string keysString;

        public IgnoreModelErrorsAttribute(string keys)
            : base()
        {
            this.keysString = keys;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
            string[] keyPatterns = keysString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keyPatterns.Length; i++)
            {
                string keyPattern = keyPatterns[i]
                    .Trim()
                    .Replace(@".", @"\.")
                    .Replace(@"[", @"\[")
                    .Replace(@"]", @"\]")
                    .Replace(@"\[\]", @"\[[0-9]+\]")
                    .Replace(@"*", @"[A-Za-z0-9]+");
                IEnumerable<string> matchingKeys = modelState.Keys.Where(x => Regex.IsMatch(x, keyPattern));
                foreach (string matchingKey in matchingKeys)
                    modelState[matchingKey].Errors.Clear();
            }
        }
    }
}