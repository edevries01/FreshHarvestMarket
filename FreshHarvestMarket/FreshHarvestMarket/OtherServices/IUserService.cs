using System.Security.Claims;

namespace FreshHarvestMarket.OtherServices
{
    public interface IUserService
    {
        public string? GetAuthenticatedUserID(ClaimsPrincipal user);
    }
}
