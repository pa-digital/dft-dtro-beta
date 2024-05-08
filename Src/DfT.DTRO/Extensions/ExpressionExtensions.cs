using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DfT.DTRO.Extensions;

/// <summary>
/// Provides extensions and helpers for <see cref="Expression"/> types.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Produces an <see cref="Expression"/> that is the conjunction (logical AND)
    /// of the current expression and the expression provided in the argument.
    /// </summary>
    /// <typeparam name="T">
    /// The input argument of the boolean functions
    /// that are represented by the expressions.
    /// </typeparam>
    /// <param name="current">The current <see cref="Expression"/>.</param>
    /// <param name="other">The <see cref="Expression"/> to conjunct the current expression with.</param>
    /// <returns>The produced <see cref="Expression"/>.</returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        ParameterExpression parameter1 = current.Parameters[0];
        var visitor = new ReplaceParameterVisitor(other.Parameters[0], parameter1);
        var body2WithParam1 = visitor.Visit(other.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(current.Body, body2WithParam1), parameter1);
    }

    /// <summary>
    /// Produces an <see cref="Expression"/> that is the disjunction (logical OR)
    /// of the current expression and the expression provided in the argument.
    /// </summary>
    /// <typeparam name="T">
    /// The input argument of the boolean functions
    /// that are represented by the expressions.
    /// </typeparam>
    /// <param name="current">The current <see cref="Expression"/>.</param>
    /// <param name="other">The <see cref="Expression"/> to disjunct the current expression with.</param>
    /// <returns>The produced <see cref="Expression"/>.</returns>
    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        ParameterExpression parameter1 = current.Parameters[0];
        var visitor = new ReplaceParameterVisitor(other.Parameters[0], parameter1);
        var body2WithParam1 = visitor.Visit(other.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(current.Body, body2WithParam1), parameter1);
    }

    /// <summary>
    /// Produces an <see cref="Expression"/> that is a conjunction (logical AND)
    /// of all the provided expressions.
    /// </summary>
    /// <typeparam name="T">
    /// The input argument of the boolean functions
    /// that are represented by the expressions.
    /// </typeparam>
    /// <param name="expressions">The expressions to conjunct.</param>
    /// <returns>The produced <see cref="Expression"/>.</returns>
    public static Expression<Func<T, bool>> AllOf<T>(IEnumerable<Expression<Func<T, bool>>> expressions)
    {
        var expr = expressions.First();

        foreach (var next in expressions.Skip(1))
        {
            expr = expr.AndAlso(next);
        }

        return expr;
    }

    /// <summary>
    /// Produces an <see cref="Expression"/> that is a disjunction (logical OR)
    /// of all the provided expressions.
    /// </summary>
    /// <typeparam name="T">
    /// The input argument of the boolean functions
    /// that are represented by the expressions.
    /// </typeparam>
    /// <param name="expressions">The expressions to disjunct.</param>
    /// <returns>The produced <see cref="Expression"/>.</returns>
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
