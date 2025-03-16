using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Employee, object>>;
public class IncEmployee: IIncludable
{
    public static readonly type Department = e=>e.DepartmentNavigation;
}