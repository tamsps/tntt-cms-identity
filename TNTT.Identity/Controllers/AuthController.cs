﻿using Microsoft.AspNetCore.Mvc;
using TNTT.Identity.Models;
using TNTT.Identity.Services;

namespace TNTT.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel user)
        {
            var loginResult = _jwtTokenService.GenerateAuthToken(user);

            return loginResult is null ? Unauthorized() : Ok(loginResult);
        }
    }
}