using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int FacebookUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastFailedAttempt { get; set; }
        public int FailedAttempts { get; set; }
        public int Status { get; set; }
        public int SourceType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateConfirmed { get; set; }
    }

    public class PictureModel
    {
        public string Base64ImageString { get; set; }
    }
}
