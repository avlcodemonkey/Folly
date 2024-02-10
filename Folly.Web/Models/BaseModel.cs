using System.Runtime.Serialization;

namespace Folly.Models;

public record BaseModel {
    public BaseModel() { }

    public int Id { get; init; }

    [IgnoreDataMember]
    public bool IsCreate => Id == 0;
}
