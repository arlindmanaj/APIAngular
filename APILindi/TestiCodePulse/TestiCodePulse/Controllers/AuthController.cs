﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestiCodePulse.Models.DTO;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        // POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Check email
            var identityUser =  await userManager.FindByEmailAsync(request.Email);
            
            if(identityUser is not null)
            {
                // Check password & njejten koh edhe email
               
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
            
                if (checkPasswordResult)
                {
                    //Create token and response
                    
                    var roles = await userManager.GetRolesAsync(identityUser);

                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);


                }


               
            
            }
            ModelState.AddModelError("", "Email or Password incorrect");

            return ValidationProblem(ModelState);


        }

        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto request)
        {
            //Create IDUser object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),

            };
            // Create User
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if(identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}
