using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        //product = productRepository.where(x => x.id> 5);
        //product.ToList(); // To save that to db
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        //_context.Entry(entity).state = EntityState.Modified
        TEntity Update(TEntity entity);



    }
}
