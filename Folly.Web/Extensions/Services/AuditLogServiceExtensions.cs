using Folly.Domain.Models;
using Folly.Utils;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class AuditLogServiceExtensions {
    public static IQueryable<DTO.AuditLog> SelectAsDTO(this IQueryable<AuditLog> query)
        => query.Select(x => new DTO.AuditLog(x.Id, x.BatchId, x.PrimaryKey, x.State, x.Date,
            (x.User != null ? x.User.LastName : null) ?? "", (x.User != null ? x.User.FirstName : null) ?? "",
            x.Entity, x.OldValues ?? "", x.NewValues ?? ""
        ));

    public static IQueryable<DTO.AuditLogSearchResult> SelectAsSearchResultDTO(this IQueryable<AuditLog> query)
        => query.Select(x => new DTO.AuditLogSearchResult(x.Id, x.BatchId, x.Entity, x.State.ToString(), x.Date.ToString("u"),
            NameHelper.DisplayName(x.User == null ? null : x.User.FirstName, x.User == null ? null : x.User.LastName))
        );
}
