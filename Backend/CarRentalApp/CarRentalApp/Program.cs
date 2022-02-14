using CarRentalApp.Configuration;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Contexts;
using Microsoft.EntityFrameworkCore;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Mappers;
using CarRentalApp.Middleware;
using CarRentalApp.Models.DAOs;
using CarRentalApp.Services.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<AccessJwtConfig>(
    builder.Configuration.GetSection(AccessJwtConfig.Section)
);
builder.Services.Configure<RefreshJwtConfig>(
    builder.Configuration.GetSection(RefreshJwtConfig.Section)
);
builder.Services.Configure<ClientRequirements>(
    builder.Configuration.GetSection(ClientRequirements.Section)
);

builder.Services.AddScoped<RefreshTokenDAO>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();

builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddAutoMapper(typeof(UserMapperProfile));

var configurationString = builder.Configuration.GetConnectionString("CarRentalDB");

builder.Services.AddDbContext<CarRentalDbContext>(
    options => options.UseSqlServer(configurationString)
);

var accessJwtConfig = new AccessJwtConfig();
builder.Configuration.Bind(AccessJwtConfig.Section, accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = TokenService.GetKey(accessJwtConfig.GenerationParameters);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = accessJwtValidationParams;
        }
    );

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            configurePolicy =>
            {
                configurePolicy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
        );
    }
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();