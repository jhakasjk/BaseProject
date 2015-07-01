using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AJAD.Business.Managers.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //TEntity Add(TEntity entity);
        //bool Add(IEnumerable<TEntity> items);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        List<TEntity> GetAll();

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        void SaveChanges();
    }
}
