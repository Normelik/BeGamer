using BeGamer.Config;
using BeGamer.Data;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers((options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<CustomUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<GuidGenerator>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameEventService, GameEventService>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<GameMapper>();
builder.Services.AddScoped<GameEventMapper>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<UserManager<CustomUser>>();
builder.Services.AddScoped<PasswordHasher<CustomUser>>();


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
app.UseCors("AllowSwagger");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// seed database with testingdata
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    AppDbContext.Seed(dbContext);
}
app.Run();


