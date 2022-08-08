using BaumKantin.Core;
using BaumKantin.Core.Models;
using BaumKantin.Core.UnitOfWorks;
using BaumKantin.Repository.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BaumKantin.Repository
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public readonly IUnitOfWork _UnitOfWork;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerConfiguraitons());
            builder.ApplyConfiguration(new RoomConfigurations());
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Room>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.HasIndex(e => e.Number);
                entity.HasIndex(e => e.Floor);

            });
            builder.Entity<Room>().HasData(new Room() { Id = 1, Number = "206", Floor = "1" });
            builder.Entity<Room>().HasData(new Room() { Id = 2, Number = "Seminer Odası", Floor = "2" });
            builder.Entity<Room>().HasData(new Room() { Id = 3, Number = "208", Floor = "1" });
            builder.Entity<Room>().HasData(new Room() { Id = 4, Number = "207", Floor = "1" });
            builder.Entity<Room>().HasData(new Room() { Id = 5, Number = "209", Floor = "1" });
            builder.Entity<Room>().HasData(new Room() { Id = 6, Number = "306", Floor = "2" });

            builder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => e.Surname);

                entity.HasIndex(e => e.Phone);

                entity.HasIndex(e => e.IdentityId);

                entity.HasIndex(e => e.ImageId);

                entity.HasIndex(e => e.RoomId);

                entity.HasIndex(e => e.UserTypeEnum);

            });
            builder.Entity<Customer>().HasData(new Customer() { Id = 1, Name = "Harun", Surname = "Bozacı", Phone = "8564528", IdentityId = 1, RoomId = 1, ImageId = 8, UserTypeEnum = UserType.Intern });
            builder.Entity<Customer>().HasData(new Customer() { Id = 2, Name = "Sude", Surname = "Akkaya", Phone = "9564875", IdentityId = 2, RoomId = 2, ImageId = 5, UserTypeEnum = UserType.Intern });
            builder.Entity<Customer>().HasData(new Customer() { Id = 3, Name = "Feyza", Surname = "Ateşoğlu", Phone = "2536984", IdentityId = 3, RoomId = 3, ImageId = 2, UserTypeEnum = UserType.Intern });
            builder.Entity<Customer>().HasData(new Customer() { Id = 4, Name = "Emre", Surname = "Işın", Phone = "2056984", IdentityId = 4, RoomId = 1, ImageId = 3, UserTypeEnum = UserType.Admin });
            builder.Entity<Customer>().HasData(new Customer() { Id = 5, Name = "Ekrem", Surname = "Ateş", Phone = "6542514", IdentityId = 5, RoomId = 2, ImageId = 1, UserTypeEnum = UserType.Worker });
            builder.Entity<Customer>().HasData(new Customer() { Id = 6, Name = "Ayça", Surname = "Renkli", Phone = "6852574", IdentityId = 6, RoomId = 3, ImageId = 4, UserTypeEnum = UserType.Worker });
            builder.Entity<Customer>().HasData(new Customer() { Id = 7, Name = "Ahmet", Surname = "Aydın", Phone = "6852574", IdentityId = 7, RoomId = 4, ImageId = 12, UserTypeEnum = UserType.Worker });
            builder.Entity<Customer>().HasData(new Customer() { Id = 8, Name = "Kerem", Surname = "Yıldırım", Phone = "6852574", IdentityId = 8, RoomId = 5, ImageId = 14, UserTypeEnum = UserType.Worker });
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreateDate = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var entityEntry in entries)
            {
                var auditableEntity = entityEntry as BaseEntity;

                if (auditableEntity != null)
                {
                    auditableEntity.CreateDate = DateTime.Now;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as BaseEntity;
                if (auditableEntity != null)
                {
                    auditableEntity.UpdateDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync();
        }
    }
}

