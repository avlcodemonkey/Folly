namespace Folly.Domain.Models;

public class BaseEntity {
    public DateTime CreatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int? UpdatedUserId { get; set; }
}
