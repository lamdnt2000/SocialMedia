using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.GenericRepo
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes  = null
        );
        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task<int> Insert(T entity);
        Task<int> InsertRange(IEnumerable<T> entities);
        Task<int> UpdateRange(IEnumerable<T> entities);
        Task<int> Update(T entity);
        Task<int> Delete(object id);
        Task<int> DeleteRange(IEnumerable<T> entities);
       
    }
}
