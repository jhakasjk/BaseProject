//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.Model.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLoginSession
    {
        public System.Guid UserLoginSessionID { get; set; }
        public Nullable<int> UserID { get; set; }
        public System.DateTime LoggedInTime { get; set; }
        public Nullable<System.DateTime> LoggedOutTime { get; set; }
        public bool SessionExpired { get; set; }
        public string LoggedInDeviceToken { get; set; }
        public int DeviceType { get; set; }
    
        public virtual User User { get; set; }
    }
}