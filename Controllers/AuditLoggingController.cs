using Microsoft.AspNetCore.Mvc;

namespace Product_Catalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLoggingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLoggingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var auditLogs = _context.AuditLogs.Select(a => new AuditLogViewModel
            {
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Action = a.Action,
                ChangedBy = a.ChangedBy,
                ChangedAt = a.ChangedAt,
                OldValues = a.OldValues,
                NewValues = a.NewValues
            }).Where(a => a.EntityName == "Product") 
                .ToList();
            return View(auditLogs);
        }
    }
}
