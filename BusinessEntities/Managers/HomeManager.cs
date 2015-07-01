using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreEntities.Models;
using DataAccessLayer.Model.DataModel;
using CoreEntities.Enums;
using CoreEntities.Classes;
using CoreEntities.Interfaces;
using System.Web.Hosting;
using LinqKit;

namespace BusinessEntities.Managers
{
    public class HomeManager : BaseManager, IHomeManager
    {
        private int Unsubscribe;
        private const string baseUrl = "http://localhost:50172";
        //private const string baseUrl = "http://www.mydomain.com";

        public HomeManager()
        {
            Unsubscribe = (int)SubscriptionStatus.Unsubscribe;
        }

        string IHomeManager.LogExceptionToDatabase(Exception ex)
        {
            ErrorLog errorObj = new ErrorLog();
            errorObj.Message = ex.Message;
            errorObj.StackTrace = ex.StackTrace;
            errorObj.InnerException = ex.InnerException == null ? "" : ex.InnerException.Message;
            errorObj.LoggedInDetails = "";
            errorObj.LoggedAt = DateTime.UtcNow;
            Context.ErrorLogs.Add(errorObj);
            Context.SaveChanges();
            return errorObj.ErrorLogID.ToString();
        }

        ActionOutput<UserDetails> IHomeManager.SaveOrUpdateUser(UserDetails User)
        {
            if (User.UserID > 0)
            {
                var existingDisplayName = Context.Users.Where(m => m.UserID != User.UserID && m.DisplayName == User.DisplayName).FirstOrDefault();
                if (existingDisplayName != null)
                    return new ActionOutput<UserDetails>
                    {
                        Message = "Please choose different displayname, It's already taken by another user.",
                        Status = ActionStatus.Error,
                        Object = User
                    };
                var existing = Context.Users.Where(m => m.UserID == User.UserID).FirstOrDefault();
                if (existing != null)
                {
                    existing.Name = User.FullName;
                    existing.DisplayName = User.DisplayName;
                    if (!string.IsNullOrEmpty(User.ProfilePicture))
                        existing.ProfilePicture = User.ProfilePicture;
                    existing.CountryID = User.CountryID;
                    existing.StateID = User.StateID;
                    existing.OtherState = User.OtherState;
                    existing.CityID = User.City;
                    existing.OtherCity = User.OtherCity;
                    existing.ZipCode = User.ZipCode;
                    SaveChanges();

                    if (User.ProfilePicture != null && !User.ProfilePicture.Contains("graph.facebook.com"))
                        User.ProfilePicture = Config.UserImages + User.UserID + "/" + User.ProfilePicture;
                    return new ActionOutput<UserDetails>
                    {
                        Message = "User successfully updated.",
                        Status = ActionStatus.Successfull,
                        Object = User
                    };
                }
                return new ActionOutput<UserDetails>
                {
                    Message = "Invalid UserID. User does not exists.",
                    Status = ActionStatus.Error,
                    Object = User
                };
            }
            else
            {
                // Check for duplicate Email
                User old = Context.Users.Where(m => m.Email == User.Email).FirstOrDefault();
                if (old != null)
                {
                    return new ActionOutput<UserDetails>
                    {
                        Message = "Email already exists.",
                        Status = ActionStatus.Error,
                        Object = new UserDetails { }
                    };
                }
                // Check for duplicate Username
                old = Context.Users.Where(m => m.DisplayName == User.DisplayName).FirstOrDefault();
                if (old != null)
                {
                    return new ActionOutput<UserDetails>
                    {
                        Message = "Username already exists.",
                        Status = ActionStatus.Error,
                        Object = new UserDetails { }
                    };
                }
                if (string.IsNullOrEmpty(User.ProfilePicture))
                    User.ProfilePicture = Constants.DefaultUserPic;
                User user = new User
                {
                    RoleID = User.RoleID,
                    RegisterVia = User.RegisterVia,
                    RegistrationIP = User.RegistrationIP,
                    Email = User.Email,
                    Password = User.Password,
                    ResetPassword = User.ResetPassword,
                    PasswordResetCode = User.PasswordResetCode,
                    Name = User.FullName,
                    DisplayName = User.DisplayName,
                    ProfilePicture = User.ProfilePicture,
                    CountryID = User.CountryID,
                    StateID = User.StateID,
                    OtherState = User.OtherState,
                    CityID = User.City,
                    OtherCity = User.OtherCity,
                    ZipCode = User.ZipCode,
                    FailedAttempts = User.FailedLoginAttempts,
                    DateCreated = DateTime.UtcNow
                };
                Context.Users.Add(user);
                SaveChanges();
                User.UserID = user.UserID;

                if (!System.IO.Directory.Exists(HostingEnvironment.MapPath(Config.UserImages + user.UserID)))
                    System.IO.Directory.CreateDirectory(HostingEnvironment.MapPath(Config.UserImages + user.UserID));

                if (!User.ProfilePicture.EndsWith("default-user.png"))
                    System.IO.File.Move(HostingEnvironment.MapPath(Config.UserImages + "0/" + User.ProfilePicture), HostingEnvironment.MapPath(Config.UserImages + user.UserID + "/" + User.ProfilePicture));

                User.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;
                return new ActionOutput<UserDetails>
                {
                    Message = "User successfully registered",
                    Status = ActionStatus.Successfull,
                    Object = User
                };
            }
        }

        ActionOutput<UserDetails> IHomeManager.LoginUser(LoginModel Login, string DeviceToken, int DeviceType)
        {
            byte[] pass = Utility.GetEncryptedValue(Login.Password);
            int roleid = (int)UserRoles.User;
            UserDetails user = Context.Users.Where(m => m.Email == Login.Email && m.Password == pass && m.RoleID == roleid)
                                            .Select(m => new UserDetails
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
                                                StateID = m.StateID,
                                                OtherState = m.OtherState,
                                                City = m.CityID,
                                                OtherCity = m.OtherCity,
                                                ZipCode = m.ZipCode,
                                                FailedLoginAttempts = m.FailedAttempts,
                                                CreatedOn = m.DateCreated,
                                                IsLoggedIn = true
                                                //SubscriptionStatus = m.EmailSubscriptions.Any() ? m.EmailSubscriptions.FirstOrDefault().Status : Unsubscribe
                                            })
                                            .FirstOrDefault();
            if (user == null)
                return new ActionOutput<UserDetails>
                {
                    Message = "Invalid Email/Password.",
                    Status = ActionStatus.Error,
                    Object = new UserDetails { }
                };
            if (!user.ProfilePicture.Contains("graph.facebook.com"))
                user.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;
            // Save User into Login Sessions
            Guid Guid = Guid.NewGuid();
            var OldSession = Context.UserLoginSessions.Where(m => m.UserID == user.UserID && !m.SessionExpired).FirstOrDefault();
            if (OldSession == null)
            {
                Context.UserLoginSessions.Add(new UserLoginSession
                {
                    LoggedInTime = DateTime.UtcNow,
                    LoggedOutTime = null,
                    SessionExpired = false,
                    UserID = user.UserID,
                    UserLoginSessionID = Guid,
                    LoggedInDeviceToken = DeviceToken,
                    DeviceType = DeviceType
                });
                user.SessionId = Guid;
                SaveChanges();
            }
            else
            {
                user.SessionId = OldSession.UserLoginSessionID;
                OldSession.LoggedInDeviceToken = DeviceToken;
                OldSession.DeviceType = DeviceType;
                SaveChanges();
            }
            return new ActionOutput<UserDetails>
            {
                Message = "User logged in successfuly.",
                Status = ActionStatus.Successfull,
                Object = user
            };
        }

        ActionOutput<UserDetails> IHomeManager.FacebookLogin(FaceBookUser Login, string DeviceToken, int DeviceType)
        {
            if (string.IsNullOrEmpty(Login.Email))
            {
                Login.Email = Login.FacebookUserID + "@yourdomain.com";
            }
            int roleid = (int)UserRoles.User;
            int[] registervia = { (int)RegisterVia.AndroidFacebook, (int)RegisterVia.IPhoneFacebook, (int)RegisterVia.WebsiteFacebook };
            // Check for duplicate Email
            var old = Context.Users.Where(m => m.Email == Login.Email).FirstOrDefault();
            if (old != null && old.FacbookUserID != Login.FacebookUserID && !registervia.Contains(old.RegisterVia))
            {
                return new ActionOutput<UserDetails>
                {
                    Message = "User with same email is already exists with normal registration.",
                    Status = ActionStatus.Error,
                    Object = null
                };
            }
            UserDetails user = Context.Users.Where(m => m.FacbookUserID == Login.FacebookUserID && m.RoleID == roleid && registervia.Contains(m.RegisterVia))
                                            .Select(m => new UserDetails
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
                                                StateID = m.StateID,
                                                OtherState = m.OtherState,
                                                City = m.CityID,
                                                OtherCity = m.OtherCity,
                                                ZipCode = m.ZipCode,
                                                FailedLoginAttempts = m.FailedAttempts,
                                                CreatedOn = m.DateCreated,
                                                IsLoggedIn = true
                                                //SubscriptionStatus = m.EmailSubscriptions.Any() ? m.EmailSubscriptions.FirstOrDefault().Status : Unsubscribe
                                            })
                                            .FirstOrDefault();
            if (user == null)
            {
                // Register the user if not found in database
                User userNew = new User
                {
                    RoleID = (int)UserRoles.User,
                    FacbookUserID = Login.FacebookUserID,
                    RegisterVia = Login.RegisterVia,
                    RegistrationIP = Login.RegistrationIP,
                    Email = Login.Email,
                    Password = null,
                    ResetPassword = false,
                    PasswordResetCode = null,
                    Name = Login.FullName,
                    DisplayName = Login.DisplayName,
                    ProfilePicture = Login.PictureUrl,
                    CountryID = Login.CountryID,
                    StateID = null,
                    OtherState = Login.OtherState,
                    CityID = null,
                    OtherCity = Login.OtherCity,
                    ZipCode = null,
                    FailedAttempts = 0,
                    DateCreated = DateTime.UtcNow
                };
                Context.Users.Add(userNew);
                SaveChanges();
                Login.UserID = userNew.UserID;

                user = new UserDetails
                {
                    UserID = Login.UserID,
                    RoleID = (int)UserRoles.User,
                    RegisterVia = Login.RegisterVia,
                    RegistrationIP = Login.RegistrationIP,
                    Email = Login.Email,
                    FullName = Login.FullName,
                    DisplayName = Login.DisplayName,
                    ProfilePicture = Login.PictureUrl,
                    CreatedOn = DateTime.UtcNow,
                    IsLoggedIn = true,
                    OtherCity = Login.OtherCity,
                    OtherState = Login.OtherState,
                    CountryID = Login.CountryID
                    //CountryName = userNew.Country != null ? userNew.Country.CountryName : ""
                };
            }
            //user.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;
            // Save User into Login Sessions
            Guid Guid = Guid.NewGuid();
            var OldSession = Context.UserLoginSessions.Where(m => m.UserID == user.UserID && !m.SessionExpired).FirstOrDefault();
            if (OldSession == null)
            {
                Context.UserLoginSessions.Add(new UserLoginSession
                {
                    LoggedInTime = DateTime.UtcNow,
                    LoggedOutTime = null,
                    SessionExpired = false,
                    UserID = user.UserID,
                    UserLoginSessionID = Guid,
                    LoggedInDeviceToken = DeviceToken,
                    DeviceType = DeviceType
                });
                user.SessionId = Guid;
                SaveChanges();
            }
            else
            {
                user.SessionId = OldSession.UserLoginSessionID;
                OldSession.LoggedInDeviceToken = DeviceToken;
                OldSession.DeviceType = DeviceType;
                SaveChanges();
            }
            return new ActionOutput<UserDetails>
            {
                Message = "User logged in successfuly.",
                Status = ActionStatus.Successfull,
                Object = user
            };
        }

        ActionOutput<UserDetails> IHomeManager.ForgotPassword(string UserEmail)
        {
            User DBUser = Context.Users.Where(m => m.Email == UserEmail).FirstOrDefault();
            UserDetails user = Context.Users.Where(m => m.Email == UserEmail)
                                     .Select(m => new UserDetails
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
                                         StateID = m.StateID,
                                         OtherState = m.OtherState,
                                         City = m.CityID,
                                         OtherCity = m.OtherCity,
                                         ZipCode = m.ZipCode,
                                         FailedLoginAttempts = m.FailedAttempts,
                                         CreatedOn = m.DateCreated
                                     })
                                     .FirstOrDefault();
            if (user == null)
                return new ActionOutput<UserDetails>
                {
                    Message = "No user found with this email.",
                    Status = ActionStatus.Error
                };
            DBUser.ResetPassword = user.ResetPassword = true;
            DBUser.PasswordResetCode = user.PasswordResetCode = Utility.CreateRandomString(5);
            SaveChanges();

            if (!user.ProfilePicture.Contains("graph.facebook.com"))
                user.ProfilePicture = Config.UserImages + user.UserID + "/" + user.ProfilePicture;

            // Send Email To User
            string template = Utility.ReadFileText("/EmailTemplates/ForgotPassword.html");
            template = template.Replace("{FullName}", user.FullName)
                             .Replace("{PasswordResetCode}", user.PasswordResetCode);
            Email.SendEmail("noreply@jokeaday.com", UserEmail.Split(','), null, null, "Reset password code", template);

            return new ActionOutput<UserDetails>
            {
                Message = "Password reset code has been sent to registered email.",
                Status = ActionStatus.Successfull,
                Object = user
            };
        }

        ActionOutput IHomeManager.ResetPassword(ResetPasswordModel model)
        {
            int[] regvia = { (int)RegisterVia.Android, (int)RegisterVia.Website, (int)RegisterVia.IPhone };
            User user = Context.Users.Where(m => m.Email == model.UserEmail).FirstOrDefault();
            if (user == null)
                return new ActionOutput
                {
                    Message = "User does not exists.",
                    Status = ActionStatus.Error
                };
            if (!user.ResetPassword)
                return new ActionOutput
                {
                    Message = "User doesn't requested to reset the password.",
                    Status = ActionStatus.Error
                };
            if (user.PasswordResetCode != model.ResetCode)
                return new ActionOutput
                {
                    Message = "Password reset code does not match.",
                    Status = ActionStatus.Error
                };
            if (!regvia.Contains(user.RegisterVia))
                return new ActionOutput
                {
                    Message = "Password can not be reset for facebook user.",
                    Status = ActionStatus.Error
                };
            user.ResetPassword = false;
            user.PasswordResetCode = null;
            user.Password = Utility.GetEncryptedValue(model.NewPassword);
            SaveChanges();

            return new ActionOutput
            {
                Message = "Password has been reset.",
                Status = ActionStatus.Successfull
            };
        }
        
        ActionOutput IHomeManager.CheckFBUser(string FacebookID)
        {
            var user = Context.Users.Where(m => m.FacbookUserID == FacebookID).FirstOrDefault();
            if (user == null)
                return new ActionOutput { Status = ActionStatus.Successfull, Message = "User is not registered." };
            return new ActionOutput { Status = ActionStatus.Error, Message = "User is already registered." };
        }
    }
}
