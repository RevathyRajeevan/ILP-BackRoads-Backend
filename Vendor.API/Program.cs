using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.AutoMapper;
using Vendor.Application.Requests.Vendor;
using Vendor.Infrastructure.Implementation.Persistence;

var builder = WebApplication.CreateBuilder(args);


var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOnlyRequiredOrigins", policyBuilder =>
    {
        policyBuilder.WithOrigins(allowedOrigins)
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddDbContext<VendorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddScoped<IVendorDbContext, VendorDbContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetVendorsQuery).Assembly));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddVendorCommandValidator>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors("AllowOnlyRequiredOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
