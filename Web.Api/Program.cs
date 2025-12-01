using Firmeza.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Firmeza.web.Data.Entity;

var builder = WebApplication.CreateBuilder(args);

// DATABASE + IDENTITY
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT CONFIG
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// AUTHORIZATION POLICIES
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "Administrador"));

    options.AddPolicy("ClienteOnly", policy =>
        policy.RequireRole("Cliente"));
});

// MVC + SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });

    // JWT Bearer estándar
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Token JWT. Solo ingrese el token, sin 'Bearer '"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

builder.Services.AddControllers();

// EMAIL SERVICE
builder.Services.Configure<Firmeza.web.Web.Api.Services.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<Firmeza.web.Web.Api.Services.IEmailService, Firmeza.web.Web.Api.Services.GmailEmailService>();

var app = builder.Build();

// APPLY MIGRATIONS WITH RETRY LOGIC
var maxRetries = 10;
var delay = TimeSpan.FromSeconds(3);

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            Console.WriteLine($"Attempting to apply migrations (attempt {i + 1}/{maxRetries})...");
            dbContext.Database.Migrate();
            Console.WriteLine("✓ Migrations applied successfully!");
            break; // Success, exit retry loop
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠ Error applying migrations (attempt {i + 1}/{maxRetries}): {ex.Message}");
        
        if (i == maxRetries - 1)
        {
            Console.WriteLine($"✗ Failed to apply migrations after {maxRetries} attempts. The application will continue but may not work correctly.");
            break;
        }
        
        Console.WriteLine($"Waiting {delay.TotalSeconds} seconds before retry...");
        Thread.Sleep(delay);
    }
}

// MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseAuthentication();
app.UseAuthorization();

// SEED ROLES (Admin / Cliente) - Run asynchronously to avoid blocking startup
_ = Task.Run(async () =>
{
    var maxRetries = 5;
    var delay = TimeSpan.FromSeconds(2);
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            await Task.Delay(delay * (i + 1)); // Wait longer on each retry
            
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "Administrador", "Cliente" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                        Console.WriteLine($"✓ Role '{role}' created successfully");
                    }
                    else
                    {
                        Console.WriteLine($"✓ Role '{role}' already exists");
                    }
                }
                
                Console.WriteLine("✓ Role seeding completed successfully");
                break; // Success, exit retry loop
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠ Error seeding roles (attempt {i + 1}/{maxRetries}): {ex.Message}");
            if (i == maxRetries - 1)
            {
                Console.WriteLine($"✗ Failed to seed roles after {maxRetries} attempts");
            }
        }
    }
});

// MAP CONTROLLERS
app.MapControllers();

app.Run();
