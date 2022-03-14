using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using CarRentalWeb.Middleware;
using CarRentalBll.Configurations;
using CarRentalBll.Services;
using CarRentalDal.Contexts;
using CarRentalWeb.Configurations.JWT.Access;
using CarRentalWeb.Configurations.JWT.Refresh;
using CarRentalWeb.Services;
using CarRentalWeb.Validation;
using FluentValidation.AspNetCore;
using SharedResources.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddFluentValidation(
        fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<AdminRegistrationRequestValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<ClientRegistrationRequestValidator>();
        }
    );

builder.Services.Configure<AccessJwtConfig>(
    builder.Configuration.GetSection(AccessJwtConfig.Section)
);
builder.Services.Configure<RefreshJwtConfig>(
    builder.Configuration.GetSection(RefreshJwtConfig.Section)
);
builder.Services.Configure<UserRequirements>(
    builder.Configuration.GetSection(UserRequirements.Section)
);

var configurationString = builder.Configuration.GetConnectionString("CarRentalDB");

builder.Services.AddDbContext<CarRentalDbContext>(
    options => options.UseSqlServer(configurationString)
);

MapsterBllConfig.Configure();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<PasswordService>();

var accessJwtConfig = new AccessJwtConfig();
builder.Configuration.Bind(AccessJwtConfig.Section, accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = JwtService.GetKey(accessJwtConfig.GenerationParameters);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options => options.TokenValidationParameters = accessJwtValidationParams
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