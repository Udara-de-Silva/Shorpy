/*
File-Name: SortModelConverter
File-Description: converts an array of sort models to sort entities which can be understood by the application layer
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using System.Reflection;
using Shorpy.DTOs;
using Shorpy.Exceptions;
using Shorpy.Sort;

namespace Shorpy.Paginate.Converters;
public class SortModelConverter
{
  public static IEnumerable<SortEntity>? Convert(IEnumerable<SortModel>? se, Type T)
  {
    // return null if se is empty
    if (se is null || se.Count() == 0)
      return null;
    // maintain converted sort entities and later assign it to the sort object
    var sl = new List<SortEntity>();
    // loop sort items
    foreach (var item in se)
    {
      var SortItem = new SortEntity();
      // set field => via reflection
      var field = T.GetField(item.field, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)?.GetValue(null) as System.Linq.Expressions.Expression;
      if (field is null)
        throw new SortFieldNotFoundException();
      SortItem.Field = field;
      // set condition
      SortItem.IsDesc = item.isDesc ?? false;
      // add sort item to sort list
      sl.Add(SortItem);
    }
    // return sort object
    return sl;
  }
}