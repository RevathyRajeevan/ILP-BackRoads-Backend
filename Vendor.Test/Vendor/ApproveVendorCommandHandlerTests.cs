using Moq;
using Vendor.Application.Requests.Vendor;
using Vendor.Infrastructure.Implementation.Persistence;

    public class ApproveVendorCommandHandlerTests
    {
        private readonly Mock<IVendorDbContext> _dbContextMock;
        private readonly ApproveVendorCommandHandler _handler;

        public ApproveVendorCommandHandlerTests()
        {
            _dbContextMock = new Mock<IVendorDbContext>();
            _handler = new ApproveVendorCommandHandler(_dbContextMock.Object);
        }

    [Fact]
    public async Task Handle_Should_ApproveVendor_When_VendorExists()
    {
        // Arrange
        var vendor = new Vendor.Domain.Entities.Vendor
        {
            Id = 1,
            Name = "Vendor 1",
            IsApproved = false
        };
        _dbContextMock.Setup(db => db.Vendors.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(vendor);
        _dbContextMock.Setup(db => db.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new ApproveVendorCommand { Id = 1 }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsApproved);
        _dbContextMock.Verify(db => db.Vendors.FindAsync(It.Is<object[]>(ids => (int)ids[0] == 1)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_VendorDoesNotExist()
    {
        // Arrange
        _dbContextMock.Setup(db => db.Vendors.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Vendor.Domain.Entities.Vendor)null);

        // Act
        var result = await _handler.Handle(new ApproveVendorCommand { Id = 1 }, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _dbContextMock.Verify(db => db.Vendors.FindAsync(It.Is<object[]>(ids => (int)ids[0] == 1)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Never);
    }

}