using Folly.Configuration;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class DashboardController : BaseController {

    public DashboardController(IAppConfiguration appConfig, ILogger<DashboardController> logger) : base(appConfig, logger) { }

    public IActionResult Index() => View("Index");
}
