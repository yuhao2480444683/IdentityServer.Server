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
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// 获得我。
        /// </summary>
        /// <returns></returns>
        // GET: api/Users/Me
        [HttpGet]
        public async Task<IActionResult> GetMeAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = (await _userManager.GetUserAsync(User)).Id;
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApplicationUserID == userId);

            if (user == null)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            else
            {
                return Ok(user);
            }

        }



        /// <summary>
        /// 根据用户ID获取当前用户。
        /// </summary>
        /// <param ID="id"></param>
        /// <returns></returns>
        /// // GET: api/user?id=5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }




        // PUT: api/user?applicationUserID=5
        [HttpPut]
        public async Task<IActionResult> PutUser(string applicationUserID, [FromBody] AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          

            //_context.Entry(user).State = EntityState.Modified;
            var putresult = await _context.Users.SingleOrDefaultAsync(m => m.ID == user.ID && m.ApplicationUserID == applicationUserID);
            if (putresult == null)
            {
                return BadRequest();
            }

            if (applicationUserID != user.ApplicationUserID)
            {
                return BadRequest();
            }



            putresult.UserName = user.UserName;
            putresult.Image = user.Image;
            await _context.SaveChangesAsync();
            return Ok(putresult);

        }


    
        /*

        // DELETE: api/user?id=5
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }


    */


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}