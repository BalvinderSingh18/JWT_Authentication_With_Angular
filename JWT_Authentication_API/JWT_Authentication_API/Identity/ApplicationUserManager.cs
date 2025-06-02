using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JWT_Authentication_API.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(ApplicationUserStore applicationUserStore, IOptions<IdentityOptions> options,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber identityErrorDescriber, IServiceProvider service, ILogger<ApplicationUserManager> logger) : base(
            applicationUserStore, options, passwordHasher, userValidators, passwordValidators, lookupNormalizer, identityErrorDescriber, service, logger)
        {
        }
    }
}
