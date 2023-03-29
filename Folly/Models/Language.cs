namespace Folly.Models;

/// <summary>
/// Represents a language that the application can be translated into. Only used for display purposes, so no model validation needed.
/// </summary>
public sealed record Language : BaseModel {
    public string LanguageCode { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsDefault { get; init; }
}
