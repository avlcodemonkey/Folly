using Folly.Models;

namespace Folly.Services;

public interface ILanguageService {
    Task<IEnumerable<Language>> GetAllLanguagesAsync();

    Task<Language> GetLanguageByIdAsync(int id);
}
