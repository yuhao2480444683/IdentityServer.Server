using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Server.Data;
using IdentityServer.Server.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Server.Controllers
{
    [Authorize(AuthenticationSchemes =
        IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/Tasks")]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IEnumerable<Models.Task> TaskCollection;

        public TasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> GetMyTasksAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = (await _userManager.GetUserAsync(User)).Id;
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApplicationUserID == userId);
            if (user != null)
            {
                //todo
                var tasks = await _context.Tasks.ToListAsync();

                foreach (Models.Task task in tasks)
                {
                    if (task.UserID != user.ID)
                    {
                        tasks.Remove(task);
                    }
                }

                return Ok(tasks);
            }
            return StatusCode((int)HttpStatusCode.Forbidden);


        }

        // GET: api/task?id=5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks
        [HttpPut]
        public async Task<IActionResult> PutTask(int id, [FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //_context.Entry(task).State = EntityState.Modified;
            var putresult = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (putresult == null)
            {
                return BadRequest();
            }

            var dateresult = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == task.ID);
            dateresult.FinishFlag = task.FinishFlag;
            dateresult.FinishTime = task.FinishTime;
            dateresult.Introduction = task.Introduction;
           
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var userId = (await _userManager.GetUserAsync(User)).Id;
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApplicationUserID == userId);
            if (user == null)
            {
                return BadRequest();
            }

            var thisTask = await _context.Tasks.SingleOrDefaultAsync(m => m.Date == task.Date && m.Begin == task.Begin);
            if (thisTask != null)
            {
                return BadRequest();
            }

            var newTask = new Models.Task
            {
                Date = task.Date,
                UserID = user.ID,
                Begin = task.Begin,
                FinishTime = 0,
                DefaultTime = task.DefaultTime,
                Introduction = "",
                FinishFlag = -1
            };

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = newTask.ID }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}