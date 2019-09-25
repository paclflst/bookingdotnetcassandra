using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingService.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        Task<T> Get<T>(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> expression);
        Task Add<T>(T entity);
        void AddInBatch<T>(T entity, IBatch batch);
    }
}