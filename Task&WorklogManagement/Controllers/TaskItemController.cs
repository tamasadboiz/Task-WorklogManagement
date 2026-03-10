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
    [Authorize(Roles = "ADMIN")]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;
        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskItemService.GetAllAsync();
            var response = ResponseModel.Success(tasks);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if(task == null)
            {
                return NotFound();
            }    
            var response = ResponseModel.Success(task);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemDTO createTaskItemDTO)
        {
            var task = await _taskItemService.CreateAsync(createTaskItemDTO);
            var response = ResponseModel.Success(task);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskItemDTO updateTaskItemDTO)
        {
            var task = await _taskItemService.UpdateAsync(id, updateTaskItemDTO);
            var response = ResponseModel.Success(task);
            return StatusCode((int)response.HttpStatus, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskItemService.DeleteAsync(id);
            var response = ResponseModel.Success("Delete task item successfully.");
            return StatusCode((int)response.HttpStatus, response);
        }
    }
}
