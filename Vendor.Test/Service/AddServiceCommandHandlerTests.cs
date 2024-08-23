using Moq;
using Vendor.Application.Requests.Services;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Tests.Requests.Services
{
    public class AddServiceCommandHandlerTests
    {
        private readonly Mock<IVendorDbContext> _mockDbContext;
        private readonly AddServiceCommandHandler _handler;

        public AddServiceCommandHandlerTests()
        {
      
            _mockDbContext = new Mock<IVendorDbContext>();
            _handler = new AddServiceCommandHandler(_mockDbContext.Object);
        }

        [Fact]
        public async Task Handle_ValidServiceName_ShouldCreateAndReturnService()
        {
          
            var request = new AddServiceCommand { Name = "Test Service" };
            var cancellationToken = new CancellationToken();

            _mockDbContext.Setup(db => db.Services.Add(It.IsAny<Service>())).Verifiable();
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);
            var result = await _handler.Handle(request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            _mockDbContext.Verify(db => db.Services.Add(It.Is<Service>(s => s.Name == request.Name)), Times.Once);
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_EmptyServiceName_ShouldThrowArgumentException()
        {
            
            var request = new AddServiceCommand { Name = "" };
            var cancellationToken = new CancellationToken();


            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, cancellationToken));

            Assert.Equal("Service name can't be null (Parameter 'Name')", exception.Message);
            _mockDbContext.Verify(db => db.Services.Add(It.IsAny<Service>()), Times.Never);
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_NullServiceName_ShouldThrowArgumentException()
        {

            var request = new AddServiceCommand { Name = null };
            var cancellationToken = new CancellationToken();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, cancellationToken));

            Assert.Equal("Service name can't be null (Parameter 'Name')", exception.Message);
            _mockDbContext.Verify(db => db.Services.Add(It.IsAny<Service>()), Times.Never);
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);
        }
    }
}
