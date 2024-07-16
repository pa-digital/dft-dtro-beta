using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DfT.DTRO.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        ParameterExpression parameter1 = current.Parameters[0];
        var visitor = new ReplaceParameterVisitor(other.Parameters[0], parameter1);
        var body2WithParam1 = visitor.Visit(other.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(current.Body, body2WithParam1), parameter1);
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        ParameterExpression parameter1 = current.Parameters[0];
        var visitor = new ReplaceParameterVisitor(other.Parameters[0], parameter1);
        var body2WithParam1 = visitor.Visit(other.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(current.Body, body2WithParam1), parameter1);
    }

    public static Expression<Func<T, bool>> AllOf<T>(IEnumerable<Expression<Func<T, bool>>> expressions)
    {
        var expr = expressions.First();

        foreach (var next in expressions.Skip(1))
        {
            expr = expr.AndAlso(next);
        }

        return expr;
    }

    public static Expression<Func<T, bool>> AnyOf<T>(IEnumerable<Expression<Func<T, bool>>> expressions)
    {
        var expr = expressions.First();

        foreach (var next in expressions.Skip(1))
        {
            expr = expr.OrElse(next);
        }

        return expr;
    }

    private class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => ReferenceEquals(node, _oldParameter) ? _newParameter : base.VisitParameter(node);
    }
}
