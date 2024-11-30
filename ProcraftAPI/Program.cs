using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Data.Settings;
using ProcraftAPI.Interfaces;
using ProcraftAPI.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("Procraft").Get<ProcraftSettings>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("procraft-policy", builder =>
    {
        builder.WithHeaders()
               .WithOrigins("br.com.procraft", "", "")
               .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH");
    });
});

if (builder.Environment.IsDevelopment())
{
    var connectionString = settings?.GetConnectionString();

    builder.Services.AddDbContext<ProcraftDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    builder.Services.AddScoped<IDbContextFactory<ProcraftDbContext>, ProcraftDbContextFactory>();

    int saltSize = settings?.Salt ?? 0;

    int keySize = settings?.KeySize ?? 0;

    int iterations = settings?.Iterations ?? 0;

    HashAlgorithmName algorithmName = HashAlgorithmName.SHA256;

    char segmentDelimiter = ':';

    builder.Services.AddSingleton<IHashService, HashService>(hashService => new(
        saltSize,
        keySize,
        iterations,
        algorithmName,
        segmentDelimiter
        )
    );

}

if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    builder.Services.AddDbContext<ProcraftDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    int saltSize = int.Parse(Environment.GetEnvironmentVariable("SALT_SIZE") ?? "0");

    int keySize = int.Parse(Environment.GetEnvironmentVariable("KEY_SIZE") ?? "0");

    int iterations = int.Parse(Environment.GetEnvironmentVariable("ITERATIONS") ?? "0");

    HashAlgorithmName algorithmName = HashAlgorithmName.SHA256;

    char segmentDelimiter = ':';

    builder.Services.AddSingleton<IHashService, HashService>(hashService => new(
        saltSize,
        keySize,
        iterations,
        algorithmName,
        segmentDelimiter
        )
    );
}

string port = Environment.GetEnvironmentVariable("PORT") ?? "6000";

string httpsPort = Environment.GetEnvironmentVariable("HTTPS_PORT") ?? "6001";

var secureKey = Environment.GetEnvironmentVariable("SECURE_KEY") ?? "";

var secretServerKey = Encoding.UTF8.GetBytes(secureKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretServerKey),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Procraft",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

string appUrls = builder.Environment.IsDevelopment()
    ? $"http://*:{port};https://*:{httpsPort}"
    : $"http://*:{port}";

builder.WebHost.UseUrls(appUrls);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.MapGet("test", () => $"API NOW RUNNING -- {DateTime.UtcNow} --");

app.UseCors("procraft-policy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
