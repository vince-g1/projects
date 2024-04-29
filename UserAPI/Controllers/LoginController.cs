using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUserService _userService;
        public LoginController(IConfiguration config, IUserService userService)
        {

            _config = config;
            _userService = userService;

        }

        private User AuthenticateUser(UserLoginRequestDto user)
        {
            User _user = null;

            _user = _userService.GetUserByEmail(user);

            return _user;
        }

        private string GenerateToken(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLoginRequestDto user)
        {
            IActionResult response = Unauthorized();
            var userChecked = AuthenticateUser(user);

            if(userChecked != null)
            {
                var token = GenerateToken(userChecked);
                response = Ok(new { token = token });
            }
            return response;
        }
    }
}
