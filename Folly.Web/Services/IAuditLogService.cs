using Folly.Models;

namespace Folly.Services;

public interface IAuditLogService {
    Task<IEnumerable<AuditLog>> GetAllLogsAsync();

    Task<IEnumerable<AuditLog>> SearchLogsAsync(AuditLogSearch search);

    Task<AuditLog> GetLogByIdAsync(long id);
}
