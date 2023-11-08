using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folly.Domain.Attributes;

namespace Folly.Domain.Models;

public class AuditableEntity {
    [Key]
    public int Id { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime CreatedDate { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime UpdatedDate { get; set; }

    [NotMapped]
    public int TemporaryId { get; set; }
}
