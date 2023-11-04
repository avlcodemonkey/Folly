using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

public class AuditableEntity {
    [NotMapped]
    public long TemporaryId { get; set; }
}
