using CarRentalBll.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using CarRentalWeb.Middleware;
using CarRentalBll.Configurations;
using CarRentalBll.Services;
using CarRentalDal.Contexts;
using CarRentalWeb.Configurations;
using SharedResources.Configurations.JWT.Access;
using SharedResources.Configurations.JWT.Refresh;

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

var configurationString = builder.Configuration.GetConnectionString("CarRentalDB");

builder.Services.AddDbContext<CarRentalDbContext>(
    options => options.UseSqlServer(configurationString)
);

MapsterWebConfig.Configure();
MapsterBllConfig.Configure();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<OrderService>();

var accessJwtConfig = new AccessJwtConfig();
builder.Configuration.Bind(AccessJwtConfig.Section, accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = TokenService.GetKey(accessJwtConfig.GenerationParameters);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options => { options.TokenValidationParameters = accessJwtValidationParams; }
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