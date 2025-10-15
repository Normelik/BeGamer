using BeGamer.Config;
using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDb;Trusted_Connection=True;"));
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


    