using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Infrastructure;
using SchoolManagementSystem.Infrastructure.Data;
using SchoolManagementSystem.Infrastructure.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management System", Version = "v1" });

    // JWT support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Infrastructure (Repositories, DbContext, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Bind JwtSetting and add JwtTokenGenerator
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSetting>>().Value);
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// Configure Authentication with JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSetting>();
if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
    throw new Exception("JWT settings are missing in appsettings.json");

var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

// Configure Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrTeacher", policy => policy.RequireRole("Admin", "Teacher"));
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await ApplicationDbContextSeed.SeedAsync(context, services);
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Order matters**: Authentication first, then Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
