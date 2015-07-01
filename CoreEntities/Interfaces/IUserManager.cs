using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Interfaces
{
    public interface IUserManager
    {
        /// <summary>
        /// Added Just for testing
        /// </summary>
        /// <returns></returns>
        string GetWelcomeMessage();

        /// <summary>
        /// Check logged in user by session key
        /// </summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        UserDetails CheckUserLogin(Guid userSessionId);

        /// <summary>
        /// log out user
        /// </summary>
        /// <param name="Logout"></param>
        /// <returns></returns>
        ActionOutput Logout(Guid SessionID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="fileName"></param>
        ActionOutput<string> UpdateProfilePic(int UserID, string fileName);

        /// <summary>
        /// Get User by Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> GetUserInformation(string Email, int LoggedInUserID = 0);

        /// <summary>
        /// Get User by UserID
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> GetUserInformation(int UserID, int LoggedInUserID = 0);

        /// <summary>
        /// Change logged in user's password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ActionOutput ChangePassword(ChangePasswordModel model);

        /// <summary>
        /// Update the password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ActionOutput UpdatePassword(ChangePasswordWebsiteModel model, int UserID);
    }
}
