using Core.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindUserByClaimsPrincipleWithAddressAsync(
            this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(u => u.Address)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipalAsync(
            this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.
                SingleOrDefaultAsync(u => u.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}
