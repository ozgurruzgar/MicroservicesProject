﻿using FreeCourse.IdentityServer.Dtos;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser
            {
                UserName = signupDto.UserName,
                Email = signupDto.Email,
                City = signupDto.City,
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);
            if (!result.Succeeded)
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            else
                return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userClaim == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userClaim.Value);
            if (user == null)
                return BadRequest();

            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City });

        }
    }
}
