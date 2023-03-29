using System.Runtime.Serialization;
using Folly.Utils;

namespace Folly.Models;

public record BaseModel {
    public BaseModel() { }

    [IgnoreDataMember]
    public string FormAction => IsCreate ? "Create" : "Edit";

    [IgnoreDataMember]
    public HttpVerb FormMethod => IsCreate ? HttpVerb.Post : HttpVerb.Put;

    public int Id { get; init; }

    [IgnoreDataMember]
    public bool IsCreate => Id == 0;
}
