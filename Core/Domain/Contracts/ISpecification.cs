using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{

    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        List<Expression<Func<TEntity, object>>> Include { get; set; }
        Expression<Func<TEntity, bool>>? Criteria { get; set; }
        Expression<Func<TEntity, object>>? OrderBy { get; set; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        bool IsPaginated { get; set; }
    }
}
