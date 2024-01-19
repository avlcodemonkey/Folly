using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class AuditLogService(FollyDbContext dbContext) : IAuditLogService {
    private readonly FollyDbContext _DbContext = dbContext;

    public async Task<IEnumerable<DTO.AuditLog>> GetAllLogsAsync()
        => await _DbContext.AuditLog.AsNoTracking().SelectAsDTO().ToListAsync();

    public async Task<DTO.AuditLog> GetLogByIdAsync(long id)
        => await _DbContext.AuditLog.AsNoTracking().Include(x => x.User).Where(x => x.Id == id).SelectAsDTO().FirstAsync();
}
