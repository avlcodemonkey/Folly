namespace Folly.Models;

public record VersionedModel : BaseModel {
    public VersionedModel() { }

    public int RowVersion { get; init; }
}
