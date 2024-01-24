using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Services;

public interface IViewService {
    Task<IEnumerable<Role>> GetAllRolesAsync();

    Task<Dictionary<string, List<Permission>>> GetControllerPermissionsAsync();

    Task<IEnumerable<SelectListItem>> GetLanguageSelectListAsync();

    Task<IEnumerable<SelectListItem>> GetUserSelectListAsync();

    IEnumerable<SelectListItem> GetEntityStatesSelectList();
}
