using Folly.Attributes;
using Folly.Extensions;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class AuditLogController(IAuditLogService auditLogService, IUserService userService, ILogger<AuditLogController> logger) : BaseController(logger) {
    private readonly IUserService _UserService = userService;
    private readonly IAuditLogService _AuditLogService = auditLogService;

    [HttpGet]
    public IActionResult Index(AuditLogSearch model) => View("Index", model);

    [HttpPost, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> Search(AuditLogSearch model) => Ok(await _AuditLogService.SearchLogsAsync(model));

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> UserList(string query) => Ok(await _UserService.FindAutocompleteUsersByNameAsync(query));

    [HttpGet]
    public async Task<IActionResult> View(long id) {
        var log = await _AuditLogService.GetLogByIdAsync(id);
        if (log == null) {
            ViewData.AddError(Core.ErrorInvalidId);
        }

        return log == null ? Index(new AuditLogSearch()) : View("View", log);
    }
}
