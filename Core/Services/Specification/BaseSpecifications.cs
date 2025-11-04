using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;

namespace Services.Specification
{
    public class BaseSpecifications<TKey, TEntity> : ISpecification<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public IList<Expression<Func<TEntity, object>>>? Includes { get; set; } = new List<Expression<Func<TEntity,object>>>();
    
        
        
        public Expression<Func<TEntity, bool>>? Criteria { get ; set ; }
        public Expression<Func<TEntity, object>>? OrderBy {get; set ; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get ; set; }


       public int Skip { get; set; }
       public int Take { get; set; }
       public bool IsPagination { get; set; }



        public BaseSpecifications(Expression<Func<TEntity, bool>>? expression)
        {
            
            Criteria=expression;
        }


        public void ApplyPagination( int pageIndex, int pageSize)
        {

            IsPagination = true;
            Skip = (pageIndex - 1) * pageSize;

            Take = pageSize;




        }

        public void AddOrderBy(Expression<Func<TEntity, object>>?  expression)
        {

            OrderBy=expression;

        }

        public void AddOrderByDescinding(Expression<Func<TEntity, object>>? expression)
        {

            OrderByDescending = expression;

        }

    }

    
}
