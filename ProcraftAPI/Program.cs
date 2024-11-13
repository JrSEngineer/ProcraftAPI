using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Data.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("Procraft").Get<ProcraftSettings>();

if (builder.Environment.IsDevelopment())
{
    var connectionString = settings?.GetConnectionString();

    builder.Services.AddDbContext<ProcraftDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    builder.Services.AddScoped<IDbContextFactory<ProcraftDbContext>, ProcraftDbContextFactory>();
}

if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    builder.Services.AddDbContext<ProcraftDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });
}

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
