/*
File-Name: IncludesConverter
File-Description: converts an array of strings containing navigation names to their respective inclubadle lambda expressions
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using System.Linq.Expressions;
using System.Reflection;
using Shorpy.Exceptions;

namespace Shorpy.Paginate.Converters;
// sort Model to sort Object class (SM_SO)
public class IncludesConverter
{
  public static IEnumerable<LambdaExpression>? Convert(IEnumerable<string>? se, Type tp)
  {
    if (se is null)
      return null;
    List<LambdaExpression> exps = new List<LambdaExpression>();
    foreach (var item in se)
    {
      var field = tp.GetField(item, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
      if (field is null)
        throw new IncludeFieldNotFoundException();
      var value = field.GetValue(tp);
      if (value is null)
        throw new IncludeFieldEmptyException();

      // 2023-04-18 : Udara de Silva <udara@thesoftwarefirm.lk>
      // check if the includable property is an array. If so; spread the array before pushing it to the exps array
      // if not an array, just add it to the exps array
      if (value.GetType().IsArray)
      {
        exps.AddRange((IEnumerable<LambdaExpression>)value);
      }
      else
        exps.Add((LambdaExpression)value);
    }
    return exps;
  }
}