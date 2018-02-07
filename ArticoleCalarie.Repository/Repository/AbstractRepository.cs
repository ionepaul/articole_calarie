using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class AbstractRepository<T> : IDisposable, IAbstractRepository<T> where T : class
    {
        #region Local Variables

        protected ArticoleCalarieDataContext _ctx;

        protected DbSet<T> _dbset;

        #endregion

        #region Constructor

        public AbstractRepository(ArticoleCalarieDataContext dataContext)
        {
            _ctx = dataContext;

            _dbset = dataContext.Set<T>();
        }

        #endregion

        #region Public Methods

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            _dbset.Add(entity);

            await _ctx.SaveChangesAsync();
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);

            _ctx.SaveChanges();
        }


        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }

        public virtual void SaveChanges()
        {
            _ctx.SaveChanges();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbset.Remove(entity);

            await _ctx.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);

            _ctx.SaveChanges();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        #endregion
    }
}