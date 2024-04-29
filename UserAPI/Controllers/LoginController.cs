using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        /// <summary>
        /// Authenticating user by checking if the given email and password exists in the database 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Return user object if found, null otherwise
        /// </returns>
        private User AuthenticateUser(UserLoginRequestDto user)
        {
            try
            {
                User _user = new User();

                _user = _userService.GetUser(user);

                return _user;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generating token for an authenticated user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Token alongwith claims
        /// </returns>
        private string GenerateToken(User user)
        {
            try
            {
                var claims = new List<Claim> 
                { 
                    new(JwtRegisteredClaimNames.Email, user.Email),
                    new(JwtRegisteredClaimNames.Name, user.FirstName),
                    new(JwtRegisteredClaimNames.FamilyName, user.LastName),
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"], 
                                                _config["Jwt:Audience"], 
                                                claims, 
                                                null,
                                                expires: DateTime.Now.AddMinutes(1),
                                                signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Authenticating user in db and generating token if found
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Token
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLoginRequestDto user)
        {
            try
            {
                IActionResult response = Unauthorized();
                var userChecked = AuthenticateUser(user);

                if (userChecked != null)
                {
                    var token = GenerateToken(userChecked);
                    response = Ok(new { token = token }); 
                }
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
