using System.ComponentModel.DataAnnotations;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;

namespace Folly.Models;

/// <summary>
/// Used to group search params and pass them around.
/// </summary>
public sealed record AuditLogSearch(
    [property: Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.Batch))]
    Guid? BatchId,

    [property: Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.Entity))]
    string? Entity,

    [property: Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.PrimaryKey))]
    long? PrimaryKey,

    [property: Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.State))]
    EntityState? State,

    [property: Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.User))]
    int? UserId,

    DateOnly? StartDate,

    DateOnly? EndDate
) {
    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.StartDate))]
    public DateOnly? StartDate { get; set; } = StartDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.EndDate))]
    public DateOnly? EndDate { get; set; } = EndDate ?? DateOnly.FromDateTime(DateTime.Now);
}
