using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using incType = Expression<Func<Department, object>>;
public class IncDepartment: IIncludable
{
    public static readonly incType Employees = d => d.Employees;
}