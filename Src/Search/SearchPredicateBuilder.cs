/*
File-Name: SearchPredicateBuilder
File-Description: builds the search predicate based on the search entities
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Shorpy.Exceptions;
namespace Shorpy.Search;
public class SearchPredicateBuilder<TEntity>
{
  private ParameterExpression PE = null!;
  /*
  Summary: builds the search predicate
  Author: udara@thesoftwarefirm.lk
  Created-On : 2022-08-11
  Function-Description: builds a new search predicate
  Parameters: IEnumerable<SearchEntity> search              
  Return-Value: Expression<Func<TEntity, bool>>
  */
  public Expression<Func<TEntity, bool>>? Build(IEnumerable<SearchEntity> search)
  {
    if (search.Count() == 0)
    {
      throw new EmptySearchException();
    }
    // set the PE (Parameter Expression) based on the first search field and use the same for the rest of the search fields
    PE = SetPE(search.First());
    // maintain last expression => used inside the loop
    Expression? lastExp = null;
    // maintain the condition for the next search item => used inside the loop
    SearchCondition nextSearchCondition = SearchCondition.and;
    foreach (var item in search)
    {
      Expression exp = buildPredicate(item);
      // nested and
      // if (item.and is not null)
      // {
      //   exp = Expression.AndAlso(exp, buildPredicate(item.and));
      // }
      // // nested or
      // if (item.or is not null)
      // {
      //   exp = Expression.OrElse(exp, buildPredicate(item.or));
      // }

      if (lastExp != null)
      {
        if (nextSearchCondition == SearchCondition.and)
          lastExp = Expression.AndAlso(lastExp, exp);
        else if (nextSearchCondition == SearchCondition.or)
          lastExp = Expression.OrElse(lastExp, exp);
      }
      else
        lastExp = exp;

      nextSearchCondition = item.Condition;
    }
    Expression<Func<TEntity, bool>>? lambda = null;
    if (lastExp is not null)
      lambda = Expression.Lambda<Func<TEntity, bool>>(lastExp, PE);
    return lambda;
  }

  private ParameterExpression SetPE(SearchEntity se)
  {
    try
    {
      var PE = ((Expression<Func<TEntity, object>>)se.Field).Parameters.First();
      return PE;
    }
    catch (System.Exception)
    {
      throw new SearchParameterNotSetException();
    }
  }

  private Expression buildPredicate(SearchEntity item)
  {
    if (item.Field is null)
      throw new SearchFieldNotFoundException();
    // use the replacer to replace the parameter of each expression so that even if there are multiple search criteria, all of them will use the same parameter
    var replacer = new ParamReplacer(PE);
    // member access expression is directly extracted from the SnS_***** classes and casted back to an Expression with a lambda function and passed through the replacer to have the same parameter
    Expression<Func<TEntity, object>> expr = (Expression<Func<TEntity, object>>)replacer.Visit((Expression<Func<TEntity, object>>)item.Field);
    // Extract and store the member expression
    Expression ME = expr.Body ?? throw new SearchPredicateMemberExpressionIsNullException();
    // Hold the type of variable
    Type? propertyType;
    switch (expr.Body.NodeType)
    {
      // if the type of the expression is a Call expression; this is most likely calling a Select function in the field of the SnS_***** class
      case ExpressionType.Call:
        propertyType = (ME as MethodCallExpression)?.Method.ReturnType;
        break;
      case ExpressionType.Convert:
      case ExpressionType.ConvertChecked:
        var ue = ME as UnaryExpression;
        // reassign ME
        ME = (ue?.Operand) as MemberExpression ?? throw new SearchPredicateMemberExpressionIsNullException();
        propertyType = ((((ue?.Operand) as MemberExpression)?.Member) as PropertyInfo)?.PropertyType;
        break;
      default:
        propertyType = (((ME as MemberExpression)?.Member) as PropertyInfo)?.PropertyType;
        break;
    }

    if (propertyType is null)
      throw new SearchFieldTypeNotFoundException();

    // get converter
    var converter = TypeDescriptor.GetConverter(propertyType);
    // if the property type is of Enumerable, do not try to run the converter
    if (!converter.CanConvertFrom(typeof(string)) && propertyType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
    {
      throw new SearchFieldValueMismatchException();
    }

    object? propertyValue; // convert to original type
    if (item.Value == null)
    {
      propertyValue = null;
    }
    else
    {
      if (propertyType == typeof(DateOnly))
      {
        propertyValue = DateOnly.Parse(item.Value);
      }
      else
      {
        try
        {
          propertyValue = converter.ConvertFromInvariantString(item.Value);
        }
        catch (Exception)
        {
          throw new SearchFieldValueMismatchException();
        }
      }

    }


    Expression? exp;
    // this will hold a reference to the value 
    // this will also enable parameterized sql queries
    var holdpv = new holdPropertyValue { v = propertyValue };
    var cons1 = Expression.Convert(Expression.PropertyOrField(Expression.Constant(holdpv), "v"), propertyType);

    try
    {
      switch (item.Method)
      {
        case SearchMethods.equal:
          exp = Expression.Equal(ME, cons1);
          break;
        case SearchMethods.notequal:
          exp = Expression.NotEqual(ME, cons1);
          break;
        case SearchMethods.gt:
          exp = Expression.GreaterThan(ME, cons1);
          break;
        case SearchMethods.lt:
          exp = Expression.LessThan(ME, cons1);
          break;
        case SearchMethods.like:
          exp = Expression.Call(ME, "Contains", null, cons1);
          break;
        case SearchMethods.wherein:
          /* 2025-03-04: udara
           if the ME is an int enumerable and item.value can be parsed an an int, use INT as the generic type */
          if (ME.Type.IsGenericType && ME.Type.GetGenericArguments().First() == typeof(int))
          {
            if (int.TryParse(item.Value, out int parsedValue))
              exp = Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(int) }, ME, Expression.Constant(parsedValue));
            else throw new SearchMethodNotSupportedException();
          }
          else
            exp = Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(string) }, ME, Expression.Constant(item.Value));
          break;

        /*
          2024-09-23 - udara
          the above 'wherein' searches for complete matches. Wheres as the following "wherepartiallyin" searches for strings
          that are even partially present in a given list of options
          Eg: needle = 'a', haystack = ['aaa','bbb','ccc','a'], result = ['aaa','a']
        */
        case SearchMethods.wherepartiallyin:
          var p = Expression.Parameter(typeof(string), "x");
          var strParam = Expression.Parameter(typeof(string), "str");
          // x.Contains(str)
          var body = Expression.Call(p, "Contains", null, Expression.Constant(item.Value));
          // x => x.Contains(str)
          var lambdaExpression = Expression.Lambda<Func<string, bool>>(body, p);
          exp = Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), new[] { typeof(string) }, ME, lambdaExpression);
          break;
        default:
          throw new SearchMethodNotSupportedException();
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new SearchMethodInvalidException();
    }

    // recursively process and & or predicates
    if (item.And is not null)
    {
      exp = Expression.AndAlso(exp, buildPredicate(item.And));
    }
    // nested or
    if (item.Or is not null)
    {
      exp = Expression.OrElse(exp, buildPredicate(item.Or));
    }
    return exp;
  }

  private sealed class holdPropertyValue
  {
    public dynamic? v;
  }
}

// used to replace the parameter expression of all the member expressions to use the same parameter
sealed class ParamReplacer : ExpressionVisitor
{
  private readonly ParameterExpression _param;

  internal ParamReplacer(ParameterExpression param)
  {
    _param = param;
  }

  // protected override Expression VisitMember(MemberExpression node)
  // {
  //   return base.VisitMember(node);
  // }

  protected override Expression VisitParameter(ParameterExpression node)
  {
    // replace the parameter when the passed parameter and the original parameter are of the same type. If the passed parameter is of a different type; most likely the parameter belongs to a different function like the "select" function
    if (_param.Type == node.Type)
      return base.VisitParameter(_param);
    else return base.VisitParameter(node);
  }
}