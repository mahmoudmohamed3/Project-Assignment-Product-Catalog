using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Product_Catalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        private IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added 
                || e.State == EntityState.Modified 
                || e.State == EntityState.Deleted)
                .ToList();

            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Unknown";

            foreach (var entry in changedEntries)
            {
                var auditLog = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = GetPrimaryKeyValue(entry).ToString(),
                    Action = entry.State.ToString(),
                    ChangedBy = userName,
                    ChangedAt = DateTime.UtcNow,
                    OldValues = entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                        ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                        : null,
                    NewValues = entry.State == EntityState.Added || entry.State == EntityState.Modified
                        ? JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                        : null
                };

                AuditLogs.Add(auditLog);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private string GetPrimaryKeyValue(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null || key.Properties.Count == 0)
            {
                return "Unknown";
            }

            var keyProperty = key.Properties[0];
            var keyValue = entry.Property(keyProperty.Name).CurrentValue;
            return keyValue?.ToString() ?? "Unknown";
        }

    }
}
