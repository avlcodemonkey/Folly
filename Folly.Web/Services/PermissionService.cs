using Folly.Domain;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class PermissionService : IPermissionService {
    private readonly FollyDbContext DbContext;

    public PermissionService(FollyDbContext dbContext) => DbContext = dbContext;

    public async Task<bool> Delete(int permissionId) {
        var permission = await DbContext.Permissions.FirstAsync(x => x.Id == permissionId);
        DbContext.Remove(permission);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Permission>> GetAll() => await DbContext.Permissions.SelectDTO().ToListAsync();

    public async Task<bool> Save(DTO.Permission permissionDTO) {
        var permission = permissionDTO.ToModel();
        if (permission.Id > 0)
            DbContext.Permissions.Update(permission);
        else
            DbContext.Permissions.Add(permission);

        return await DbContext.SaveChangesAsync() > 0;
    }
}
