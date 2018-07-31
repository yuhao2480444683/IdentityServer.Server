using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/FriendShips")]
    public class FriendShipsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<RankList> rankList;


        public FriendShipsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            rankList = new List<RankList>();
        }


        // GET: api/FriendShips?applicationUserID=?
        [HttpGet]
        public async Task<IActionResult> GetRankingList(string applicationUserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = (await _userManager.GetUserAsync(User)).Id;
            var thisuser = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApplicationUserID == userId);
            if ((thisuser == null) || (thisuser.ApplicationUserID != applicationUserID))
            {
                return BadRequest();
            }
            

            var friends = await _context.FriendShips.ToListAsync();

            foreach (FriendShip thisFriendShip in friends)
            {
                if (thisFriendShip.UserID != thisuser.ID)
                {
                    friends.Remove(thisFriendShip);
                }
            }

            rankList.Clear();

            foreach (FriendShip thisFriendShip in friends)
            {
                var friend = await _context.Users.SingleOrDefaultAsync(m => m.ID == thisFriendShip.FriendID);
                var newItem = new RankList { Name = friend.UserName, TotalTime = friend.TotalTime};
                rankList.Add(newItem);

            }
            var me = new RankList { Name = thisuser.UserName, TotalTime = thisuser.TotalTime };
            rankList.Add(me);
            return Ok(rankList.OrderByDescending(m => m.TotalTime));

  
        }

       


        // POST: api/FriendShips
        [HttpPost]
        public async Task<IActionResult> PostFriendShip([FromBody] AppUser friend)
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

            var thisfriend = await _context.Users.SingleOrDefaultAsync(m => m.UserName == friend.UserName);
            if (thisfriend == null)
            {
                return BadRequest();
            }

            var thisFriendShip = await _context.FriendShips.SingleOrDefaultAsync(m => m.UserID ==user.ID && m.FriendID == thisfriend.ID);

            if (thisFriendShip == null)
            {
                var newFriendShip = new FriendShip { UserID = user.ID, FriendID = thisfriend.ID };
                await _context.FriendShips.AddAsync(newFriendShip);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return Ok(null);
            }
           
        }

        // DELETE: api/FriendShips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendShip([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var friendShip = await _context.FriendShips.SingleOrDefaultAsync(m => m.ID == id);
            if (friendShip == null)
            {
                return NotFound();
            }

            _context.FriendShips.Remove(friendShip);
            await _context.SaveChangesAsync();

            return Ok(friendShip);
        }

        private bool FriendShipExists(int id)
        {
            return _context.FriendShips.Any(e => e.ID == id);
        }
    }
}