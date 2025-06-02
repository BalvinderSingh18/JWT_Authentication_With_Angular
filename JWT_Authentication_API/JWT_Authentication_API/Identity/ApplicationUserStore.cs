using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JWT_Authentication_API.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext context):base(context)
        {
                
        }
    }
}
