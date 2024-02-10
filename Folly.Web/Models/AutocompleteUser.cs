namespace Folly.Models;

/// <summary>
/// Used for returning search results for the audit log user autocomplete.
/// </summary>
public sealed record AutocompleteUser {
    public string? Label { get; init; }

    public int Value { get; init; }
}
