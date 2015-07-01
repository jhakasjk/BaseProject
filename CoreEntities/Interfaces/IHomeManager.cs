using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Interfaces
{
    public interface IHomeManager
    {
        /// <summary>
        /// Log Exception into database
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        string LogExceptionToDatabase(Exception ex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> SaveOrUpdateUser(UserDetails user);

        /// <summary>
        /// Login user by passing email/password
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> LoginUser(LoginModel Login, string DeviceToken, int DeviceType);

        /// <summary>
        /// Login via facebook
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="DeviceToken"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> FacebookLogin(FaceBookUser Login, string DeviceToken, int DeviceType);

        /// <summary>
        /// Sends password reset code on user's registered email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        ActionOutput<UserDetails> ForgotPassword(string Email);

        /// <summary>
        /// Changes user's password using password reset code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ActionOutput ResetPassword(ResetPasswordModel model);

        /// <summary>
        /// Check for Registered User in FB login
        /// </summary>
        /// <param name="FacebookID"></param>
        /// <returns></returns>
        ActionOutput CheckFBUser(string FacebookID);
    }
}
