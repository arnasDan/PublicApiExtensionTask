using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PublicApiExtension.Storage.Entities;
using System;

namespace PublicApiExtension.Storage
{
    public class SchedulerDatabaseContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public SchedulerDatabaseContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.Tracked += OnEntityTracked;
            ChangeTracker.StateChanged += OnEntityStateChanged;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
            });
        }

        private static void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IEntity entity)
            {
                entity.CreatedAt = DateTime.Now;
                entity.ModifiedAt = DateTime.Now;
            }
        }

        private static void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.NewState == EntityState.Modified && e.Entry.Entity is IEntity entity)
            {
                entity.ModifiedAt = DateTime.Now;
            }
        }
    }
}
