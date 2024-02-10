using Folly.Domain.Models;
using Folly.Utils;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class AuditLogServiceExtensions {
    public static IQueryable<DTO.AuditLog> SelectAsDTO(this IQueryable<AuditLog> query)
        => query.Select(x => new DTO.AuditLog {
            Id = x.Id, BatchId = x.BatchId, Entity = x.Entity, PrimaryKey = x.PrimaryKey,
            State = x.State, Date = x.Date, OldValues = x.OldValues ?? "", NewValues = x.NewValues ?? "",
            UserLastName = x.User != null ? x.User.LastName : null,
            UserFirstName = (x.User != null ? x.User.FirstName : null) ?? ""
        });

    public static IQueryable<DTO.AuditLogSearchResult> SelectAsSearchResultDTO(this IQueryable<AuditLog> query)
        => query.Select(x => new DTO.AuditLogSearchResult {
            Id = x.Id, BatchId = x.BatchId, Entity = x.Entity, State = x.State.ToString(),
            UniversalDate = x.Date.ToString("u"),
            UserFullName = NameHelper.DisplayName(x.User == null ? null : x.User.FirstName, x.User == null ? null : x.User.LastName)
        });
}
