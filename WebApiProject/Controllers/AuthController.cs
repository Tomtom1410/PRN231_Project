using DataAccess.Dtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.Constants;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBusiness _authBusiness;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthBusiness authBusiness, IConfiguration configuration)
        {
            _authBusiness = authBusiness;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModelDto account)
        {
            var response = await _authBusiness.GetAccountAsync(account.Username, account.Password);
            if (response == null)
            {
                return NotFound();
            }

            var token = GenerateToken(response);

            var loginResponse = new LoginModelResponse()
            {
                Account = response,
                Token = token
            };

            return Ok(loginResponse);
        }

        private string GenerateToken(AccountDto account)
        {
            var role = Roles.Student;

            if (account.IsTeacher == true)
            {
                role = Roles.Teacher;
            }
            #region Tạo token

            try
            {
                //create claims details based on the user information
                var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim(ClaimTypes.Role, role),
                            new Claim("Id", account.Id.ToString()),
                            new Claim("FullName", account.FullName),
                            new Claim("UserName", account.Username)
                        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(null, null, claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }

            #endregion
        }

    }
}
