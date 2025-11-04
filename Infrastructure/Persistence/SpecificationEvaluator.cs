using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Persistence
{
    public class SpecificationEvaluator
    {


        //generate dynamic query  
        public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> inputQuery
            ,ISpecification<TKey,TEntity> specification) where TEntity : BaseEntity<TKey>
        {

            var query = inputQuery;


            if(specification.Criteria is not null)
            {
              //query =  query.Where(specification.Criteria); //context.entity.where(conditions)
  
            }




            if(specification.OrderBy is not null)
            {
                query=query.OrderBy(specification.OrderBy);
            }
            
            else if(specification.OrderByDescending is not null)
            {
                query=query.OrderByDescending(specification.OrderByDescending);
            }



            if (specification.IsPagination)
            {

                query = query.Skip(specification.Skip).Take(specification.Take);


            }




            foreach (var include in specification.Includes)
            {
                query = query.Include(include);

            }


            return query;
            //context.entity.where(conditions).include().include()
            //specification.Includes.Aggregate(query, (query, includeExpression) =>
            //query.Include(includeExpression));

        }
    }
}
