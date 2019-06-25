using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreJwt.Controllers.RequestModels.Authentication;
using NetCoreJwt.Helpers;

namespace NetCoreJwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtTokenManager tokenManager;
        public AuthenticationController(IOptions<JwtTokenManager> tokenManager)
        {
            this.tokenManager = tokenManager.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] TokenRequest request)
        {

            if (ModelState.IsValid)
            {
                // to do: add actual user authentication logic
                //   i.e: check agains username and password stored in db
                bool isAuthenticated = request.Username == "john" && request.Password == "doe";

                if (isAuthenticated)
                {
                    var claim = new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManager.Secret));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var jwtToken = new JwtSecurityToken(
                        tokenManager.Issuer,
                        tokenManager.Audience,
                        claim,
                        expires: DateTime.Now.AddMinutes(tokenManager.AccessExpiration),
                        signingCredentials: credentials
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwtToken));
                }
                else
                {
                    return BadRequest("Unable to authenticate user");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}