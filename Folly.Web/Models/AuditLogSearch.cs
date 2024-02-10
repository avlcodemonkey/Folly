using System.ComponentModel.DataAnnotations;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;

namespace Folly.Models;

/// <summary>
/// Used to group search params and pass them around.
/// </summary>
public sealed record AuditLogSearch {
    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.Batch))]
    public Guid? BatchId { get; set; }

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.Entity))]
    public string? Entity { get; set; }

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.PrimaryKey))]
    public long? PrimaryKey { get; set; }

    /// <summary>
    /// Should only be one of: Deleted = 2, Modified = 3, Added = 4
    /// </summary>
    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.State))]
    public EntityState? State { get; set; }

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.StartDate))]
    public DateOnly? StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.EndDate))]
    public DateOnly? EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(ResourceType = typeof(AuditLogs), Name = nameof(AuditLogs.User))]
    public int? UserId { get; set; }
}
