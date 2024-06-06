namespace Folly.Models;

/// <summary>
/// Used for returning search results for the audit log user autocomplete.
/// </summary>
public sealed record AutocompleteUser(
    int Value,
    string Label = ""
);
