using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data.Contexts;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext _context) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTracked = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return isTracked ?
                await _context.Products.Include(p => p.Brand).Include(p => p.Type).ToListAsync() as IEnumerable<TEntity>
                : await _context.Products.Include(p => p.Brand).Include(p => p.Type).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }

            return isTracked ?
                await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
                return await _context.Products.Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity;
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification, bool isTracked = false)
        {
            return isTracked
                ? await ApplySpecification(specification).ToListAsync()
                : await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> specification)
        {
            return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification);
        }


    }
}

