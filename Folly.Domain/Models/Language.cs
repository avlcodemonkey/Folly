using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folly.Domain.Attributes;

namespace Folly.Domain.Models;

[Table("Language")]
public class Language : IAuditedEntity {
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    [Required]
    public string CountryCode { get; set; } = null!;

    public bool IsDefault { get; set; }

    [StringLength(10)]
    [Required]
    public string LanguageCode { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime CreatedDate { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime UpdatedDate { get; set; }

    [NotMapped]
    public int TemporaryId { get; set; }
}
