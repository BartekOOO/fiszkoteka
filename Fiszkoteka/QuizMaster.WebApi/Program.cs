using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizMaster.Application.Services;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Infrastructure.Data;
using QuizMaster.Infrastructure.Services;
using QuizMaster.WebApi.Middlewares;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<QuizMasterDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var db = context.HttpContext.RequestServices
                    .GetRequiredService<QuizMasterDbContext>();

                var jti = context.Principal?
                    .Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)
                    ?.Value;

                if (string.IsNullOrWhiteSpace(jti))
                {
                    context.Fail("Token nie zawiera JTI.");
                    return;
                }

                var revoked = await db.RevokedTokens
                    .AnyAsync(x => x.Jti == jti);

                if (revoked)
                {
                    context.Fail("Token został unieważniony.");
                }
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IFlashcardSetService, FlashcardSetService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
