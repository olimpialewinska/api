using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly InMemoryDbContext _dbContext;

        public TasksController(InMemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/tasks/{userId}
        [HttpGet("{userId}")]
        public IActionResult GetTasks(Guid userId)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                return Ok(user.Tasks);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }

        // GET api/tasks/{userId}/{taskId}
        [HttpGet("{userId}/{taskId}")]
        public IActionResult GetTask(Guid userId, Guid taskId)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                var task = user.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }

        // POST api/tasks/{userId}
        [HttpPost("{userId}")]
        public IActionResult CreateTask(Guid userId, JObject taskData)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                Console.WriteLine(taskData["description"].ToString());

                var taskModel = new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Description = taskData["description"].ToString(),
                    IsCompleted = false
                };

                user.Tasks.Add(taskModel);
                _dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetTask), new { userId = userId, taskId = taskModel.Id }, taskModel);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }

        // PUT api/tasks/{userId}/{taskId}/description
        [HttpPut("{userId}/{taskId}/description")]
        public IActionResult UpdateTaskDescription(Guid userId, Guid taskId, JObject taskData)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                var task = user.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    return NotFound();


                task.Description = taskData["description"].ToString();
                _dbContext.SaveChanges();


                return NoContent();
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }

        // PUT api/tasks/{userId}/{taskId}/completed
        [HttpPut("{userId}/{taskId}/completed")]
        public IActionResult UpdateTaskCompleted(Guid userId, Guid taskId)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                var task = user.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    return NotFound();

                task.IsCompleted = true;
                _dbContext.SaveChanges();


                return NoContent();
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }

        // DELETE api/tasks/{userId}/{taskId}
        [HttpDelete("{userId}/{taskId}")]
        public IActionResult DeleteTask(Guid userId, Guid taskId)
        {
            try
            {
                var user = _dbContext.Users.Include(f => f.Tasks).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound();

                var task = user.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    return NotFound();

                user.Tasks.Remove(task);
                _dbContext.SaveChanges();


                return NoContent();
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };

                return StatusCode(500, problemDetails);
            }
        }
    }
}