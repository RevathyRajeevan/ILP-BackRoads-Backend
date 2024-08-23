using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;
using Xunit;

public class GetVendorsHandlerTests
{
    private readonly Mock<IVendorDbContext> _dbContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetVendorsHandler _handler;

    public GetVendorsHandlerTests()
    {
        _dbContextMock = new Mock<IVendorDbContext>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetVendorsHandler(_dbContextMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task Handle_Should_ReturnVendors_When_VendorsExist()
    {
        // Arrange
        var vendors = new List<Vendor.Domain.Entities.Vendor>
    {
        new Vendor.Domain.Entities.Vendor
        {
            Id = 1,
            Name = "Vendor 1",
            Email = "vendor2@example.com",
            ServiceId=1,
            IsApproved=true,
            VendorMarket = new List<VendorMarket>
            {
                new VendorMarket { Market = new Market { Id = 1, Name = "Market 1" } }
            }
        },
        new Vendor.Domain.Entities.Vendor
        {
            Id = 2,
            Name = "Vendor 2",
            Email = "vendor2@example.com",
            ServiceId=1,
            IsApproved=true,
            VendorMarket = new List<VendorMarket>
            {
                new VendorMarket { Market = new Market { Id = 2, Name = "Market 2" } }
            }
        }
    }.AsQueryable();

        var vendorDtos = new List<VendorDto>
    {
        new VendorDto { Id = 1, Name = "Vendor 1", Email = "vendor2@example.com", Service= new ServiceDto{Id=1},IsApproved=true, Markets = new List<MarketDto> { new MarketDto { Id = 1, Name = "Market 1" } } },
        new VendorDto { Id = 2, Name = "Vendor 2", Email = "vendor2@example.com",  Service= new ServiceDto{Id=1},IsApproved=true, Markets = new List<MarketDto> { new MarketDto { Id = 2, Name = "Market 2" } } }
    };

        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(vendors);
        _dbContextMock.Setup(db => db.Vendors).Returns(vendorDbSetMock.Object);

        _mapperMock.Setup(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()))
            .Returns(vendorDtos);

        // Act
        var result = await _handler.Handle(new GetVendorsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoVendorsExist()
    {
        // Arrange
        var vendors = new List<Vendor.Domain.Entities.Vendor>().AsQueryable();
        var vendorDtos = new List<VendorDto>();

        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(vendors);
        _dbContextMock.Setup(db => db.Vendors).Returns(vendorDbSetMock.Object);

        _mapperMock.Setup(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()))
            .Returns(vendorDtos);

        // Act
        var result = await _handler.Handle(new GetVendorsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_HandleMappingException()
    {
        // Arrange
        var vendors = new List<Vendor.Domain.Entities.Vendor>
        {
            new Vendor.Domain.Entities.Vendor { Id = 1, Name = "Vendor 1" }
        }.AsQueryable();

        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(vendors);
        _dbContextMock.Setup(db => db.Vendors).Returns(vendorDbSetMock.Object);

        _mapperMock.Setup(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()))
            .Throws(new AutoMapperMappingException("Mapping failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(new GetVendorsQuery(), CancellationToken.None));
        Assert.Equal("Mapping failed", exception.Message);

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<VendorDto>>(It.IsAny<IEnumerable<Vendor.Domain.Entities.Vendor>>()), Times.Once);
    }
}
