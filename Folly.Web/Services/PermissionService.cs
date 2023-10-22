using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class PermissionService : IPermissionService {
    private readonly FollyDbContext _DbContext;

    public PermissionService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<bool> Delete(int permissionId) {
        var permission = await _DbContext.Permissions.FirstAsync(x => x.Id == permissionId);
        _DbContext.Remove(permission);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Permission>> GetAll() => await _DbContext.Permissions.SelectDTO().ToListAsync();

    public async Task<bool> Save(DTO.Permission permissionDTO) {
        var permission = permissionDTO.ToModel();
        if (permission.Id > 0)
            _DbContext.Permissions.Update(permission);
        else
            _DbContext.Permissions.Add(permission);

        return await _DbContext.SaveChangesAsync() > 0;
    }
}
