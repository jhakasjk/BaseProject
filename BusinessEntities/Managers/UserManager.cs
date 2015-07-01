using CoreEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Model.DataModel;
using BusinessEntities.Managers;
using CoreEntities.Models;
using CoreEntities.Classes;
using CoreEntities.Enums;
using System.Threading;
using System.Data.Entity;
using LinqKit;

namespace BusinessEntities.Managers
{
    public class UserManager : BaseManager, IUserManager
    {
        private int Unsubscribe;
        private IPushNotificationManager _pushNotificationManager;

        public UserManager()
        {
            Unsubscribe = (int)SubscriptionStatus.Unsubscribe;
            _pushNotificationManager = new PushNotificationManager();
        }

        string IUserManager.GetWelcomeMessage()
        {
            return "Welcome to Demo Function";
        }

        UserDetails IUserManager.CheckUserLogin(Guid userSessionId)
        {
            var loginSession = Context.UserLoginSessions
                                      .Where(m => m.UserLoginSessionID == userSessionId && !m.SessionExpired)
                                      .Select(o => new UserDetails
                                        {
                                            UserID = o.User.UserID,
                                            RoleID = o.User.RoleID,
                                            //UserGuId = o.User.UserGuId,
                                            Email = o.User.Email,
                                            SessionId = o.UserLoginSessionID,
                                            FullName = o.User.Name,
                                            DisplayName = o.User.DisplayName,
                                            ProfilePicture = o.User.ProfilePicture,
                                            DeviceToken = o.LoggedInDeviceToken,
                                            DeviceType = o.DeviceType
                                        }).FirstOrDefault();
            if (loginSession != null && !string.IsNullOrEmpty(loginSession.ProfilePicture) && !loginSession.ProfilePicture.Contains("graph.facebook.com"))
                loginSession.ProfilePicture = Config.UserImages + loginSession.UserID + "/" + loginSession.ProfilePicture;
            return loginSession;
        }

        ActionOutput IUserManager.Logout(Guid SessionID)
        {
            var session = Context.UserLoginSessions.Where(m => !m.SessionExpired && m.UserLoginSessionID == SessionID).FirstOrDefault();
            if (session != null)
            {
                session.SessionExpired = true;
                session.LoggedOutTime = DateTime.UtcNow;
                SaveChanges();
                return new ActionOutput { Status = ActionStatus.Successfull, Message = "Use has been logged out successfully." };
            }
            return new ActionOutput { Status = ActionStatus.Error, Message = "Use is not logged in." };
        }

        ActionOutput<string> IUserManager.UpdateProfilePic(int UserID, string fileName)
        {
            string oldFile = string.Empty;
            var old = Context.Users.Where(m => m.UserID == UserID).FirstOrDefault();
            oldFile = old.ProfilePicture;
            old.ProfilePicture = fileName;
            SaveChanges();
            // Delete Old File
            if (System.IO.File.Exists(Config.UserImages + old.UserID + "/" + oldFile))
                System.IO.File.Delete(Config.UserImages + old.UserID + "/" + oldFile);
            return new ActionOutput<string>
            {
                Status = ActionStatus.Successfull,
                Message = "Profile picture has been changed.",
                Results = new List<string> { Config.UserImages + UserID + "/" + fileName }
            };
        }

        ActionOutput<UserDetails> IUserManager.GetUserInformation(string Email, int LoggedInUserID = 0)
        {
            var user = Context.Users
                              .Where(o => o.Email == Email)
                              .Select(m =>
                                    new UserDetails
                                    {
                                        UserID = m.UserID,
                                        RoleID = m.RoleID,
                                        RegisterVia = m.RegisterVia,
                                        RegistrationIP = m.RegistrationIP,
                                        Email = m.Email,
                                        FullName = m.Name,
                                        DisplayName = m.DisplayName,
                                        ProfilePicture = m.ProfilePicture,
                                        CountryID = m.CountryID,
                                        //CountryName = m.Country.CountryName,
                                        StateID = m.StateID,
                                        //StateName = m.State.StateName,
                                        OtherState = m.OtherState,
                                        City = m.CityID,
                                        //CityName = m.City.CityName,
                                        OtherCity = m.OtherCity,
                                        ZipCode = m.ZipCode,
                                        FailedLoginAttempts = m.FailedAttempts,
                                        CreatedOn = m.DateCreated                                        
                                    }).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePicture) && !user.ProfilePicture.Contains("graph.facebook.com"))
                    user.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;
                return new ActionOutput<UserDetails> { Message = "User information has been retrieved.", Status = ActionStatus.Successfull, Object = user };
            }
            return new ActionOutput<UserDetails> { Message = "User not found.", Status = ActionStatus.Error };
        }

        ActionOutput<UserDetails> IUserManager.GetUserInformation(int UserID, int LoggedInUserID = 0)
        {
            var user = Context.Users
                              .Where(o => o.UserID == UserID)
                              .Select(m =>
                                    new UserDetails
                                    {
                                        UserID = m.UserID,
                                        RoleID = m.RoleID,
                                        RegisterVia = m.RegisterVia,
                                        RegistrationIP = m.RegistrationIP,
                                        Email = m.Email,
                                        FullName = m.Name,
                                        DisplayName = m.DisplayName,
                                        ProfilePicture = m.ProfilePicture,
                                        CountryID = m.CountryID,
                                        //CountryName = m.Country.CountryName,
                                        StateID = m.StateID,
                                        //StateName = m.State.StateName,
                                        OtherState = m.OtherState,
                                        City = m.CityID,
                                        //CityName = m.City.CityName,
                                        OtherCity = m.OtherCity,
                                        ZipCode = m.ZipCode,
                                        FailedLoginAttempts = m.FailedAttempts,
                                        CreatedOn = m.DateCreated
                                    }).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePicture) && !user.ProfilePicture.Contains("graph.facebook.com"))
                    user.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;
                return new ActionOutput<UserDetails> { Message = "User information has been retrieved.", Status = ActionStatus.Successfull, Object = user };
            }
            return new ActionOutput<UserDetails> { Message = "User not found.", Status = ActionStatus.Error };
        }

        ActionOutput IUserManager.ChangePassword(ChangePasswordModel model)
        {
            byte[] oldPassword = Utility.GetEncryptedValue(model.OldPassword);
            byte[] newPassword = Utility.GetEncryptedValue(model.NewPassword);
            User user = Context.Users.Where(m => m.UserID == model.UserID && m.Password == oldPassword).FirstOrDefault();
            if (user == null)
                return new ActionOutput
                {
                    Status = ActionStatus.Error,
                    Message = "Old password is incorrect."
                };
            user.Password = newPassword;
            SaveChanges();
            return new ActionOutput
            {
                Status = ActionStatus.Successfull,
                Message = "Password has been changed."
            };
        }

        ActionOutput IUserManager.UpdatePassword(ChangePasswordWebsiteModel model, int UserID)
        {
            byte[] pass = Utility.GetEncryptedValue(model.OldPassword);
            var user = Context.Users.Where(m => m.UserID == UserID && m.Password == pass).FirstOrDefault();
            if (user == null)
                return new ActionOutput { Status = ActionStatus.Error, Message = "Old password is incorrect!!!" };
            user.Password = Utility.GetEncryptedValue(model.Password);
            SaveChanges();
            return new ActionOutput { Status = ActionStatus.Successfull, Message = "Password has been changed." };
        }

    }
}
