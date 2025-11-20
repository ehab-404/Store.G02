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
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        //private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();
        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();


        public IGenericRepository<TEntity, TKey> CreateRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //var repoName = typeof(TEntity).Name;

            //if (!_repositories.ContainsKey(repoName))
            //{
            //    var repo = new GenericRepository<TEntity, TKey>(_context);
            //    _repositories.Add(repoName, repo);
            //}

            //return (IGenericRepository<TEntity, TKey>) _repositories[repoName];


            var repoName = typeof(TEntity).Name;

            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(repoName, new GenericRepository<TEntity, TKey>(_context));

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

