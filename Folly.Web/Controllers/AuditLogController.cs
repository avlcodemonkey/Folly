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
public class AuditLogController(IAuditLogService auditLogService, IPermissionService permissionService, ILogger<AuditLogController> logger)
    : BaseController(logger) {

    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IAuditLogService _AuditLogService = auditLogService;

    [HttpGet]
    public async Task<IActionResult> View(long id) {
        var model = await _AuditLogService.GetLogByIdAsync(id);
        if (model == null) {
            ViewData.AddError(Core.ErrorInvalidId);
        }

        return model == null ? Index(new AuditLogSearch()) : View("View", model);
    }

    [HttpGet]
    public IActionResult Index(AuditLogSearch model) => View("Index", model);

    [HttpPost, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> Search(AuditLogSearch search)
        => Ok((await _AuditLogService.SearchLogsAsync(search)).Select(x => new { x.Id, x.BatchId, x.UniversalDate, x.UserFullName, x.StateDesc, x.Entity }));
}
