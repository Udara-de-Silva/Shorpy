using Microsoft.EntityFrameworkCore;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.DB;
public class TestDBContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department()
            {
                Id = 1,
                Name = "Creative Products"
            },
            new Department()
            {
                Id = 2,
                Name = "Human Resources"
            },
            new Department()
            {
                Id = 3,
                Name = "Boring Stuff"
            }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee()
            {
                Id = 1,
                Name = "Shash",
                DepartmentNavigationId = 1
            },
            new Employee(){
                Id = 2,
                Name = "Vuks",
                DepartmentNavigationId = 2
            },
            new Employee(){
                Id = 3,
                Name = "Jacob",
                DepartmentNavigationId = 3
            },
            new Employee(){
                Id = 4,
                Name = "Oliver",
                DepartmentNavigationId = 3
            },
            new Employee(){
                Id = 5,
                Name = "Tim",
                DepartmentNavigationId = 3
            }
        );
    }
}