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
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Include { get; set; } = new();
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }

        public BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }


        protected void AddIncludeExp(Expression<Func<TEntity, object>> exp) => Include.Add(exp);
        protected void AddOrderBy(Expression<Func<TEntity, object>> exp) => OrderBy = exp;
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> exp) => OrderByDesc = exp;
        protected void AddPagination(int pageIndex, int pageSize)
        {
            if (pageIndex > 0 && pageSize > 0)
            {
                Skip = (pageIndex - 1) * pageSize;
                Take = pageSize;
                IsPaginated = true;
            }
            else
                IsPaginated = false;
        }
    }
}
