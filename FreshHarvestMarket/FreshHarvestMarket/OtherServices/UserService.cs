using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FreshHarvestMarket.OtherServices
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        public UserService(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }

        public string? GetAuthenticatedUserID(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user) ?? null;
        }
    }
}
