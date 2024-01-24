using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class AuditLogServiceExtensions {
    public static IQueryable<DTO.AuditLog> SelectAsDTO(this IQueryable<AuditLog> query)
        => query.Select(x => new DTO.AuditLog {
            Id = x.Id, BatchId = x.BatchId, Entity = x.Entity, PrimaryKey = x.PrimaryKey,
            State = x.State, Date = x.Date, UserId = x.UserId, OldValues = x.OldValues, NewValues = x.NewValues,
            UserLastName = x.User != null ? x.User.LastName : null,
            UserFirstName = x.User != null ? x.User.FirstName : null
        });
}
