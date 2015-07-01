using CoreEntities.Models;
using DataAccessLayer.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Managers
{
    public abstract class BaseManager : DbContext
    {
        public BaseManager()
            : base()
        {

        }
        /// <summary>
        /// protected, it only visible for inherited class
        /// </summary>
        protected void SaveChanges()
        {
            Context.SaveChanges();            
        }
    }
}
