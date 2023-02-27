using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Folly.Services;

namespace Folly.Utils;

public class ClaimsTransformer : IClaimsTransformation
{
    private readonly IUserService UserService;

    public ClaimsTransformer(IUserService userService)
    {
        UserService = userService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var currentPrincipal = (ClaimsIdentity)principal.Identity;
        if (currentPrincipal.Claims.Any(x => x.Type == currentPrincipal.RoleClaimType))
            return principal;

        var user = await UserService.GetUserByUsername(principal.Identity.Name);
        if (user == null)
            return principal;

        var claims = await UserService.GetClaimsByUserId(user.Id);
        if (claims.Any())
            currentPrincipal.AddClaims(claims.Select(x => new Claim(currentPrincipal.RoleClaimType, $"{x.ControllerName}.{x.ActionName}".ToLower())));
        return principal;
    }
}
