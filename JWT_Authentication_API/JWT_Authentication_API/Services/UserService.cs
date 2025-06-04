using JWT_Authentication_API.Identity;
using JWT_Authentication_API.Models.ViewModel;
using JWT_Authentication_API.ServiceContract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWT_Authentication_API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly AppSettings _appSettings;
        public UserService(ApplicationSignInManager signInManager, ApplicationUserManager applicationUserManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _applicationUserManager = applicationUserManager;
            _appSettings = appSettings.Value;
        }
        public async Task<ApplicationUser> Authenticate(LoginVM loginVM)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(loginVM.UserName);
                applicationUser.PasswordHash = "";
                //JWT Token
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.Role_Admin))
                    applicationUser.Role = SD.Role_Admin;
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.Role_Employee))
                    applicationUser.Role = SD.Role_Employee;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                        new Claim(ClaimTypes.Name,applicationUser.Id),
                        new Claim(ClaimTypes.Email,applicationUser.Email),
                        new Claim(ClaimTypes.Role,applicationUser.Role)
                  }),
                    Expires = DateTime.UtcNow.AddHours(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                  SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);

                //*****
                return applicationUser;
            }
            return null;
        }
        public async Task<ApplicationUser?> Register(RegisterVM registerVM)
        2{
            var user = new ApplicationUser
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };

            var result = await _applicationUserManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                // Assign default role
                await _applicationUserManager.AddToRoleAsync(user, SD.Role_Employee);

                // Generate JWT Token
                user.Role = SD.Role_Employee;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                // Clear password hash before returning
                user.PasswordHash = "";

                return user;
            }

            return null;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return _applicationUserManager.Users.ToList(); // You can filter or project as needed
        }

        public async Task<ApplicationUser?> UpdateUser(string userId, UpdateUserVM updateUserVM)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            if (user == null) return null;

            // Check for unique username or email if changed
            if (user.Email != updateUserVM.Email)
            {
                var emailExists = await _applicationUserManager.FindByEmailAsync(updateUserVM.Email);
                if (emailExists != null && emailExists.Id != userId)
                    throw new Exception("Email already in use.");
            }

            if (user.UserName != updateUserVM.UserName)
            {
                var usernameExists = await _applicationUserManager.FindByNameAsync(updateUserVM.UserName);
                if (usernameExists != null && usernameExists.Id != userId)
                    throw new Exception("Username already in use.");
            }

            // Update values
            user.Email = updateUserVM.Email;
            user.UserName = updateUserVM.UserName;

            var result = await _applicationUserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Update failed: {errors}");
            }

            // Optional: Update role
            //if (!string.IsNullOrWhiteSpace(updateUserVM.Role))
            //{
            //    var roles = await _applicationUserManager.GetRolesAsync(user);
            //    await _applicationUserManager.RemoveFromRolesAsync(user, roles);
            //    await _applicationUserManager.AddToRoleAsync(user, updateUserVM.Role);
            //    user.Role = updateUserVM.Role;
            //}

            user.PasswordHash = "";
            return user;
        }


        public async Task<bool> DeleteUser(string userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _applicationUserManager.DeleteAsync(user);
            return result.Succeeded;
        }

    }
}
