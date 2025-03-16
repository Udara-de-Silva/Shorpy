namespace Shorpy.Tests.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DepartmentNavigationId { get; set; }
    public virtual Department DepartmentNavigation { get; set; } = null!;
}