/*
File-Name: OrderByPredicate
File-Description: An extension method so that IQueryable from EF understands Shorpy
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using Shorpy.Sort;

namespace Shorpy.Helpers;
public static class OrderByPredicate
{
  public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, IEnumerable<SortEntity> sorts) where TEntity : class
  {
    var result = source.Provider.CreateQuery<TEntity>(new SortPredicateBuilder<TEntity>().Build(source, sorts));
    return (IOrderedQueryable<TEntity>)result;
  }
}