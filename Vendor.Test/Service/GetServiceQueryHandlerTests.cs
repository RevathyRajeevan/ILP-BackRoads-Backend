using Microsoft.EntityFrameworkCore;
using Moq;
using Vendor.Application.Requests.Markets;
using Vendor.Application.Requests.Services;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

public class GetServiceQueryHandlerTests
{
    private Mock<DbSet<Service>> _servicesDbSetMock;
    private Mock<IVendorDbContext> _dbContextMock;

    public GetServiceQueryHandlerTests()
    {
        var services = new List<Service>
        {
            new Service { Id = 1, Name = "Service 1" },
            new Service { Id = 2, Name = "Service 2" }
        }.AsQueryable();
        _servicesDbSetMock = DbSetMockUtility.CreateDbSetMock(services);

        _dbContextMock = new Mock<IVendorDbContext>();
        _dbContextMock.Setup(m => m.Services).Returns(_servicesDbSetMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnMarkets_When_DataExists()
    {
        var handler = new GetServiceQueryHandler(_dbContextMock.Object);
        var query = new GetServiceQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        var serviceList = result.ToList();
        Assert.Equal(2, serviceList.Count);
        Assert.Contains(serviceList, m => m.Name == "Service 1");
        Assert.Contains(serviceList, m => m.Name == "Service 2");
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoDataExists()
    {
        var emptyServices = new List<Service>().AsQueryable();

        _servicesDbSetMock = DbSetMockUtility.CreateDbSetMock(emptyServices);
        _dbContextMock.Setup(m => m.Services).Returns(_servicesDbSetMock.Object);

        var handler = new GetServiceQueryHandler(_dbContextMock.Object);
        var query = new GetServiceQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }
}
