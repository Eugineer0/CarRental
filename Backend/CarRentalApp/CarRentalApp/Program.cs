using CarRentalApp.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalApp.Services.Data;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;
using CarRentalApp.Services.Authentication;
using WebApplicationTest.Services.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepositoryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


var accessJwtConfig = new JwtConfig();
builder.Configuration.Bind("AccessJwtConfig", accessJwtConfig);

var accessJwtValidationParams = accessJwtConfig.ValidationParameters;
accessJwtValidationParams.IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(accessJwtConfig.GenerationParameters.Secret)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = accessJwtValidationParams;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
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