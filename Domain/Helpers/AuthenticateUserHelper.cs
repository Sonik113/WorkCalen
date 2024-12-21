using System.Security.Claims;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.ViewModels.LogAndReg;

namespace WorkCalendarik.Domain.Helpers;

public static class AuthenticateUserHelper
{
    public static ClaimsIdentity Authenticate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
            new Claim("AvatarPath", user.ImagePath),
        };
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimTypes.Email, ClaimsIdentity.DefaultRoleClaimType);
    }
}