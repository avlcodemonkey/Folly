using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class AuditLogService(FollyDbContext dbContext) : IAuditLogService {
    private readonly FollyDbContext _DbContext = dbContext;

    public static int MaxResults { get; } = 1000;

    public async Task<IEnumerable<DTO.AuditLog>> SearchLogsAsync(DTO.AuditLogSearch search) {
        var query = _DbContext.AuditLog.AsNoTracking();

        if (search.StartDate.HasValue) {
            query = query.Where(x => x.Date >= search.StartDate.Value.ToDateTime(new TimeOnly(0)));
        }

        if (search.EndDate.HasValue) {
            query = query.Where(x => x.Date <= search.EndDate.Value.ToDateTime(new TimeOnly(23, 59, 59)));
        }

        if (search.BatchId.HasValue) {
            query = query.Where(x => x.BatchId == search.BatchId);
        }

        if (!string.IsNullOrWhiteSpace(search.Entity)) {
            query = query.Where(x => x.Entity == search.Entity);
        }

        if (search.PrimaryKey.HasValue) {
            query = query.Where(x => x.PrimaryKey == search.PrimaryKey);
        }

        if (search.State.HasValue) {
            query = query.Where(x => x.State == search.State);
        }

        if (search.UserId.HasValue) {
            query = query.Where(x => x.UserId == search.UserId);
        }

        query = query.Take(MaxResults + 1);

        return await query.SelectAsDTO().ToListAsync();
    }

    public async Task<DTO.AuditLog> GetLogByIdAsync(long id)
        => await _DbContext.AuditLog.AsNoTracking().Include(x => x.User).Where(x => x.Id == id).SelectAsDTO().FirstAsync();
}
