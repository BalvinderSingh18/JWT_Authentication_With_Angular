using JWT_Authentication_API;
using JWT_Authentication_API.Identity;
using JWT_Authentication_API.ServiceContract;
using JWT_Authentication_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
string cs = builder.Configuration.GetConnectionString("ConStr");

builder.Services.AddEntityFrameworkSqlServer().
  AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(cs, b =>
 b.MigrationsAssembly("JWT_Authentication_API")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//*****
builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
builder.Services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
builder.Services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddUserStore<ApplicationUserStore>()
.AddUserManager<ApplicationUserManager>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>()
.AddRoleStore<ApplicationRoleStore>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ApplicationRoleStore>();
builder.Services.AddScoped<ApplicationUserStore>();
//***
//Add JWT Authentication
var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);

var appSetting = appSettingSection.Get<AppSettings>();
var key = System.Text.Encoding.ASCII.GetBytes(appSetting.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddCookie()
  .AddJwtBearer(x =>
  {
      x.RequireHttpsMetadata = false;
      x.TokenValidationParameters = new TokenValidationParameters()
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
      };
  });
//******
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
           ConfigureSwaggerOptions>();
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
////Add Data
//IServiceScopeFactory serviceScopeFactory = app.Services.GetRequiredService
//  <IServiceScopeFactory>();
//using (IServiceScope scope = serviceScopeFactory.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService
//     <RoleManager<ApplicationRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService
//     <UserManager<ApplicationUser>>();
//    //Roles
//    if (!await roleManager.RoleExistsAsync("Admin"))
//    {
//        var role = new ApplicationRole();
//        role.Name = "Admin";
//        await roleManager.CreateAsync(role);
//    }
//    if (!await roleManager.RoleExistsAsync("Employee"))
//    {
//        var role = new ApplicationRole();
//        role.Name = "Employee";
//        await roleManager.CreateAsync(role);
//    }
//    //Users
//    if (await userManager.FindByNameAsync("Jashan") == null)
//    {
//        var user = new ApplicationUser();
//        user.UserName = "Jashan";
//        user.Email = "jashan@gmail.com";
//        var chkUser = await userManager.CreateAsync(user, "Jashan@8020");
//        if (chkUser.Succeeded)
//        {
//            await userManager.AddToRoleAsync(user, "Admin");
//        }
//    }
//    if (await userManager.FindByNameAsync("Mayank") == null)
//    {
//        var user = new ApplicationUser();
//        user.UserName = "Mayank";
//        user.Email = "mayank@gmail.com";
//        var chkUser = await userManager.CreateAsync(user, "Mayank@8020");
//        if (chkUser.Succeeded)
//        {
//            await userManager.AddToRoleAsync(user, "Employee");
//        }
//    }
//}
app.MapControllers();

app.Run();
