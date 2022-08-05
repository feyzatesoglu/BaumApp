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
            builder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => e.Surname);

                entity.HasIndex(e => e.Phone);

                entity.HasIndex(e => e.IdentityId);

                entity.HasIndex(e => e.ImageId);

                entity.HasIndex(e => e.UserTypeEnum);

            });
            builder.Entity<Customer>().HasData(new Customer() { Id = 1, Name = "Ahmet", Surname = "Yalnız", Phone = "8564528", IdentityId=1, ImageId=8, UserTypeEnum= UserType.Intern });
            builder.Entity<Customer>().HasData(new Customer() { Id = 2, Name = "Ali", Surname = "Sucu", Phone = "9564875", IdentityId=2, ImageId=5, UserTypeEnum= UserType.Worker });
            builder.Entity<Customer>().HasData(new Customer() { Id = 3, Name = "Burcu", Surname = "Bilir", Phone = "2536984", IdentityId=3, ImageId=2, UserTypeEnum= UserType.Admin });
            builder.Entity<Customer>().HasData(new Customer() { Id = 4, Name = "Buğlem", Surname = "Yalın", Phone = "2056984", IdentityId=4, ImageId=3, UserTypeEnum= UserType.Intern });
            builder.Entity<Customer>().HasData(new Customer() { Id = 5, Name = "Ekrem", Surname = "Ateş", Phone = "6542514", IdentityId=5, ImageId=1, UserTypeEnum= UserType.Worker });
            builder.Entity<Customer>().HasData(new Customer() { Id = 6, Name = "Ayça", Surname = "Renkli", Phone = "6852574", IdentityId=6, ImageId=4, UserTypeEnum= UserType.Worker });

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

