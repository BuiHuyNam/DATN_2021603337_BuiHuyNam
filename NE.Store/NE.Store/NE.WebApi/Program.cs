using Microsoft.EntityFrameworkCore;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Infrastructure.Context;
using NE.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NEContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("DATNConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IProvinceService, ProvinceService>();
builder.Services.AddTransient<IDistrictService, DistrictService>();
builder.Services.AddTransient<IWardService, WardService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IColorService, ColorService>();
//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
