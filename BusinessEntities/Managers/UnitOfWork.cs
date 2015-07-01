using AJAD.Business.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJAD.Data.Model.DataModel;
using AJAD.Data.Repository.Interfaces;
using AJAD.Data.Repository;

namespace AJAD.Business.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private JokeEntities context;
        private IRepository<User> userRepository;

        public UnitOfWork(JokeEntities context)
        {
            this.context = context;
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<User>(context);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
