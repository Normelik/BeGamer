using BeGamer.Config;
using BeGamer.Data;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDb;Trusted_Connection=True;")); // TODO: pøesunout connection string do appsettings.json
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameEventService, GameEventService>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<GameEventMapper>();

// Pøidej CORS službu
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

builder.Services.AddJwtAuthentication(builder.Configuration);

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

// vložení testovacích dat do databáze
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    AppDbContext.Seed(dbContext);
}
app.Run();


