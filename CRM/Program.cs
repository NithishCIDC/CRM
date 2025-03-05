using CRMuser.Application.Interfaces;
using CRMuser.Application.Service;
using CRMuser.Infrastructure.Data;
using CRMuser.Infrastructure.Repository;
using CRMUser.domain.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS_Policy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

#endregion

#region DbContext
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection"));
});
#endregion

#region JWT Token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Jwt:Issuer"],
           ValidAudience = builder.Configuration["Jwt:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
       };
   });
#endregion

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IEmailService, EmailService>(); 

builder.Services.AddScoped<IOtpService, OtpService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CORS_Policy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
