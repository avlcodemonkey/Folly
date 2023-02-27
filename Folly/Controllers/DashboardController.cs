using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Folly.Configuration;
using Folly.Utils;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName), Petch]
public class DashboardController : BaseController
{
    public DashboardController(IAppConfiguration appConfig) : base(appConfig)
    {
    }

    public IActionResult Index() => View("Index");
}
