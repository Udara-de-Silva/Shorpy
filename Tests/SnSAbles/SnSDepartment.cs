using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Department, object>>;
public class SnSDepartment: ISnSAble
{
    public static readonly type Name = d=>d.Name;
    public static readonly type NoOfEmployees = d=>d.Employees.Count;
}