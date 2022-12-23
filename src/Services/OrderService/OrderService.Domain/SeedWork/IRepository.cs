using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }


    Task<List<T>> GetAll();
    Task<List<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
    Task<List<T>> Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
    Task<T> GetById(Guid id);
    Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includes);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
    Task<T> AddAsync(T entity);
    T Update(T entity);
  }
}
