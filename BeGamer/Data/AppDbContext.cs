namespace BeGamer.Data;

using BeGamer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : IdentityDbContext<CustomUser>
{
    public DbSet<Game> Games { get; set; }
    public DbSet<GameEvent> GameEvents { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // 1:N Organizer - GameEvents
        modelBuilder.Entity<GameEvent>()
            .HasOne(e => e.Organizer)
            .WithMany(u => u.OrganizedEvents)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);  // aby smazání uživatele nesmazalo eventy

        // 1:N Game - GameEvents
        modelBuilder.Entity<GameEvent>()
            .HasOne(e => e.Game)
            .WithMany(g => g.Events)
            .HasForeignKey(e => e.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        // N:N GameEvent - Participants (Users)
        modelBuilder.Entity<GameEvent>()
            .HasMany(e => e.Participants)
            .WithMany(u => u.EventsToAttend)
            .UsingEntity<Dictionary<string, object>>(  // implicitní spojovací tabulka
                "GameEventParticipant",
                j => j
                    .HasOne<CustomUser>()
                    .WithMany()
                    .HasForeignKey("Id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<GameEvent>()
                    .WithMany()
                    .HasForeignKey("GameEventId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("GameEventId", "Id");
                });
    }

    public static void Seed(AppDbContext context)
    {
        // Ověření, zda už jsou data přítomná
        if (!context.Games.Any())
        {
            context.Games.AddRange(
                new Game
                {
                    Id = Guid.NewGuid(),
                    Title = "Catan",
                    MinPlayers = 3,
                    MaxPlayers = 4,
                    Type = Enums.BoardGameType.Strategy
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Title = "Dixit",
                    MinPlayers = 3,
                    MaxPlayers = 6,
                    Type = Enums.BoardGameType.Party
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Title = "Pandemic",
                    MinPlayers = 2,
                    MaxPlayers = 4,
                    Type = Enums.BoardGameType.Cooperative
                }
            );
        }
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new CustomUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "john_doe",
                    PasswordHash = "password123",
                    Nickname = "Johnny"
                },
                new CustomUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "jane_smith",
                    PasswordHash = "securepass",
                    Nickname = "Janie"
                }
            );
        }
        context.SaveChanges();
    }
}



