using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class DashboardController : BaseController {

    public DashboardController(ILogger<DashboardController> logger) : base(logger) { }

    public IActionResult Index() => View("Index");
}
