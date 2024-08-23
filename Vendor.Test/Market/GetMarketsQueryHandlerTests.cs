using Microsoft.EntityFrameworkCore;
using Moq;
using Vendor.Application.Requests.Markets;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

public class GetMarketQueryHandlerTests
{
    private Mock<DbSet<Market>> _marketsDbSetMock;
    private Mock<IVendorDbContext> _dbContextMock;

    public GetMarketQueryHandlerTests()
    {
        var markets = new List<Market>
        {
            new Market { Id = 1, Name = "Market 1" },
            new Market { Id = 2, Name = "Market 2" }
        }.AsQueryable();
        _marketsDbSetMock = DbSetMockUtility.CreateDbSetMock(markets);

        _dbContextMock = new Mock<IVendorDbContext>();
        _dbContextMock.Setup(m => m.Markets).Returns(_marketsDbSetMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnMarkets_When_DataExists()
    {
        var handler = new GetMarketQueryHandler(_dbContextMock.Object);
        var query = new GetMarketQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        var marketList = result.ToList();
        Assert.Equal(2, marketList.Count);
        Assert.Contains(marketList, m => m.Name == "Market 1");
        Assert.Contains(marketList, m => m.Name == "Market 2");
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoDataExists()
    {
        var emptyMarkets = new List<Market>().AsQueryable();

        _marketsDbSetMock = DbSetMockUtility.CreateDbSetMock(emptyMarkets);
        _dbContextMock.Setup(m => m.Markets).Returns(_marketsDbSetMock.Object);

        var handler = new GetMarketQueryHandler(_dbContextMock.Object);
        var query = new GetMarketQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }
}
