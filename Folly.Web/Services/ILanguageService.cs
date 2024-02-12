using Folly.Models;

namespace Folly.Services;

/// <summary>
/// Fetches languages from the DB.
/// Use AsNoTracking since languages can't be modified.
/// </summary>
public interface ILanguageService {
    Task<IEnumerable<Language>> GetAllLanguagesAsync();

    Task<Language?> GetLanguageByIdAsync(int id);
}
