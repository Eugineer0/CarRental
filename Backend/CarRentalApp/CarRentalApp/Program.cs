using CarRentalApp.Configuration.JWT.Access;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Data;
using CarRentalApp.Contexts;
using Microsoft.EntityFrameworkCore;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Services.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<AccessJwtConfig>(
    builder.Configuration.GetSection(AccessJwtConfig.Section)
);
builder.Services.Configure<RefreshJwtConfig>(
    builder.Configuration.GetSection(RefreshJwtConfig.Section)
);

builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<PasswordService, ShaPasswordService>();

builder.Services.AddAutoMapper(typeof(UserMapperProfile));

var configurationString = builder.Configuration.GetConnectionString("sqlserver");
builder.Services.AddDbContext<AuthenticationDbContext>(
    options => options.UseSqlServer(configurationString)
);

var accessJwtConfig = new AccessJwtConfig();
builder.Configuration.Bind(AccessJwtConfig.Section, accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(accessJwtConfig.GenerationParameters.Secret)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = accessJwtValidationParams;
    }
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        configurePolicy =>
        {
            configurePolicy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();