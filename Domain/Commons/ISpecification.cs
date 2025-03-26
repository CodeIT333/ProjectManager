using System.Linq.Expressions;

namespace Domain.Commons
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpressAll();
    }
}
