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
    public class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity:BaseEntity<TKey>
    {
        private readonly StoreDbContext _storeDbContext;

        public GenericRepository(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }




        public async Task<IEnumerable<TEntity>?> GetAllAsync(bool changeTracker = false)
        {

            // i canceled bad way to include navigation properties  of product 

            //if (typeof(TEntity) == typeof(Product))
            //{
            //    return changeTracker ? await _storeDbContext.products.Include(p => p.Brand).Include(p => p.Type).ToListAsync()  as IEnumerable<TEntity>
            //   : await _storeDbContext.products.Include(p => p.Brand).Include(p => p.Type).AsNoTracking().ToListAsync() as IEnumerable<TEntity> ;


            //}





            var All= await _storeDbContext.Set<TEntity>().ToListAsync();
          

            return changeTracker ? All :
                await _storeDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>?> GetAllAsync(ISpecification<TKey, TEntity> specification, bool changeTracker = false)
        {



            return await AppliedSpecification(specification).ToListAsync();

            




        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TKey, TEntity> specification)
        {
            return await AppliedSpecification(specification).FirstOrDefaultAsync();
        }




        public async Task<TEntity?> GetByIdAsync(TKey key)
        {


            return await _storeDbContext.Set<TEntity>().FindAsync(key);
        }




        public async Task AddAsync(TEntity entity)
        {
            await _storeDbContext.AddAsync(entity);
        }

       

        

        public void Update(TEntity entity)
        {
            _storeDbContext.Update(entity);
        }


        public void Delete(TEntity entity)
        {
            _storeDbContext.Remove(entity);
        }
        public async Task<int> GetCountAsync(ISpecification<TKey, TEntity> specification)
        {
            return await AppliedSpecification(specification).CountAsync();

            
        }



        private IQueryable<TEntity> AppliedSpecification(ISpecification<TKey, TEntity> specification)
        {

            return SpecificationEvaluator.GetQuery(_storeDbContext.Set<TEntity>().AsQueryable(), specification);

        }

       
    }
}
