using _01.FlightBookingSystem.Core.DTO_s.Identity;
using _01.FlightBookingSystem.Core.Models.Identity;
using _03.FlightBookingSystem.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _03.FlightBookingSystem.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper _mapper) : base(_mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Register a new user and assign them the default "User" role
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseAPI(400, "Data Not Valid."));

            var user = _mapper.Map<ApplicationUser>(registerDTO);
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return Ok(new ResponseAPI(200, "Registration is Done."));
                }

                return BadRequest(new ResponseAPI(400, "", roleResult.Errors.Select(e => e.Description)));
            }

            return BadRequest(new ResponseAPI(400, "Registration Failed.", result.Errors.Select(e => e.Description)));
        }

        // Authenticate the user and return a JWT token if credentials are valid
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseAPI(400, "Data Not Valid."));

            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if (user == null)
                return Unauthorized(new ResponseAPI(401, "User Name is Wrong."));

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordCorrect)
                return Unauthorized(new ResponseAPI(401, "Password is Wrong."));

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudiance"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: loginDTO.RememberMe ? DateTime.Now.AddDays(15) : DateTime.Now.AddHours(1)
            );

            return Ok(new LoginResultDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = jwtToken.ValidTo
            });
        }
    }
}
