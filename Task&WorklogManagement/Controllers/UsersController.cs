using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Models;

namespace Task_WorklogManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllAsync();
            var response = ResponseModel.Success(users);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            var response = ResponseModel.Success(user);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            var user = await _service.CreateAsync(userDTO);
            var response = ResponseModel.Success(user);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPut("{id:guid}")]  
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDTO userDTO)
        {
            var user = await _service.UpdateAsync(id, userDTO);
            var response = ResponseModel.Success(user);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            var response = ResponseModel.Success("Delete user successfully.");
            return StatusCode((int)response.HttpStatus, response);
        }
    }
}
