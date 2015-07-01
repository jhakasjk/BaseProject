using AJAD.Core.Models;
using AJAD.Business.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJAD.Business.Managers.Interfaces
{
    public interface IUserManager
    {
        string GetWelcome();
        int SaveOrUpdateUser(UserModel user);
        UserDetails CheckUserLogin(Guid userSessionId);
    }
}
