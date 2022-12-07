using DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Repository.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal SocialMediaContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(SocialMediaContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<int> Delete(object id)
        {
            try
            {
                int count = 0;
                dbSet.Remove(dbSet.Find(id));
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    await context.DisposeAsync();
                }
                throw new Exception(e.Message);
            }
        }

        public async Task<int> DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                int count = 0;
                dbSet.RemoveRange(entities);
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    await context.DisposeAsync();
                }
                throw new Exception(e.Message);
            }
        }


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
           
        }


        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = dbSet;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }


        public async Task<int> Insert(T entity)
        {
            try
            {
                int count = 0;
                await dbSet.AddAsync(entity);
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (SqlException e)
            {
                await context.DisposeAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<int> InsertRange(IEnumerable<T> entities)
        {
            try
            {
                int count = 0;
                await dbSet.AddRangeAsync(entities);
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    await context.DisposeAsync();
                }
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Update(T entity)
        {
            try
            {
                int count = 0;
                dbSet.Update(entity);
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    await context.DisposeAsync();
                }
                throw new Exception(e.Message);
            }
        }

        public async Task<int> UpdateRange(IEnumerable<T> entities)
        {
            try
            {
                int count = 0;
                dbSet.UpdateRange(entities);
                count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    await context.DisposeAsync();
                }
                throw new Exception(e.Message);
            }
        }



    }
}
