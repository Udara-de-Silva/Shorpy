using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using incType = Expression<Func<Employee, object>>;
public class IncEmployee: IIncludable
{
    public static readonly incType Department = e=>e.DepartmentNavigation;
}