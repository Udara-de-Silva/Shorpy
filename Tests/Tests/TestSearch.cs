using Microsoft.EntityFrameworkCore;
using Shorpy.DTOs;
using Shorpy.Paginate;
using Shorpy.Tests.DB;
using Shorpy.Tests.Entities;
using Shorpy.Tests.SnSAbles;

namespace Tests;

public class TestSearch
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
    public async Task Paginate_WhenSearchMethodIsEqual_ReturnsPaginatedObject()
    {
        var searchString = "Shash";
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Search = new List<SearchModel>(){
                new(){
                    method = "equal",
                    value = searchString,
                    field = "name"
                }
            }
        });
        // loop through all returned results
        if (paginated.Values is not null && paginated.Values.Count() > 0)
        {
            foreach (var employee in paginated.Values)
            {
                if (!employee.Name.Equals(searchString)) Assert.Fail("Incorrect search result");
            }
            Assert.Pass();
        }
        else
            Assert.Fail("Returned empty result set");
    }

    [Test]
    public async Task Paginate_WhenSearchMethodIsLike_ReturnsPaginatedObject()
    {
        var searchString = "Sha";
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Search = new List<SearchModel>(){
                new(){
                    method = "like",
                    value = searchString,
                    field = "name"
                }
            }
        });
        // loop through all returned results
        if (paginated.Values is not null && paginated.Values.Count() > 0)
        {
            foreach (var employee in paginated.Values)
            {
                if (!employee.Name.Contains(searchString)) Assert.Fail("Incorrect search result");
            }
            Assert.Pass();
        }
        else
            Assert.Fail("Returned empty result set");
    }

    [Test]
    public async Task Paginate_WhenSearchMethodIsNotEqual_ReturnsPaginatedObject()
    {
        var searchString = "Shash";
        var paginated = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_context, new PaginateModel()
        {
            Search = new List<SearchModel>(){
                new(){
                    method = "notequal",
                    value = searchString,
                    field = "name"
                }
            }
        });
        // loop through all returned results
        if (paginated.Values is not null && paginated.Values.Count() > 0)
        {
            foreach (var employee in paginated.Values)
            {
                if (employee.Name.Equals(searchString)) Assert.Fail("Incorrect search result");
            }
            Assert.Pass();
        }
        else
            Assert.Fail("Returned empty result set");
    }

    [Test]
    public async Task Paginate_WhenSearchMethodIsGt_ReturnsPaginatedObject()
    {
        var searchString = "1";
        var paginated = await Paginate<Department, SnSDepartment, IncDepartment>.PaginateWithTracking(_context, new PaginateModel()
        {
            Search = new List<SearchModel>(){
                new(){
                    method = "gt",
                    value = searchString,
                    field = "noOfEmployees"
                }
            }
        });
        // loop through all returned results
        if (paginated.Values is not null)
        {

            // only one department has more than 1 employee
            Assert.That(paginated.Values.Count() == 1, "Incorrect search result");

            if (paginated.Values.Single().Id == 3 && paginated.Values.Single().Name == "Boring Stuff")
                Assert.Pass();
            else
                Assert.Fail("Incorrect search result");
        }
        else
            Assert.Fail("Returned empty result set");
    }

    [Test]
    public async Task Paginate_WhenNoMatchingResults_ReturnsEmptyPaginatedObject()
    {
        var searchString = "10";
        var paginated = await Paginate<Department, SnSDepartment, IncDepartment>.PaginateWithTracking(_context, new PaginateModel()
        {
            Search = new List<SearchModel>(){
                new(){
                    method = "gt",
                    value = searchString,
                    field = "noOfEmployees"
                }
            }
        });
        // loop through all returned results
        if (paginated.Values is not null)
        {

            // only one department has more than 1 employee
            Assert.That(paginated.Values.Count() == 0, "Incorrect search result");
        }
        else
            Assert.Fail("Returned empty result set");
    }
}