using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class LanguageService : ILanguageService {
    private readonly FollyDbContext _DbContext;

    public LanguageService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<IEnumerable<DTO.Language>> GetAll() => await _DbContext.Languages.SelectDTO().ToListAsync();

    public async Task<DTO.Language> GetDefaultLanguage() => await _DbContext.Languages.Where(x => x.IsDefault).SelectDTO().FirstAsync();

    public async Task<DTO.Language> GetLanguageById(int id) => await _DbContext.Languages.Where(x => x.Id == id).SelectDTO().FirstAsync();
}
