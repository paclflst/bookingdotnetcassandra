using Cassandra.Data.Linq;
using BookingService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookingService.Model.Interface;

namespace BookingService.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : IEntity
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public Task<T> Get<T>(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.GetTable<T>()
                .Where(expression)
                .FirstOrDefault()
                .ExecuteAsync();
        }

        public Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.GetTable<T>()
                .Where(expression)
                .ExecuteAsync(); 
        }

        public async Task Add<T>(T entity)
        {
            var command = RepositoryContext.GetTable<T>()
                .Insert(entity);
            await command.ExecuteAsync();
        }

        public void AddInBatch<T>(T entity, IBatch batch)
        {
            var command = RepositoryContext.GetTable<T>()
                .Insert(entity);
            batch.Append(command);
        }
    }
}
