namespace Folly.Models;

/// <summary>
/// Represents a language that the application can be translated into. Only used for display purposes, so no model validation needed.
/// </summary>
public sealed record Language : BaseModel {
    public string? LanguageCode { get; init; }

    public string? Name { get; init; }

    public bool IsDefault { get; init; }
}
