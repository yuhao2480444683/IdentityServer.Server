using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Server.Data;
using IdentityServer.Server.Models;

namespace IdentityServer.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/BlackListProgresses")]
    public class BlackListProgressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlackListProgressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<BlackListProgress>> GetBlackListProgressesAsync()
        {
            var progresses = await _context.Progresses.ToListAsync();
            return progresses;

        }

      



        // GET: api/BlackListProgresses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlackListProgress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blackListProgress = await _context.Progresses.SingleOrDefaultAsync(m => m.ID == id);

            if (blackListProgress == null)
            {
                return NotFound();
            }

            return Ok(blackListProgress);
        }




        // PUT: api/BlackListProgresses
        [HttpPut]
        public async Task<IActionResult> PutBlackListProgress(
            [FromBody] BlackListProgress blackListProgress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            //_context.Entry(blackListProgress).State = EntityState.Modified;
            var putresult = await _context.Progresses.SingleOrDefaultAsync(m => m.ID == blackListProgress.ID);
            if (putresult == null)
            {
                return BadRequest();
            }

            putresult.FileName = blackListProgress.FileName;

            await _context.SaveChangesAsync();
            return Ok(blackListProgress);

        }
        



        /*
        [HttpPut]
        public async Task<IActionResult> PutBlackListProgress(int id, [FromBody] BlackListProgress blackListProgress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blackListProgress.ID)
            {
                return BadRequest();
            }

            _context.Entry(blackListProgress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlackListProgressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/BlackListProgresses
        [HttpPost]
        public async Task<IActionResult> PostBlackListProgress([FromBody] BlackListProgress blackListProgress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Progresses.Add(blackListProgress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlackListProgress", new { id = blackListProgress.ID }, blackListProgress);
        }

        // DELETE: api/blackListProgresses?id=5
        [HttpDelete]
        public async Task<IActionResult> DeleteBlackListProgress( int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blackListProgress = await _context.Progresses.SingleOrDefaultAsync(m => m.ID == id);
            if (blackListProgress == null)
            {
                return NotFound();
            }

            _context.Progresses.Remove(blackListProgress);
            await _context.SaveChangesAsync();

            return Ok(blackListProgress);
        }

        private bool BlackListProgressExists(int id)
        {
            return _context.Progresses.Any(e => e.ID == id);
        }
    }
}