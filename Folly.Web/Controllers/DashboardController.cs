using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class DashboardController(ILogger<DashboardController> logger) : BaseController(logger) {
    public IActionResult Index() => View("Index");
}
