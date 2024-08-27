using AutoMapper;
using Moq;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

public class GetVendorByIdHandlerTests
{
    private readonly Mock<IVendorDbContext> _dbContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetVendorByIdHandler _handler;

    public GetVendorByIdHandlerTests()
    {
        _dbContextMock = new Mock<IVendorDbContext>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetVendorByIdHandler(_dbContextMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task Handle_Should_ReturnVendorDataDto_When_VendorExist()
    {
        // Arrange
        var vendor = new Vendor.Domain.Entities.Vendor
        {
            Id = 1,
            Name = "Vendor 1",
            Email = "vendor1@example.com",
            ServiceId = 1,
            IsApproved = true,
            VendorMarket = new List<VendorMarket>
            {
                new VendorMarket { Market = new Market { Id = 1, Name = "Market 1" } }
            }
        };

        var vendorDataDto = new VendorDataDto
        {
            Id = 1,
            Name = "Vendor 1",
            Email = "vendor1@example.com",
            Service = new ServiceDto { Id = 1, Name = "Service 1" },
            IsApproved = true,
            Markets = new List<MarketDto>
            {
                new MarketDto { Id = 1, Name = "Market 1" }
            }
        };

        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(new[] { vendor }.AsQueryable());
        _dbContextMock.Setup(db => db.Vendors).Returns(vendorDbSetMock.Object);

        _mapperMock.Setup(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()))
            .Returns(vendorDataDto);

        // Act
        var result = await _handler.Handle(new GetVendorByIdQuery { Id = 1 }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vendorDataDto, result);

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_NoVendorExist()
    {
        // Arrange
        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(new List<Vendor.Domain.Entities.Vendor>().AsQueryable());
        _dbContextMock.Setup(db => db.Vendors).Returns(vendorDbSetMock.Object);

        _mapperMock.Setup(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()))
        .Returns((VendorDataDto)null);

        // Act
        var result = await _handler.Handle(new GetVendorByIdQuery { Id = 2 }, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()), Times.Never);
    }


    [Fact]
    public async Task Handle_Should_HandleMappingException()
    {
        // Arrange
        var vendorId = 1;
        var vendorEntity = new Vendor.Domain.Entities.Vendor
        {
            Id = vendorId,
            Name = "Test Vendor",
            Service = new Service { Id = 1, Name = "Test Service" },
            VendorMarket = new List<VendorMarket>
           {
               new VendorMarket { Market = new Market { Id = 1, Name = "Test Market" } }
           }
        };

        var vendorList = new List<Vendor.Domain.Entities.Vendor> { vendorEntity }.AsQueryable();
        var mockDbSet = DbSetMockUtility.CreateDbSetMock(vendorList);

        _dbContextMock.Setup(db => db.Vendors).Returns(mockDbSet.Object);

        _mapperMock.Setup(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()))
            .Throws(new AutoMapperMappingException("Mapping failed"));

        var query = new GetVendorByIdQuery { Id = vendorId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal("Mapping failed", exception.Message);

        _dbContextMock.Verify(db => db.Vendors, Times.Once);
        _mapperMock.Verify(m => m.Map<VendorDataDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()), Times.Once);
    }
}
