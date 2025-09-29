using Microsoft.EntityFrameworkCore;
using ProductCRUD.Application.Mapping;
using ProductCRUD.Domain.Interfaces;
using ProductCRUD.Infrastructure.Persistence;
using ProductCRUD.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));
builder.Services.AddScoped<IDapperRepository, DapperRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Muestra Swagger en la raíz (http://localhost:5000/)
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenericCrud.Api v1");
        c.RoutePrefix = string.Empty;
    });
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
