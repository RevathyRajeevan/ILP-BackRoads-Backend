using AutoMapper;
using Moq;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

public class CreateVendorHandlerTests
{
    private readonly Mock<IVendorDbContext> _dbContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateVendorHandler _handler;
    private readonly AddVendorCommandValidator _validator;
    public CreateVendorHandlerTests()
    {

        // Mock DbSet for Vendor and VendorMarket
        _validator = new AddVendorCommandValidator();
        var vendors = new List<Vendor.Domain.Entities.Vendor>().AsQueryable();
        var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(vendors);
        var vendorMarkets = new List<VendorMarket>().AsQueryable();
        var vendorMarketDbSetMock = DbSetMockUtility.CreateDbSetMock(vendorMarkets);

        // Mock the database context
        _dbContextMock = new Mock<IVendorDbContext>();
        _dbContextMock.Setup(c => c.Vendors).Returns(vendorDbSetMock.Object);
        _dbContextMock.Setup(c => c.VendorMarkets).Returns(vendorMarketDbSetMock.Object);

        // Mock AutoMapper
        _mapperMock = new Mock<IMapper>();

        // Initialize the handler with mocks
        _handler = new CreateVendorHandler(_dbContextMock.Object, _mapperMock.Object,_validator);
    }

    [Fact]
    public async Task Handle_Should_AddVendor_When_RequestIsValid()
    {
        // Arrange
        var request = new AddVendorCommand(
            "Vendor Name",
            "City",
            "State",
            "12345",
            "Country",
            "experion@gmail.com",
            "1234567890",
           "https://www.youtube.com/",
            1,
            true,
            new List<int> { 1, 2, 3 }
        );

        var vendor = new Vendor.Domain.Entities.Vendor
        {
            Name = request.Name,
            City = request.City,
            StateProvinceRegion = request.StateProvinceRegion,
            PostalCode = request.PostalCode,
            Country = request.Country,
            Email = request.Email,
            Phone = request.Phone,
            Website = request.Website,
            ServiceId = request.ServiceId,
            IsApproved = request.IsApproved,
            VendorMarket = request.MarketIds.Select(id => new VendorMarket { MarketId = id }).ToList()
        };

        var createOrEditDto = new CreateOrEditDto { Id = 1, Name = request.Name };

        // Setup the mapping for the request to vendor and vendor to CreateDto
        _mapperMock.Setup(m => m.Map<Vendor.Domain.Entities.Vendor>(It.IsAny<AddVendorCommand>())).Returns(vendor);
        _mapperMock.Setup(m => m.Map<CreateOrEditDto>(It.IsAny<Vendor.Domain.Entities.Vendor>())).Returns(createOrEditDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        _dbContextMock.Verify(db => db.Vendors.Add(It.IsAny<Vendor.Domain.Entities.Vendor>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<Vendor.Domain.Entities.Vendor>(It.IsAny<AddVendorCommand>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CreateOrEditDto>(It.IsAny<Vendor.Domain.Entities.Vendor>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_NameIsNull_ValidationFails()
    {

     
        var request = new AddVendorCommand(
            "",
            "City",
            "State",
            "12345",
            "Country",
            "experion@gmail.com",
            "1234567890",
            "https://www.youtube.com/",
            1,
            true,
            new List<int> { 1, 2, 3 }
        );


 
        var validationResult = await _validator.ValidateAsync(request);
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Vendor name is required.");
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));

        _dbContextMock.Verify(db => db.Services.Add(It.IsAny<Service>()), Times.Never);
            _dbContextMock.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);
    }
}