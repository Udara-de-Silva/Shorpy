using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Employee, object>>;
public class SnSEmployee: ISnSAble
{
    public static readonly type Name = e=>e.Name;
}