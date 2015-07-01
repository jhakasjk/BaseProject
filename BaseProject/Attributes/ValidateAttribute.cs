using CoreEntities.Enums;
using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Http;
using System.Net.Http;

namespace BaseProject.Attributes
{
    /// <summary>
    /// validates the incomming model
    /// </summary>
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                }
                string message = errors.FirstOrDefault().Value.FirstOrDefault();
                actionContext.Response = actionContext.Request.CreateResponse<ActionOutput<Dictionary<string, IEnumerable<string>>>>(HttpStatusCode.OK, new ActionOutput<Dictionary<string, IEnumerable<string>>>
                {
                    Status = ActionStatus.Error,
                    Message = "Validation Error Found in Request Paramaters : " + message,
                    Object = errors
                });
            }
        }
    }
}
