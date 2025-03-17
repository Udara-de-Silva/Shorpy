using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using incType = Expression<Func<Employee, object>>;
public class SnSEmployee: ISnSAble
{
    public static readonly incType Name = e=>e.Name;
}