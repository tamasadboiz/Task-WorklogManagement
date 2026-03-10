using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Models;

namespace Task_WorklogManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var result = await _service.RegisterAsync(userDTO);
            var response = ResponseModel.Success(result);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var result = await _service.LoginAsync(userDTO);
            var response = ResponseModel.Success(result);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var result = await _service.RefreshTokenAsync(refreshTokenDTO);
            var response = ResponseModel.Success(result);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            await _service.LogoutAsync(refreshTokenDTO);
            return Ok(new { message = "Đã đăng xuất." });
        }
    }
}
