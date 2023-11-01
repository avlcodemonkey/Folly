using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

/// <summary>
/// Fetches languages from the DB.
/// Use AsNoTracking since languages can't be modified.
/// </summary>
public sealed class LanguageService : ILanguageService {
    private readonly FollyDbContext _DbContext;

    public LanguageService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<IEnumerable<DTO.Language>> GetAllLanguagesAsync()
        => await _DbContext.Languages.AsNoTracking().SelectAsDTO().ToListAsync();

    public async Task<DTO.Language> GetLanguageByIdAsync(int id)
        => await _DbContext.Languages.AsNoTracking().Where(x => x.Id == id).SelectAsDTO().FirstAsync();
}
