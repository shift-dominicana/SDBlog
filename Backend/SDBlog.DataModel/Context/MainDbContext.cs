using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SDBlog.Core.Base;
using SDBlog.Core.Extensions;
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
                                .Where(type => typeof(IEntityBase).IsAssignableFrom(type.ClrType)))
            {
                modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }

        //DbSet

        //DbSet
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

        //DbSet
        private void AuditEntities()
        {
            // Get current date & time
            DateTime now = DateTime.Now;

            // For every changed entity marked as "IEntidadAuditableBase" set the values for the audit properties
            foreach (EntityEntry<EntityAuditableBase> entry in ChangeTracker.Entries<EntityAuditableBase>())
            {
                // If the entity was added.
                if (entry.State == EntityState.Added)
                {
                    //get entity
                    entry.Entity.CreadoPor = 1;
                    var entidad = entry.Entity.ToString().Replace("SDBlog.DataModel.Entities.", "");
                    entry.Entity.FechaRegistro = now;
                    entry.Entity.Estatus = EntityEstatus.Activo;

                }
                else if (entry.State == EntityState.Modified) // If the entity was updated
                {
                    entry.Entity.ModificadoPor = 1;
                    entry.Entity.FechaModificacion = now;
                    Entry(entry.Entity).Property(x => x.FechaRegistro).IsModified = false;
                    Entry(entry.Entity).Property(x => x.CreadoPor).IsModified = false;
                }
            }
        }

    }
}
