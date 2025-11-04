using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Persistence.Data.Contexts;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _storeDbContext) : IUnitOfWork
    {
        //private Dictionary<string, object> _repositories=new Dictionary<string, object>();



        //public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        //{
        //    var type = typeof(TEntity).Name;

        //    if (! _repositories.ContainsKey(type))
        //    {

        //        var repository = new GenericRepository<TKey, TEntity>(_storeDbContext);
        //        _repositories.Add(type, repository);




        //    }

        //    return (IGenericRepository<TKey, TEntity>)_repositories[type];



        //}





        //container of repositories of all types as dictionary  key: string =name of the type
        //and key is object = repository 

        private ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();



        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        {
           
            return (IGenericRepository < TKey, TEntity > ) _repositories.GetOrAdd(typeof(TKey).Name,new GenericRepository<TKey,TEntity>(_storeDbContext));




        }

        public async Task<int> SaveChangesAsync()
        {
            return await _storeDbContext.SaveChangesAsync();
        }
    }
}
