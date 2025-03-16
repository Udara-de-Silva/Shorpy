namespace Shorpy.Tests.Entities;

public class Department
{
    public int Id{get;set;}
    public string Name {get;set;} = null!;
    public virtual List<Employee> Employees {get;set;} = new();
}