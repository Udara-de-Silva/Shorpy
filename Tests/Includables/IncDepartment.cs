using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Department, object>>;
public class IncDepartment: IIncludable
{
    public static readonly type Employees = d => d.Employees;
}