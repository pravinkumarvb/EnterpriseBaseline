using EnterpriseBaseline.Api.Authorization;
using EnterpriseBaseline.Api.Middleware;
using EnterpriseBaseline.Application.Interfaces.Repositories;
using EnterpriseBaseline.Application.Interfaces.Services;
using EnterpriseBaseline.Application.Services;
using EnterpriseBaseline.Domain.Entities;
using EnterpriseBaseline.Infrastructure.Identity;
using EnterpriseBaseline.Infrastructure.Persistence;
using EnterpriseBaseline.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Users.View", policy =>
        policy.Requirements.Add(new PermissionRequirement("Users.View")));

    options.AddPolicy("Users.Create", policy =>
        policy.Requirements.Add(new PermissionRequirement("Users.Create")));

    options.AddPolicy("Users.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Users.Update")));

    options.AddPolicy("Users.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Users.Delete")));

    options.AddPolicy("Departments.View", policy =>
       policy.Requirements.Add(new PermissionRequirement("Departments.View")));

    options.AddPolicy("Departments.Create", policy =>
        policy.Requirements.Add(new PermissionRequirement("Departments.Create")));

    options.AddPolicy("Departments.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Departments.Update")));

    options.AddPolicy("Departments.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Departments.Delete")));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EnterpriseBaseline API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


// Identity / Security
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    if (!db.Users.Any())
    {
        var adminUser = new User
        {
            UserName = "admin",
            Email = "admin@enterprise.local",
            IsActive = true,
            PasswordHash = hasher.HashPassword("Admin@123")
        };

        db.Users.Add(adminUser);
        db.SaveChanges();
    }

    if (!db.Roles.Any())
    {
        var adminRole = new Role
        {
            Name = "Admin"
        };

        db.Roles.Add(adminRole);

        db.Permissions.AddRange(
            new Permission { Code = "Departments.View", Description = "View departments" },
            new Permission { Code = "Departments.Create", Description = "Create departments" },
            new Permission { Code = "Departments.Update", Description = "Update departments" },
            new Permission { Code = "Departments.Delete", Description = "Delete departments" }
        );

        db.SaveChanges();

        var permissions = db.Permissions.ToList();

        foreach (var permission in permissions)
        {
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = permission.Id
            });
        }

        var adminUser = db.Users.First();
        db.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
