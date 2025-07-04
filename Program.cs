using DPAS.Api.Context;
using DPAS.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BaseDbContext>(options =>
{
    var sqlConnectionString = builder.Configuration.GetConnectionString("Sql") ?? throw new InvalidOperationException("SQL is not configured.");
    options.UseSqlServer(sqlConnectionString);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis is not configured.");
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

builder.Services.AddSingleton<IDatabase>(provider =>
{
    var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
    return multiplexer.GetDatabase();
});

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
