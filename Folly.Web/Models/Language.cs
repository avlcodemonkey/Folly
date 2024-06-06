namespace Folly.Models;

/// <summary>
/// Represents a language that the application can be translated into. Only used for display purposes, so no model validation needed.
/// </summary>
public sealed record Language(
    int Id,
    string Name = "",
    string LanguageCode = "",
    bool IsDefault = false
) : IAuditedModel;
