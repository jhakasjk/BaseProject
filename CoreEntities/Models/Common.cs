using CoreEntities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Models
{
    public class UserSessions
    {
        public string UserIP { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActivityOn { get; set; }
        public List<UserPageView> UserPageViews { get; set; }
    }

    public class UserPageView
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ID { get; set; }
        public int Count { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActivityOn { get; set; }
    }

    public class APICountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryShortCode { get; set; }
    }

    public class APIStates
    {
        public int StateID { get; set; }
        public int? CountryID { get; set; }
        public string StateName { get; set; }
        public string StateShortCode { get; set; }
        public int PostalCode { get; set; }
    }

    public class APICities
    {
        public int CityID { get; set; }
        public int? StateID { get; set; }
        public string CityName { get; set; }
    }

    [DataContract]
    public class APIUser
    {
        [DataMember]
        public int UserID { get; set; }
        //public string UserGuId { get; set; }
        [DataMember]
        public int RegisterVia { get; set; }
        [DataMember]
        public string RegistrationIP { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string Password { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "Fullname should not be more than 50 chars long.")]
        public string FullName { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "DisplayName should not be more than 50 chars long.")]
        public string DisplayName { get; set; }
        [DataMember]
        public string ProfilePicture { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required")]
        public int? CountryID { get; set; }
        [DataMember]
        public int? StateID { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "State should not be more than 50 chars long.")]
        public string OtherState { get; set; }
        [DataMember]
        public int? City { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "City should not be more than 50 chars long.")]
        public string OtherCity { get; set; }
        [DataMember]
        [RegularExpression(@"^[0-9]{5,6}$", ErrorMessage = "Zip Code should be a 5-6 digit number.")]
        public string ZipCode { get; set; }

        [DataMember]
        public bool Subscribe { get; set; }
    }

    public class WebUser
    {
        public int UserID { get; set; }
        public int RegisterVia { get; set; }
        public string RegistrationIP { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password does not match with password")]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "Fullname should not be more than 50 chars long.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "DisplayName should not be more than 50 chars long.")]
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        [StringLength(50, ErrorMessage = "State should not be more than 50 chars long.")]
        public string OtherState { get; set; }
        public int? City { get; set; }
        [StringLength(50, ErrorMessage = "City should not be more than 50 chars long.")]
        public string OtherCity { get; set; }
        [RegularExpression(@"^[0-9]{5,6}$", ErrorMessage = "Zip Code should be a 5-6 digit number.")]
        public string ZipCode { get; set; }
        public bool Subscribe { get; set; }
    }

    [DataContract]
    public class ChangePasswordModel
    {
        [Required, DataMember, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string OldPassword { get; set; }
        [Required, DataMember, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string NewPassword { get; set; }

        public int UserID { get; set; }
    }

    [DataContract]
    public class APIUpdateUser
    {
        [DataMember]
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "Fullname shoyuld not be more than 50 chars long.")]
        public string FullName { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required"), StringLength(50, ErrorMessage = "DisplayName should not be more than 50 chars long.")]
        public string DisplayName { get; set; }
        [DataMember]
        public string ProfilePicture { get; set; }
        [DataMember]
        public int? CountryID { get; set; }
        [DataMember]
        public int? StateID { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "State should not be more than 50 chars long.")]
        public string OtherState { get; set; }
        [DataMember]
        public int? City { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "City should not be more than 50 chars long.")]
        public string OtherCity { get; set; }
        [DataMember]
        [RegularExpression(@"^[0-9]{5,6}$", ErrorMessage = "Zip Code should be a 5-6 digit number.")]
        public string ZipCode { get; set; }
    }

    public class UserDetails
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int RegisterVia { get; set; }
        public string RegistrationIP { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public bool ResetPassword { get; set; }
        public string PasswordResetCode { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
        public int? StateID { get; set; }
        public string StateName { get; set; }
        public string OtherState { get; set; }
        public int? City { get; set; }
        public string CityName { get; set; }
        public string OtherCity { get; set; }
        public string ZipCode { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid SessionId { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public string DeviceToken { get; set; }
        public int DeviceType { get; set; }
        public int UserJokes { get; set; }
        public int UserFollowers { get; set; }
        public int UserFollowing { get; set; }
        public bool IsFollowMe { get; set; }
        public bool IFollowHim { get; set; }
        public int SubscriptionStatus { get; set; }
    }

    public class ExceptionModal
    {
        public Exception Exception { get; set; }
        public UserDetails User { get; set; }
        public string FormData { get; set; }
        public string QueryData { get; set; }
        public string RouteData { get; set; }
    }

    public class ExceptionReturnModal
    {
        public string ErrorID { get; set; }
        public string ErrorText { get; set; }
        public bool DatabaseLogStatus { get; set; }
    }

    public class ActionOutputBase
    {
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
        public List<String> Results { get; set; }
    }

    public class ActionOutput<T> : ActionOutputBase
    {
        public T Object { get; set; }
        public List<T> Results { get; set; }
    }

    public class ActionOutput : ActionOutputBase
    {
        public long ID { get; set; }
    }
    public class ApiActionOutput
    {
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
        public Object JsonData { get; set; }
    }

    public class ApiActionPagingOutput
    {
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
        public Object JsonData { get; set; }
        public long TotalRecords { get; set; }
    }

    //public class AuthorizationResponse : APIUser
    //{
    //    public AuthorizeErrorCode ErrorCode { get; set; }
    //}

    public class PagingResultBase : ActionOutputBase
    {
        public long TotalResults { get; set; }
    }

    public class PagingResult<T> : PagingResultBase
    {
        public List<T> Data { get; set; }
    }

    public class AddAttachment
    {
        public byte[] Stream { get; set; }

        public string MediaType { get; set; }
    }

    public class FaceBookUserWeb
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
    }

    public class FaceBookUser
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string FacebookUserID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required")]
        public string DisplayName { get; set; }
        [DataMember]
        public string PictureUrl { get; set; }
        [DataMember]
        //[Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required")]
        public int RegisterVia { get; set; }
        [DataMember]
        public string RegistrationIP { get; set; }

        [DataMember]
        public int? CountryID { get; set; }
        [DataMember]
        public string OtherState { get; set; }
        [DataMember]
        public string OtherCity { get; set; }
        [DataMember]
        public bool Subscribe { get; set; }
    }

    [DataContract]
    public class LoginModel
    {
        [DataMember]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string Password { get; set; }
    }

    [DataContract]
    public class LogoutModel
    {
        [DataMember]
        public Guid SessionID { get; set; }
    }

    [DataContract]
    public class TopJokesModel
    {
        [DataMember]
        public DateTime? Date { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
    }

    [DataContract]
    public class ResetPasswordModel
    {
        [DataMember]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string UserEmail { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Required")]
        public string ResetCode { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string NewPassword { get; set; }
    }

    [DataContract]
    public class JokeSearchModel
    {
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public string SearchKeywords { get; set; }
        [DataMember]
        public int? CategoryID { get; set; }
        [DataMember]
        public int? UserID { get; set; }
        [DataMember]
        public string OrderBy { get; set; }
        [DataMember]
        public bool IsAscending { get; set; }
    }

    [DataContract]
    public class JokesModel
    {
        [DataMember]
        public long JokeID { get; set; }
        [DataMember]
        public int? UserID { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Required")]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(250, ErrorMessage = "Title should not be more than 250 chars long.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(8000, ErrorMessage = "Joke should not be more than 8000 chars long.")]
        public string Joke { get; set; }
        [DataMember]
        public string SearchKeywords { get; set; }
    }

    public class JokesViewModel
    {
        public long JokeID { get; set; }
        public long? UserID { get; set; }
        public bool IsMyFavorite { get; set; }
        public bool IsLiked { get; set; }
        public string UserDisplayName { get; set; }
        public string UserFullName { get; set; }
        public string UserProfilePic { get; set; }
        public int? CategoryID { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Joke { get; set; }
        public string Keywords { get; set; }
        public DateTime? DatePublished { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public int TotalFavorite { get; set; }
        public decimal JokeRating { get; set; }
        public int RegisterVia { get; set; }
        public decimal LoggedInUserRating { get; set; }
        public int FlagCount { get; set; }
        public bool AmIFlagged { get; set; }
    }

    [DataContract]
    public class CategoryModel
    {
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public int? ParentID { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(100, ErrorMessage = "Category should not be more than 100 chars long.")]
        public string CategoryName { get; set; }

        [DataMember, StringLength(250, ErrorMessage = "Description should not be more than 250 chars long.")]
        public string Description { get; set; }
    }

    [DataContract]
    public class UserPreferredCategoryModel
    {
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public int? ParentID { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataMember, StringLength(100, ErrorMessage = "Category should not be more than 100 chars long.")]
        public string CategoryName { get; set; }

        [DataMember]
        public bool IsPreferred { get; set; }
    }

    [DataContract]
    public class CommentModel
    {
        [DataMember]
        public int JokeID { get; set; }
        [DataMember]
        public int? ParentID { get; set; }
        [DataMember, StringLength(8000, ErrorMessage = "Comment should not be more than 8000 chars long.")]
        public string Comment { get; set; }

        public int UserID { get; set; }
    }

    public class Comments
    {
        public long? JokeID { get; set; }
        public long? ParentID { get; set; }
        public long CommentID { get; set; }
        public long? UserID { get; set; }
        public string Comment { get; set; }
        public string UserDisplayName { get; set; }
        public string UserFullName { get; set; }
        public string UserProfilePic { get; set; }
        public DateTime CommentOn { get; set; }
    }

    public class CommentViewModel : Comments
    {
        public IEnumerable<Comments> CommentReply { get; set; }
    }

    [DataContract]
    public class JokeRatingModel
    {
        [DataMember]
        public long JokeRatingID { get; set; }
        [DataMember]
        public int JokeID { get; set; }
        public int UserID { get; set; }
        [DataMember]
        public decimal Rating { get; set; }
    }

    public class PushNotificationMessage
    {
        public string alert { get; set; }
        public int badge { get; set; }
        public string sound { get; set; }
    }

    public class PushNotificationDevice
    {
        public string DeviceToken { get; set; }
        public int DeviceType { get; set; }
    }

    public class UserNotificationModel
    {
        public int NotificationType { get; set; }
        public DateTime DateCreated { get; set; }
        public int? UserID { get; set; }
        public int UserNotificationID { get; set; }
    }

    //public sealed class Singleton
    //{
    //    Singleton() { }
    //    public static Singleton Instance { get { return Nested.instance; } }
    //    class Nested
    //    {
    //        // Explicit static constructor to tell C# compiler         
    //        // not to mark type as beforefieldinit         
    //        static Nested() { }
    //        internal static readonly Singleton instance = new Singleton();
    //    }
    //}

    public class JokeWinnerViewModel
    {
        public long JokeWinnerID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public decimal RewardAmount { get; set; }
        public int RewardTypeID { get; set; }

        public JokesViewModel JokeDetails { get; set; }
        //public UserDetails JokeUserDetails { get; set; }
    }

    public class PopupModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class ChangePasswordWebsiteModel
    {
        [Required, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string OldPassword { get; set; }
        [Required, StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password should be same.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string UserEmail { get; set; }
    }

    public class EmailShare
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string UserEmail { get; set; }

        public int JokeID { get; set; }
        public string JokeURL { get; set; }
    }

    public class ResetPasswordModelWebsite
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ResetCode { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "Password should be between 6-14 Chars only.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password should be same.")]
        public string ConfirmPassword { get; set; }
    }

    public class ContactModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name should be less than 50 charaters long.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[\+]*[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-??]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Phone number should be a 10-15 digit number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "City Name should be less than 50 charaters long.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(2000, ErrorMessage = "Comments should be less than 2000 charaters long.")]
        public string Comments { get; set; }
    }

    public class RewardTypeViewModel
    {
        public int RewardTypeId { get; set; }
        public string RewardName { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
