using AJAD.Data.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJAD.Business.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        //public IRepository<User> UserRepository { get; }
        void Save();
        virtual void Dispose(bool disposing);      
    }
}
