/*
File-Name: SearchModelConverter
File-Description: converts an array of search models to search entities which can be understood by the application layer
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using System.Reflection;
using Shorpy.DTOs;
using Shorpy.Exceptions;
using Shorpy.Search;

namespace Shorpy.Paginate.Converters;
// Search Model to Search Object class (SM_SO)
public class SearchModelConverter
{
  public static IEnumerable<SearchEntity>? Convert(IEnumerable<SearchModel>? se, Type T)
  {
    // return null if se is empty
    if (se is null || se.Count() == 0)
      return null;

    // maintain converted search entities and later assign it to the search object
    var sl = new List<SearchEntity>();
    // loop search items
    foreach (var item in se)
    {
      var searchItem = ConvertSearchEntity(item, T);
      // add search item to search list
      sl.Add(searchItem);
    }

    // return search object
    return sl;
  }

  private static SearchEntity ConvertSearchEntity(SearchModel item, Type T)
  {
    var searchItem = new SearchEntity();
    // set search method
    SearchMethods? searchMethod = (typeof(SearchMethods).GetField(item.method, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)?.GetValue(null)) as SearchMethods?;
    if (!searchMethod.HasValue)
    {
      // throw exception
      throw new SearchMethodNotFoundException();
    }
    // set method
    searchItem.Method = searchMethod.Value;
    // set value
    searchItem.Value = item.value;
    // set field => via reflection
    var field = T.GetField(item.field, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)?.GetValue(null) as System.Linq.Expressions.Expression;
    if (field is null)
      throw new SearchFieldNotFoundException();
    searchItem.Field = field;

    // set condition
    searchItem.Condition = item.isAnd ? SearchCondition.and : SearchCondition.or;

    // if nested field is available
    if (item.and is not null)
      searchItem.And = ConvertSearchEntity(item.and, T);
    if (item.or is not null)
      searchItem.Or = ConvertSearchEntity(item.or, T);

    return searchItem;
  }
}