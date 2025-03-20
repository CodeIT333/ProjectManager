using System.Linq.Expressions;

namespace Domain.Commons
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpressAll();
    }
}
