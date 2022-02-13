using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SDBlog.Core.Base;
using SDBlog.Core.Enums;
using SDBlog.Core.Extensions;
using SDBlog.DataModel.Entities.Categories;
using SDBlog.DataModel.Entities.Posts;
using SDBlog.DataModel.Entities.Tags;
using SDBlog.DataModel.Entities.Users;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SDBlog.DataModel.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> context) : base(context)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            /*Through reflection, all classes that implement IEntityTypeConfiguration
             are scan and register each one automatically*/
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // NOTE: A query filter is applied to all the entities that inherit from 
            // IEntidadAuditableBase with the objective that when a result of the 
            // database is obtained, it automatically ignores the records that have 
            // the true value in "Borrado" field.
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                                .Where(type => typeof(EntityBase).IsAssignableFrom(type.ClrType)))
            {
                modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }

        public override int SaveChanges()
        {

            this.AuditEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChanges so we can call the new AuditEntities method.
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.AuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AuditEntities()
        {
            // Get current date & time
            DateTime now = DateTime.Now;

            // For every changed entity marked as "IEntidadAuditableBase" set the values for the audit properties
            foreach (EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
            {
                // If the entity was added.
                if (entry.State == EntityState.Added)
                {
                    //get entity
                    entry.Entity.CreatedBy = 1;
                    var entidad = entry.Entity.ToString().Replace("SDBlog.DataModel.Entities.", "");
                    entry.Entity.CreatedDate = now;
                    entry.Entity.Status = EntityEstatus.Active;

                }
                else if (entry.State == EntityState.Modified) // If the entity was updated
                {
                    entry.Entity.ModifiedBy = 1;
                    entry.Entity.ModifiedDate = now;
                    Entry(entry.Entity).Property(x => x.CreatedDate).IsModified = false;
                    Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}
