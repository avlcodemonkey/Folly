using System.Globalization;
using System.Security.Claims;
using Folly.Services;
using Microsoft.AspNetCore.Authentication;

namespace Folly.Utils;

public sealed class ClaimsTransformer : IClaimsTransformation {
    private readonly IUserService UserService;

    public ClaimsTransformer(IUserService userService) => UserService = userService;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal) {
        if (principal.Identity == null)
            return principal;

        var currentPrincipal = (ClaimsIdentity)principal.Identity;
        if (currentPrincipal.Claims.Any(x => x.Type == currentPrincipal.RoleClaimType))
            return principal;

        if (string.IsNullOrWhiteSpace(principal.Identity.Name))
            return principal;

        var user = await UserService.GetUserByUserName(principal.Identity.Name);
        if (user == null)
            return principal;

        var claims = await UserService.GetClaimsByUserId(user.Id);
        if (claims.Any())
            currentPrincipal.AddClaims(claims.Select(x => new Claim(currentPrincipal.RoleClaimType, $"{x.ControllerName}.{x.ActionName}".ToLower(CultureInfo.InvariantCulture))));
        return principal;
    }
}
