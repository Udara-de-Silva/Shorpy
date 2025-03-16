/*
File-Name: SortPredicateBuilder
File-Description: Logic for building the sort predicate
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using System.Linq.Expressions;
using Shorpy.Exceptions;
using Shorpy.Helpers;
namespace Shorpy.Sort;
public class SortPredicateBuilder<TEntity> where TEntity : class
{
    private ParameterExpression parameter = null!;

    public Expression Build(IQueryable<TEntity> source, IEnumerable<SortEntity> sorts)
    {
        if (sorts is null || sorts.Count() == 0)
            throw new EmptySortException();
        IQueryable<TEntity> result = source;
        Expression resultExpression = source.Expression;
        int count = 0;
        parameter = SetPE(sorts.First());
        // use the replacer to replace the parameter of each expression so that even if there are multiple search criteria, all of them will use the same parameter
        var replacer = new ParamReplacer(parameter);

        foreach (var item in sorts)
        {
            string command;
            var type = typeof(TEntity);
            LambdaExpression orderByExpression;
            // member access expression is directly extracted from the SnS_***** classes and casted back to an Expression with a lambda function and passed through the replacer to have the same parameter
            Expression<Func<TEntity, object>> expr = (Expression<Func<TEntity, object>>)replacer.Visit((Expression<Func<TEntity, object>>)item.Field);
            Expression propertyAccess = expr.Body;// CreateMemberExpression<TEntity>.Create((Expression<Func<TEntity, object>>)item.field, parameter);
            if (count == 0)
            {
                command = item.IsDesc ? "OrderByDescending" : "OrderBy";
            }
            else
            {
                command = item.IsDesc ? "ThenByDescending" : "ThenBy";

            }

            /*
              2025-03-13 Udara de Silva
              if custom sort order is specified
            */
            if (item.CustomSortOrder is not null && item.CustomSortOrder.Length > 0)
            {
                Expression expression = Expression.Constant(item.CustomSortOrder.Length);
                for (int i = 0; i < item.CustomSortOrder.Length; i++)
                {
                    var value = Expression.Constant(item.CustomSortOrder[i]);
                    var condition = Expression.Equal(propertyAccess, value);
                    var rank = Expression.Constant(i);

                    expression = Expression.Condition(condition, rank, expression);
                }
                orderByExpression = Expression.Lambda(expression, parameter);
            }
            else
            {
                orderByExpression = Expression.Lambda(propertyAccess, parameter);
            }



            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, propertyAccess.Type }, resultExpression, Expression.Quote(orderByExpression));

            count++;
        }
        return resultExpression;
    }

    private ParameterExpression SetPE(SortEntity se)
    {
        try
        {
            var PE = ((Expression<Func<TEntity, object>>)se.Field).Parameters.First();
            return PE;
        }
        catch (System.Exception)
        {
            throw new SortParameterNotSetException();
        }
    }
}