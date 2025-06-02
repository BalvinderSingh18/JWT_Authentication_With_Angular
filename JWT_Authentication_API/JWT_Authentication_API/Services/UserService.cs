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
    }
}
