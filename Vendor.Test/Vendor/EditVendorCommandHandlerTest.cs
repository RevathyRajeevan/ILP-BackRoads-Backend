using AutoMapper;
using Moq;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;
using FluentValidation;

namespace Vendor.Application.Tests
{
    public class EditVendorHandlerTests
    {
        private readonly Mock<IVendorDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EditVendorHandler _handler;
        private readonly EditVendorCommandValidator _validator;

        public EditVendorHandlerTests()
        {
            _validator = new EditVendorCommandValidator();
            var vendors = new List<Vendor.Domain.Entities.Vendor>().AsQueryable();
            var vendorDbSetMock = DbSetMockUtility.CreateDbSetMock(vendors);
            var vendorMarkets = new List<VendorMarket>().AsQueryable();
            var vendorMarketDbSetMock = DbSetMockUtility.CreateDbSetMock(vendorMarkets);

            _dbContextMock = new Mock<IVendorDbContext>();
            _dbContextMock.Setup(c => c.Vendors).Returns(vendorDbSetMock.Object);
            _dbContextMock.Setup(c => c.VendorMarkets).Returns(vendorMarketDbSetMock.Object);

            _mapperMock = new Mock<IMapper>();

            _handler = new EditVendorHandler(_dbContextMock.Object, _mapperMock.Object, _validator);
        }



        [Fact]
        public async Task Handle_Should_ThrowException_When_VendorNotFound()
        {
            // Arrange
            var vendorId = 999; // Non-existent ID
            var request = new EditVendorCommand
            {
                Name = "Updated Vendor Name",
                City = "Updated City",
                StateProvinceRegion = "Updated State",
                PostalCode = "54321",
                Country = "Updated Country",
                Email = "updated@example.com",
                Phone = "9876543210",
                Website = "https://www.updated-example.com/",
                ServiceId = 2,
                IsApproved = false,
                MarketIds = new List<int> { 1,2,3 }
            };

            var createDto = new CreateDto { Id = 1, Name = request.Name };

            _mapperMock.Setup(m => m.Map<CreateDto>(It.IsAny<Vendor.Domain.Entities.Vendor>())).Returns(createDto);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(new EditVendorCommandWithId(vendorId, request), CancellationToken.None));
            _dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_ValidationFails()
        {
            // Arrange
            var vendorId = 1;
            var request = new EditVendorCommand
            {
                Name = "Vendor 1", 
                City = "Updated City",
                StateProvinceRegion = "Updated State",
                PostalCode = "54321",
                Country = "Updated Country",
                Email = "updated@example.com",
                Phone = "",
                Website = "https://www.updated-example.com/",
                ServiceId = 2,
                IsApproved = false,
                MarketIds = new List<int> { 1, 2 }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(new EditVendorCommandWithId(vendorId, request), CancellationToken.None));
            _dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Never);
        }
    }
}
