using Microsoft.AspNetCore.Identity;

namespace TestiCodePulse.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
