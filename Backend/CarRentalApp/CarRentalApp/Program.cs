using CarRentalApp.Configuration.JWT.Access;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Data;
using CarRentalApp.Models.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var configurationString = builder.Configuration.GetConnectionString("sqlserver");
builder.Services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(configurationString));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<PasswordService, ShaPasswordService>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<AuthenticationService>();

builder.Services.Configure<AccessJwtConfig>(
    builder.Configuration.GetSection(AccessJwtConfig.Section)
);

var accessJwtConfig = new AccessJwtConfig();
builder.Configuration.Bind(AccessJwtConfig.Section, accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(accessJwtConfig.GenerationParameters.Secret)
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = accessJwtValidationParams;
        }
    );

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
        );
    }
);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();