using Folly.Models;
using Microsoft.EntityFrameworkCore;

namespace Folly.Services;

public interface IAuditLogService {
    Task<IEnumerable<AuditLogSearchResult>> SearchLogsAsync(AuditLogSearch search);

    Task<AuditLog?> GetLogByIdAsync(long id);

    IEnumerable<EntityState> GetEntityStates();
}
