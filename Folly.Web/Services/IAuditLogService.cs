using Folly.Models;

namespace Folly.Services;

public interface IAuditLogService {
    Task<IEnumerable<AuditLog>> GetAllLogsAsync();

    Task<AuditLog> GetLogByIdAsync(long id);
}
