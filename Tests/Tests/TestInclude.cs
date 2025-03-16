using Microsoft.EntityFrameworkCore;
using Shorpy.DTOs;
using Shorpy.Paginate;
using Shorpy.Tests.DB;
using Shorpy.Tests.Entities;
using Shorpy.Tests.SnSAbles;

namespace Tests;

public class TestInclude
{

    private TestDBContext _context = null!;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TestDBContext>()
                .UseInMemoryDatabase(databaseName: "CompanyDatabase")
                .Options;

        _context = new TestDBContext(options);
        _context.Database.EnsureCreated();
    }

    [Test]
    public async Task Paginate_WhenIncludeSpecified_ReturnsPaginatedObjectWithIncludedNavigation()
    {
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Limit = 2,
            Offset = 0,
            Search = new List<SearchModel>(){
                new(){
                    method = "like",
                    value = "a",
                    field = "name"
                }
            },
            Include = new List<string>(){"Department"}
        });
        Assert.That(paginated.Values, Is.Not.Null);
        Assert.That(paginated.Values.Where(d=>d.DepartmentNavigation is null), Is.Empty);
    }
}