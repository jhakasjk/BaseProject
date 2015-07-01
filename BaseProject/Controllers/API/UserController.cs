using BaseProject.Attributes;
using CoreEntities.Classes;
using CoreEntities.Enums;
using CoreEntities.Interfaces;
using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BaseProject.Controllers.API
{
    /// Restfull Services for Authorized Users
    /// </summary>
    [RoutePrefix("api/user")]
    [EnableCors(origins: "http://www.mydomain.com", headers: "*", methods: "*")]
    public class UserController : WepApiAuthorizeController
    {
        #region Properties & Constructor
        private readonly IUserManager _userManager;
        private readonly IHomeManager _homeManager;
        private readonly IPushNotificationManager _pushManager;

        public UserController(IUserManager userManager, IHomeManager homeManager, IPushNotificationManager pushManager)
        {
            _userManager = userManager;
            _homeManager = homeManager;
            _pushManager = pushManager;
        }
        #endregion

        /// <summary>
        /// Update User Information in Database.        
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("UpdateUser")]
        public HttpResponseMessage UpdateUser(APIUpdateUser user)
        {
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
            UserDetails userDetails = new UserDetails
            {
                UserID = User.UserID,
                RoleID = User.RoleID,
                Email = User.Email,
                //Password =  Utility.GetEncryptedValue(user.Password),
                DisplayName = user.DisplayName,
                FullName = user.FullName,
                ProfilePicture = filename,
                CountryID = user.CountryID,
                StateID = user.StateID,
                OtherState = user.OtherState,
                City = user.City,
                OtherCity = user.OtherCity,
                ZipCode = user.ZipCode
            };
            ActionOutput<UserDetails> Result = _homeManager.SaveOrUpdateUser(userDetails);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message,
                JsonData = Result.Object
            });
        }

        /// <summary>
        /// Change logged in User's Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        public HttpResponseMessage ChangePassword(ChangePasswordModel model)
        {
            model.UserID = User.UserID;
            ActionOutput output = _userManager.ChangePassword(model);
            return Request.CreateResponse<ApiActionOutput>(new ApiActionOutput
            {
                Status = output.Status,
                Message = output.Message
            });
        }

        /// <summary>
        /// Logout the logged in User
        /// Post Url: http://www.mydomain.com/api/user/logout
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        public HttpResponseMessage Logout()
        {
            var session = new Guid(Request.Headers.GetValues("SessionToken").FirstOrDefault());
            ActionOutput Result = _userManager.Logout(session);
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = Result.Status,
                Message = Result.Message
            });
        }

        /// <summary>
        /// Upload Profile picture of logged in user. PictureString must be in bse64 string format.
        /// </summary>
        /// <param name="PictureString"></param>
        /// <returns></returns>
        [Route("UploadProfilePic")]
        [HttpPost]
        public HttpResponseMessage UploadProfilePic(PictureModel model)
        {
            string root = HostingEnvironment.MapPath(Config.UserImages + User.UserID);
            if (!System.IO.Directory.Exists(root))
                System.IO.Directory.CreateDirectory(root);

            string filename = string.Empty;
            if (!string.IsNullOrEmpty(model.Base64ImageString))
            {
                try
                {
                    filename = Guid.NewGuid().ToString() + ".jpg";
                    var bytes = Convert.FromBase64String(model.Base64ImageString);
                    using (var imageFile = new FileStream(root + "\\" + filename, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    var output = _userManager.UpdateProfilePic(User.UserID, filename);
                    return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
                    {
                        Status = output.Status,
                        Message = output.Message,
                        JsonData = output.Results[0]
                    });
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
            return Request.CreateResponse<ApiActionOutput>(HttpStatusCode.OK, new ApiActionOutput
            {
                Status = ActionStatus.Error,
                Message = "Invalid base64 string.",
                JsonData = null
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
            ActionOutput<UserDetails> output = _userManager.GetUserInformation(Email, User.UserID);
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
            ActionOutput<UserDetails> output = _userManager.GetUserInformation(UserID, User.UserID);
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
