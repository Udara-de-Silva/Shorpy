using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using incType = Expression<Func<Department, object>>;
public class SnSDepartment: ISnSAble
{
    public static readonly incType Name = d=>d.Name;
    public static readonly incType NoOfEmployees = d=>d.Employees.Count;
}