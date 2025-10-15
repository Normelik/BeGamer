namespace BeGamer.Data;

using BeGamer.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<GameEvent> GameEvents { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<GameEvent>()
                    .WithMany()
                    .HasForeignKey("GameEventId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("GameEventId", "UserId");
                });
    }
}