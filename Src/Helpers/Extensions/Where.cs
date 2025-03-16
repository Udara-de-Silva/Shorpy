/*
File-Name: WherePredicate
File-Description: An extension method so that IQueryable from EF understands Shorpy
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/

using System.Linq.Expressions;
using Shorpy.Exceptions;
using Shorpy.Search;

namespace Shorpy.Helpers;
public static class WherePredicate
{
  public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, IEnumerable<SearchEntity> search)
  {
    var predicate = new SearchPredicateBuilder<TEntity>().Build(search);
    if (predicate is null)
      throw new SearchPredicateIsNullException();
    var resultExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(TEntity) }, source.Expression, predicate);
    var result = source.Provider.CreateQuery<TEntity>(resultExpression);
    return result;
  }
}