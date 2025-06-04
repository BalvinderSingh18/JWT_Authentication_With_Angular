using JWT_Authentication_API.Identity;
using JWT_Authentication_API.Models.ViewModel;

namespace JWT_Authentication_API.ServiceContract
{
    public interface IUserService
    {
        Task<ApplicationUser> Authenticate(LoginVM loginVM);
        Task<ApplicationUser?> Register(RegisterVM registerVM);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser?> UpdateUser(string userId, UpdateUserVM updateUserVM);
        Task<bool> DeleteUser(string userId);

    }
}
