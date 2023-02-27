using System.Collections.Generic;
using System.Threading.Tasks;
using Folly.Models;

namespace Folly.Services;

public interface ILanguageService
{
    Task<IEnumerable<Language>> GetAll();

    Task<Language> GetDefaultLanguage();

    Task<Language> GetLanguageById(int id);
}
