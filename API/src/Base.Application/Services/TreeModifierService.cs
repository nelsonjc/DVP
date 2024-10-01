using System.Linq.Expressions;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.Services
{
    public class TreeModifierService : ExpressionVisitor, ITreeModifierService
    {
        private readonly IReplaceVisitorService _replaceVisitor;

        public TreeModifierService(IReplaceVisitorService replaceVisitor)
        {
            _replaceVisitor = replaceVisitor;
        }

        ///<inheritdoc/>
        public Expression<Func<T, bool>> CombineAnd<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2)
        {
            _replaceVisitor.SetData(filter1.Parameters[0], filter2.Parameters[0]);
            var rewrittenBody1 = _replaceVisitor.Visit(filter1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(rewrittenBody1, filter2.Body), filter2.Parameters);
            return newFilter;
        }

        ///<inheritdoc/>
        public Expression<Func<T, bool>> CombineOr<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2)
        {
            _replaceVisitor.SetData(filter1.Parameters[0], filter2.Parameters[0]);
            var rewrittenBody1 = _replaceVisitor.Visit(filter1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(Expression.Or(rewrittenBody1, filter2.Body), filter2.Parameters);
            return newFilter;
        }
    }
}
