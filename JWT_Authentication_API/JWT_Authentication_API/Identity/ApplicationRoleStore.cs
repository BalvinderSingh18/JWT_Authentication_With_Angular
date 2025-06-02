using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JWT_Authentication_API.Identity
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, ApplicationDbContext>
    {
        public ApplicationRoleStore(ApplicationDbContext context,
        IdentityErrorDescriber errorDescriber) : base(context, errorDescriber)
        {

        }
    }
}
