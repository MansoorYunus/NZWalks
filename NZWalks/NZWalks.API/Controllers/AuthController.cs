using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public UserManager<IdentityUser> UserManager { get; }
        public ITokenRepository TokenRepository { get; }

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            UserManager = userManager;
            TokenRepository = tokenRepository;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = request.userName,
                Email = request.userName
            };

            var identityResult = await UserManager.CreateAsync(IdentityUser, request.password);

            if (identityResult.Succeeded)
            {
                if (request.Roles.Any() && request.Roles != null)
                {
                    identityResult = await UserManager.AddToRolesAsync(IdentityUser, request.Roles);
                    if (identityResult.Succeeded)
                    {

                        return Ok("User was succesfully registered. Please login.");
                    }

                }


            }
            return BadRequest("Something Went wrong. Try again");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await UserManager.FindByEmailAsync(request.userName);

            if (user != null)
            {
                var checkPasswordResult = await UserManager.CheckPasswordAsync(user, request.password);
                if (checkPasswordResult)
                {
                    var userRoles = await UserManager.GetRolesAsync(user);

                    if (userRoles != null)
                    {
                        var jwtToken = TokenRepository.CreateJwtToken(user, userRoles.ToList());

                        var response = new LoginResponseDto
                        {
                            jwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password incorrect");
        }
    }
}
