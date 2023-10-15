using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("Language")]
public class Language : BaseEntity {
    [StringLength(10)]
    [Required]
    public string CountryCode { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    public bool IsDefault { get; set; }

    [StringLength(10)]
    [Required]
    public string LanguageCode { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = null!;
}
