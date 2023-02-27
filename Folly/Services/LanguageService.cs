using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Folly.Domain.Models;
using Folly.ServiceExtensions;
using DTO = Folly.Models;

namespace Folly.Services;

public class LanguageService : ILanguageService
{
    private readonly FollyDbContext DbContext;

    public LanguageService(FollyDbContext dbContext) => DbContext = dbContext;

    public async Task<IEnumerable<DTO.Language>> GetAll()
        => await DbContext.Languages.SelectDTO().ToListAsync();

    public async Task<DTO.Language> GetDefaultLanguage()
        => await DbContext.Languages.Where(x => x.IsDefault).SelectDTO().FirstOrDefaultAsync();

    public async Task<DTO.Language> GetLanguageById(int id)
            => await DbContext.Languages.Where(x => x.Id == id).SelectDTO().FirstOrDefaultAsync();
}
