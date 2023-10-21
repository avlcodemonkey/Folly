using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class LanguageService : ILanguageService {
    private readonly FollyDbContext DbContext;

    public LanguageService(FollyDbContext dbContext) => DbContext = dbContext;

    public async Task<IEnumerable<DTO.Language>> GetAll() => await DbContext.Languages.SelectDTO().ToListAsync();

    public async Task<DTO.Language> GetDefaultLanguage() => await DbContext.Languages.Where(x => x.IsDefault).SelectDTO().FirstAsync();

    public async Task<DTO.Language> GetLanguageById(int id) => await DbContext.Languages.Where(x => x.Id == id).SelectDTO().FirstAsync();
}
