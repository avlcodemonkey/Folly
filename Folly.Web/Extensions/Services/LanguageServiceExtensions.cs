using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class LanguageServiceExtensions {
    public static IQueryable<DTO.Language> SelectAsDTO(this IQueryable<Language> query)
        => query.Select(x => new DTO.Language(x.Id, x.Name, x.LanguageCode, x.IsDefault));
}
