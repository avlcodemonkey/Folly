using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Services;

public interface IViewService {
    Task<IEnumerable<Role>> GetAllRoles();

    Task<Dictionary<string, List<Permission>>> GetControllerPermissions();

    Task<IEnumerable<SelectListItem>> GetLanguageList();
}
