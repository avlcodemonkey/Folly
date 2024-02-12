using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class LanguageService(FollyDbContext dbContext) : ILanguageService {
    private readonly FollyDbContext _DbContext = dbContext;

    public async Task<IEnumerable<DTO.Language>> GetAllLanguagesAsync()
        => await _DbContext.Languages.AsNoTracking().SelectAsDTO().ToListAsync();

    public async Task<DTO.Language?> GetLanguageByIdAsync(int id)
        => await _DbContext.Languages.AsNoTracking().Where(x => x.Id == id).SelectAsDTO().FirstOrDefaultAsync();
}
