using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenericRepository<TKey,TEntity> where TEntity:BaseEntity<TKey>
    {

        Task<IEnumerable<TEntity>?> GetAllAsync(bool changeTracker = false);

        Task<IEnumerable<TEntity>?> GetAllAsync(ISpecification<TKey,TEntity> specification ,bool changeTracker = false);

        Task<int> GetCountAsync(ISpecification<TKey, TEntity> specification);
        Task<TEntity?> GetByIdAsync(TKey key);

        Task<TEntity?> GetByIdAsync(ISpecification<TKey, TEntity> specification );

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);

        void Delete(TEntity entity);


    }
}
