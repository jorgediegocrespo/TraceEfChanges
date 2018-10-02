using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraceEFChanges.DataAccess.EntityConfig;
using TraceEFChanges.DataAccess.Model;

namespace TraceEFChanges.DataAccess
{
    public class TraceContext : DbContext
    {
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        public TraceContext(DbContextOptions options) : base(options) { }

        public TraceContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PersonEntityConfig.SetEntityBuilder(modelBuilder.Entity<PersonEntity>());
            LogEntityConfig.SetEntityBuilder(modelBuilder.Entity<LogEntity>());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("YOUR CONNECTION STRING");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ProcessLog();
            return await base.SaveChangesAsync(cancellationToken);
        }


        public override int SaveChanges()
        {
            ProcessLog();
            return base.SaveChanges();
        }

        private void ProcessLog()
        {
            var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.UtcNow;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.Properties)
                {
                    var originalValue = change.OriginalValues[prop.Name].ToString();
                    var currentValue = change.CurrentValues[prop.Name].ToString();
                    if (originalValue != currentValue)
                    {
                        LogEntity log = new LogEntity()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        Logs.Add(log);
                    }
                }
            }
        }

        private object GetPrimaryKeyValue(EntityEntry entry)
        {
            var keyName = this.Model.FindEntityType(entry.Entity.GetType()).FindPrimaryKey().Properties.Select(x => x.Name).Single();
            return entry.CurrentValues[keyName];
        }
    }
}
