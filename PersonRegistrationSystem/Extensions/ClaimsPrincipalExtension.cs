using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new ArgumentNullException("User not found");

        return Guid.Parse(userIdClaim.Value);
    }

    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        var userNameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name) ??
                            throw new ArgumentNullException("User not found");

        return userNameClaim.Value;
    }
}