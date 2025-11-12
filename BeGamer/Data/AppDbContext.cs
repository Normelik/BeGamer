namespace BeGamer.Data;

using BeGamer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : IdentityDbContext<CustomUser, IdentityRole<Guid>,Guid>
{
    public DbSet<Game> Game { get; set; }
    public DbSet<GameEvent> GameEvents { get; set; }
    public DbSet<Address> Addresses { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nastavení unikátního indexu na Username
        modelBuilder.Entity<CustomUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // 1:N Organizer - GameEvents
        modelBuilder.Entity<GameEvent>()
            .HasOne(e => e.Organizer)
            .WithMany(u => u.OrganizedEvents)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade);


        // 1:N Game - GameEvents
        modelBuilder.Entity<GameEvent>()
            .HasOne(e => e.Game)
            .WithMany(g => g.Events)
            .HasForeignKey(e => e.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        // N:N GameEvent - Participants (Users)
        modelBuilder.Entity<GameEvent>()
            .HasMany(e => e.Participants)
            .WithMany(u => u.EventsToAttend)
            .UsingEntity<Dictionary<string, object>>(
                "GameEventParticipant",
                j => j
                    .HasOne<CustomUser>()
                    .WithMany()
                    .HasForeignKey("Id"),
                j => j
                    .HasOne<GameEvent>()
                    .WithMany()
                    .HasForeignKey("GameEventId")
                    .OnDelete(DeleteBehavior.Restrict));
    }

    public static void Seed(AppDbContext context)
    {
        PasswordHasher<CustomUser> passwordHasher = new PasswordHasher<CustomUser>();
        var hashedPassword1 = passwordHasher.HashPassword(null, "password123");
        var hashedPassword2 = passwordHasher.HashPassword(null, "securepass");
        
        if (!context.Game.Any())
        {
            context.Game.AddRange(
                new Game
                {
                    Id = Guid.Parse("792ad432-0dbc-4b89-a19f-adbe77ce5701"),
                    Title = "Catan",
                    MinPlayers = 3,
                    MaxPlayers = 4,
                    Type = Enums.BoardGameType.Strategy
                },
                new Game
                {
                    Id = Guid.Parse("bc1d1e13-63cf-45d7-b9d6-b6e0377dce3c"),
                    Title = "Dixit",
                    MinPlayers = 3,
                    MaxPlayers = 6,
                    Type = Enums.BoardGameType.Party
                },
                new Game
                {
                    Id = Guid.Parse("1e7449e8-c4d4-43f1-9305-14c5fa95820c"),
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
                    Id = Guid.Parse("47b41a8f-ac72-478f-a6cc-a80919aad117"),
                    UserName = "john_doe",
                    PasswordHash = hashedPassword1,
                    Nickname = "Johnny"
                },
                new CustomUser
                {
                    Id = Guid.Parse("1a482a4e-3395-4269-8766-edeab8eb65f7"),
                    UserName = "jane_smith",
                    PasswordHash = hashedPassword2,
                    Nickname = "Janie"
                }
            );
        }
        
        if (!context.Addresses.Any())
        {
            context.Addresses.AddRange(
                new Address
                {
                    Id = Guid.Parse("6346917e-7783-4a44-ae55-ddc743364ed7"),
                    Street = "Main St 123",
                    City = "Springfield",
                    PostalCode = "12345",
                    Country = "USA",
                    Latitude = 39.7817,
                    Longitude = -89.6501
                },
                new Address
                {
                    Id = Guid.Parse("7f068584-3b05-4989-b3a9-a075a4749122"),
                    Street = "Second St 456",
                    City = "Shelbyville",
                    PostalCode = "67890",
                    Country = "USA",
                    Latitude = 39.4061,
                    Longitude = -88.8454
                }
            );
        }
        context.SaveChanges();
    }
}



