using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class LanguageServiceExtensions {
    public static IQueryable<DTO.Language> SelectDTO(this IQueryable<Language> query)
        => query.Select(x => new DTO.Language { Id = x.Id, Name = x.Name, IsDefault = x.IsDefault, LanguageCode = x.LanguageCode });
}
