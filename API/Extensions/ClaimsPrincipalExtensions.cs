using System.Security.Claims;

namespace API.Exception;

public static class ClaimsPrincipalExtensions{
    public static string GetUsername(this ClaimsPrincipal user){
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}