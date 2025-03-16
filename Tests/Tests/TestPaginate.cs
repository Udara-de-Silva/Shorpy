using Microsoft.EntityFrameworkCore;
using Shorpy.DTOs;
using Shorpy.Paginate;
using Shorpy.Tests.DB;
using Shorpy.Tests.Entities;
using Shorpy.Tests.SnSAbles;

namespace Tests;

public class TestPaginate
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
    public async Task Paginate_WhenLimitAndOffsetGiven_ReturnsPaginatedObject()
    {
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Limit = 2,
            Offset = 0
        });
        Assert.That(paginated.Values, Is.Not.Null);
        Assert.That(paginated.Values.Count(), Is.EqualTo(2));
        Assert.That(paginated.Total, Is.EqualTo(_context.Employees.Count()));
    }

    [Test]
    public async Task Paginate_WhenSearchMethodIsLikeAndLimitAndOffsetIsGiven_ReturnsPaginatedObject()
    {
        var searchString = "a";
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Limit = 2,
            Offset = 0,
            Search = new List<SearchModel>(){
                new(){
                    method = "like",
                    value = searchString,
                    field = "name"
                }
            }
        });
        Assert.That(paginated.Values, Is.Not.Null);
        Assert.That(paginated.Values.Count(), Is.EqualTo(2));
        Assert.That(paginated.Total, Is.EqualTo(_context.Employees.Where(e=>e.Name.Contains(searchString)).Count()));
    }
}