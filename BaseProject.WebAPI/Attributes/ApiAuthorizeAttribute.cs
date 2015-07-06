using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CoreEntities.Models;
using CoreEntities.Enums;
using CoreEntities.Classes;
using BaseProject.WebAPI.Controllers.API;
using CoreEntities.Interfaces;
using System.Web.Mvc;

namespace BaseProject.WebAPI.Attributes
{
    public class AllowAdmin : Attribute { }

    public class ApiAuthorizeUserAttribute : System.Web.Http.AuthorizeAttribute
    {
        private readonly IUserManager _userManager;

        public ApiAuthorizeUserAttribute(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApiAuthorizeUserAttribute()
        {
            // TODO: Complete member initialization
            _userManager = (IUserManager)DependencyResolver.Current.GetService(typeof(IUserManager));
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }
        }

        private bool Authorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var licenseKey = ConfigurationManager.AppSettings["LicenseKey"];
                var clientHash = actionContext.Request.Headers.GetValues("ClientHash").FirstOrDefault();
                var timeStamp = actionContext.Request.Headers.GetValues("TimeStamp").FirstOrDefault();
                var sessionToken = actionContext.Request.Headers.GetValues("SessionToken").FirstOrDefault();
                var deviceToken = actionContext.Request.Headers.GetValues("DeviceToken").FirstOrDefault();
                var deviceType = actionContext.Request.Headers.GetValues("DeviceType").FirstOrDefault();
                var baseController = (WepApiAuthorizeController)actionContext.ControllerContext.Controller;

                var validationHash = Utility.HashCode(string.Format("{0}{1}{2}{3}{4}", sessionToken, timeStamp, licenseKey, deviceToken, deviceType));
                if (validationHash != clientHash)
                {
                    actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = ActionStatus.Error,
                        Message = "Invalid LicenseKey/ClientHash."
                    });
                    return false;
                }

                var sessionID = new Guid(sessionToken);
                UserDetails loginSession = _userManager.CheckUserLogin(sessionID);

                if (loginSession == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = ActionStatus.Error,
                        Message = "Please login first to make this request."
                    });
                    return false;
                }
                else if (loginSession.DeviceToken != deviceToken || loginSession.DeviceType != Convert.ToInt32(deviceType))
                {
                    actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = ActionStatus.Error,
                        Message = "You have been logged out from this device. Your session is active on some other device."
                    });
                    return false;
                }
                else
                {
                    baseController.User = new UserDetails
                    {
                        UserID = loginSession.UserID,
                        RoleID = loginSession.RoleID,
                        SessionId = loginSession.SessionId,
                        DeviceToken = loginSession.DeviceToken,
                        DeviceType = loginSession.DeviceType
                    };
                }
                if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAdmin>(true).Any() && (loginSession.RoleID != (int)UserRoles.Admin))
                {
                    actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = ActionStatus.Error,
                        Message = "You're not authorized to perform this action!!!"
                    });
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                {
                    Status = ActionStatus.Error,
                    Message = ex.ToString()
                });
                return false;
            }
        }
    }

    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }
            //HandleUnauthorizedRequest(actionContext);
            actionContext.Response = actionContext.Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = ActionStatus.Error,
                Message = "Invalid LicenseKey/ClientHash."
            });
        }

        private bool Authorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var licenseKey = ConfigurationManager.AppSettings["LicenseKey"];
                var clientHash = actionContext.Request.Headers.GetValues("ClientHash").FirstOrDefault();
                var timeStamp = actionContext.Request.Headers.GetValues("TimeStamp").FirstOrDefault();
                var deviceToken = actionContext.Request.Headers.GetValues("DeviceToken").FirstOrDefault();
                var deviceType = actionContext.Request.Headers.GetValues("DeviceType").FirstOrDefault();

                var validationHash = Utility.HashCode(string.Format("{0}{1}{2}{3}", timeStamp, licenseKey, deviceToken, deviceType));
                if (validationHash != clientHash)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
