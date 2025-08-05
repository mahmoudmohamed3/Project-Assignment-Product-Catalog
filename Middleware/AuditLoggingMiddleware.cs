using Newtonsoft.Json;

namespace Product_Catalog.Middleware
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationDbContext _context;
        public AuditLoggingMiddleware(RequestDelegate next, ApplicationDbContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originEntries = _context.ChangeTracker.Entries().ToList();

            await _next(context);

            var changedEntries = _context.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                && e.Entity.GetType().Name == "Product");

            foreach (var entry in changedEntries)
            {
                string action = entry.State.ToString();

                string? oldValues = entry.State == EntityState.Modified ||
                                    entry.State == EntityState.Deleted
                    ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                    : null;

                string? newValues = entry.State == EntityState.Added || 
                                    entry.State == EntityState.Modified
                ? JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                : null;

                var auditLog = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = (entry.Entity as Product)?.ProductId.ToString()?? "UnKnown",
                    Action = action,
                    ChangedBy = context.User?.Identity?.Name ?? "Unknown",
                    ChangedAt = DateTime.UtcNow,
                    OldValues = oldValues,
                    NewValues = newValues
                };

                _context.AuditLogs.Add(auditLog);

            }

            await _context.SaveChangesAsync();

        }

    }
}
