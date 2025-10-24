using BeGamer.Config;
using BeGamer.Data;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers((options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}));

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDb;Trusted_Connection=True;")); // TODO: pøesunout connection string do appsettings.json
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwJwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("JwtSettings:SecretKey"))
    };
});
builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<GuidGenerator>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameEventService, GameEventService>();

builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<GameEventMapper>();


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger",
        policy =>
        {
            policy.WithOrigins("http://localhost:5254", "http://localhost:7180")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerWithJwt();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapIdentityApi<CustomUser>();
app.UseCors("AllowSwagger");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// seed database with testing initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    AppDbContext.Seed(dbContext);
}
app.Run();


