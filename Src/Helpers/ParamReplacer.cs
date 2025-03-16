/*
File-Name: ParamReplacer
File-Description: A helper to replace the param of the lambda expression
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/

using System.Linq.Expressions;

namespace Shorpy.Helpers;
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