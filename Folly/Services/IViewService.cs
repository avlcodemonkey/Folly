using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Folly.Models;

namespace Folly.Services;

public interface IViewService
{
    Task<IEnumerable<Role>> GetAllRoles();

    Task<Dictionary<string, List<Permission>>> GetControllerPermissions();

    Task<IEnumerable<SelectListItem>> GetLanguageList();
}
