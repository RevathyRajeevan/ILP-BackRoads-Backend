using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Vendor.Application.Requests.Markets;
using Vendor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Vendor.Infrastructure.Implementation.Persistence;

public class AddMarketCommandHandlerTests
{
    private readonly Mock<IVendorDbContext> _dbContextMock;
    private readonly Mock<DbSet<Market>> _marketsDbSetMock;
    private readonly AddMarketCommandValidator _validator;
    public AddMarketCommandHandlerTests()
    {
        // Set up the DbSet mock
        _marketsDbSetMock = new Mock<DbSet<Market>>();
        _validator = new AddMarketCommandValidator();
        // Set up the DbContext mock with the virtual DbSet property
        _dbContextMock = new Mock<IVendorDbContext>();
        _dbContextMock.Setup(m => m.Markets).Returns(_marketsDbSetMock.Object);
        _dbContextMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_Should_AddMarket_When_NameIsValid()
    {
       
        var handler = new AddMarketCommandHandler(_dbContextMock.Object, _validator);

        var command = new AddMarketCommand
        {
            Name = "New Market"
        };

 
        var result = await handler.Handle(command, CancellationToken.None);

 
        Assert.NotNull(result);
        Assert.Equal("New Market", result.Name);

        _marketsDbSetMock.Verify(m => m.Add(It.IsAny<Market>()), Times.Once);
        _dbContextMock.Verify(m => m.SaveChangesAsync());
    }

    [Fact]
    public async Task Handle_Should_ThrowArgumentException_When_NameIsNull()
    {
        var handler = new AddMarketCommandHandler(_dbContextMock.Object,_validator);
        var command = new AddMarketCommand { Name = null };

        var validationResult = await _validator.ValidateAsync(command);
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Market name is required.");

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));


    }

    [Fact]
    public async Task Handle_Should_ThrowArgumentException_When_NameIsWhitespace()
    {
        
        var handler = new AddMarketCommandHandler(_dbContextMock.Object, _validator);

        var command = new AddMarketCommand
        {
            Name = "    "
        };

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

    }
}
