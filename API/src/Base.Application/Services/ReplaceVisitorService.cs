using System.Linq.Expressions;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.Services
{
    public class ReplaceVisitorService : ExpressionVisitor, IReplaceVisitorService
    {
        private Expression from, to;

        ///<inheritdoc/>
        public void SetData(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        ///<inheritdoc/>
        public override Expression Visit(Expression node) => node == from ? to : base.Visit(node);
    }
}
