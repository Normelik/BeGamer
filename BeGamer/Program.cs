using AutoMapper;
using BeGamer.Config;
using BeGamer.Data;
using BeGamer.Mappers;
using BeGamer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<CustomUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Mapper
builder.Services.AddAutoMapper(cf => cf.AddProfile<GenericMapper>());

builder.Services.AddAllServices(Assembly.GetExecutingAssembly());


// Authentication & Authorization
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);

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

// seed database with testing data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    AppDbContext.Seed(dbContext);
}
app.Run();


