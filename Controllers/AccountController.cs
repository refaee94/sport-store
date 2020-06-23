// using System;
// using System.ComponentModel.DataAnnotations;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;

// namespace SportsStore.Controllers
// {
//     [Authorize]

//     public class AccountController : Controller
//     {
//         private readonly SignInManager<IdentityUser> signInManager;
//         private readonly UserManager<IdentityUser> userManager;
//  private readonly IConfiguration configuration;
//         public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
//         {

//             this.userManager = userManager;
//             this.signInManager = signInManager;
//             this.configuration = configuration;
//         }
//         private async Task<bool> DoLogin(LoginViewModel creds)
//         {
//             IdentityUser user = await userManager.FindByNameAsync(creds.Name);
//             if (user != null)
//             {
//                 await signInManager.SignOutAsync();
//                 Microsoft.AspNetCore.Identity.SignInResult result =
//                 await signInManager.PasswordSignInAsync(user, creds.Password, false, false);
//                 return result.Succeeded;
//             }
//             return false;
//         }




//         [HttpPost("/api/account/login")]
//         public async Task<IActionResult> Login([FromBody] LoginViewModel creds)
//         {

//             if (ModelState.IsValid && await DoLogin(creds))
//             { IdentityUser user = await userManager.FindByNameAsync(creds.Name);
//                 var tokenHandler = new JwtSecurityTokenHandler();
//             var key = Encoding.ASCII.GetBytes(
//                 this.configuration.GetSection("AppSettings:Jwt.Key").Value
//             );
//             var tokenDescriptor = new SecurityTokenDescriptor()
//             {
//                 Subject = new ClaimsIdentity(new Claim[]{
//                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                     new Claim(ClaimTypes.Name, user.UserName)
//                 }),
//                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
//                 SecurityAlgorithms.HmacSha512Signature),
//                 Expires = DateTime.Now.AddDays(1)
//             };
//             var token = tokenHandler.CreateToken(tokenDescriptor);
//             var tokenString = tokenHandler.WriteToken(token);


//             return Ok(tokenString );
//             }
//                 return Unauthorized();
//         }
//         [HttpPost("/api/account/logout")]
//         public async Task<IActionResult> Logout()
//         {
//             await signInManager.SignOutAsync();
//             return Ok();
//         }
//     }
//     public class LoginViewModel
//     {
//         [Required]
//         public string Name { get; set; }
//         [Required]
//         public string Password { get; set; }
//     }
// }
