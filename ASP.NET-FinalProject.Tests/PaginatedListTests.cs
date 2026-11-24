using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ASP.NET_FinalProject.Tests
{
    [TestFixture]
    public class PaginatedListTests
    {
        private ApplicationDbContext _context = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            for (int i = 1; i <= 10; i++)
            {
                _context.Categories.Add(new Category { Id = i, Name = $"Category {i}" });
            }

            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ReturnsFirstPageCorrectly()
        {
            var query = _context.Categories.OrderBy(c => c.Id);

            var result = await PaginatedList<Category>.CreateAsync(query, pageIndex: 1, pageSize: 4);

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.PageIndex, Is.EqualTo(1));
            Assert.That(result.TotalPages, Is.EqualTo(3));
            Assert.That(result.HasPreviousPage, Is.False);
            Assert.That(result.HasNextPage, Is.True);
            Assert.That(result[0].Id, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAsync_ReturnsMiddlePageCorrectly()
        {
            var query = _context.Categories.OrderBy(c => c.Id);

            var result = await PaginatedList<Category>.CreateAsync(query, pageIndex: 2, pageSize: 4);

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.PageIndex, Is.EqualTo(2));
            Assert.That(result.HasPreviousPage, Is.True);
            Assert.That(result.HasNextPage, Is.True);
            Assert.That(result[0].Id, Is.EqualTo(5));
        }

        [Test]
        public async Task CreateAsync_ReturnsLastPageCorrectly()
        {
            var query = _context.Categories.OrderBy(c => c.Id);

            var result = await PaginatedList<Category>.CreateAsync(query, pageIndex: 3, pageSize: 4);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.PageIndex, Is.EqualTo(3));
            Assert.That(result.HasPreviousPage, Is.True);
            Assert.That(result.HasNextPage, Is.False);
            Assert.That(result[0].Id, Is.EqualTo(9));
        }

        [Test]
        public async Task CreateAsync_ClampsPageIndexToOne_WhenPageIndexIsZeroOrNegative()
        {
            var query = _context.Categories.OrderBy(c => c.Id);

            var resultZero = await PaginatedList<Category>.CreateAsync(query, pageIndex: 0, pageSize: 4);
            var resultNegative = await PaginatedList<Category>.CreateAsync(query, pageIndex: -5, pageSize: 4);

            Assert.That(resultZero.PageIndex, Is.EqualTo(1));
            Assert.That(resultZero[0].Id, Is.EqualTo(1));
            Assert.That(resultNegative.PageIndex, Is.EqualTo(1));
            Assert.That(resultNegative[0].Id, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAsync_ClampsPageIndexToLastPage_WhenPageIndexExceedsTotalPages()
        {
            var query = _context.Categories.OrderBy(c => c.Id);

            var result = await PaginatedList<Category>.CreateAsync(query, pageIndex: 99, pageSize: 4);

            Assert.That(result.PageIndex, Is.EqualTo(3));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.HasNextPage, Is.False);
        }

        [Test]
        public async Task CreateAsync_ReturnsEmptyList_WhenSourceHasNoItems()
        {
            var emptyQuery = _context.Categories.Where(c => c.Id < 0);

            var result = await PaginatedList<Category>.CreateAsync(emptyQuery, pageIndex: 1, pageSize: 4);

            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result.TotalPages, Is.EqualTo(0));
            Assert.That(result.HasPreviousPage, Is.False);
            Assert.That(result.HasNextPage, Is.False);
        }
    }
}