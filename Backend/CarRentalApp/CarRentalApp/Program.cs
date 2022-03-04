using CarRentalApp.Configuration;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarRentalApp.Services.Token;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Contexts;
using Microsoft.EntityFrameworkCore;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Configuration.Mappers;
using CarRentalApp.Middleware;
using CarRentalApp.Services;
using MapsterMapper;

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

var mapper = new Mapper(MapsterConfig.GetConfig());
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<RentalCenterService>();
builder.Services.AddScoped<PasswordService>();

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