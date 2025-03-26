using System.Linq.Expressions;

namespace Domain.Commons
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpressAll();

        public Specification<T> And(Specification<T> spec)
        {
            return new AndSpecification<T>(this, spec);
        }
    }

    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpressAll()
        {
            var leftExpression = _left.ToExpressAll();
            var rightExpression = _right.ToExpressAll();

            var paramExpr = Expression.Parameter(typeof(T));
            var combinedBody = Expression.AndAlso(
                Expression.Invoke(leftExpression, paramExpr),
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(combinedBody, paramExpr);
        }
    }
}
