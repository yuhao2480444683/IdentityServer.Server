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
    [Route("api/FriendLists")]
    public class FriendListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<FriendList> myFriendList;

        public FriendListsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            myFriendList = new List<FriendList>();
        }

        // GET: api/FriendLists/?
        [HttpGet("{applicationUserID}")]
        public async Task<IActionResult> GetMyFriend([FromRoute] string applicationUserID)
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
            myFriendList.Clear();

            foreach (FriendShip thisFriendShip in friends)
            {
                var friend = await _context.Users.SingleOrDefaultAsync(m => m.ID == thisFriendShip.FriendID);
                var newItem = new FriendList { FriendID = friend.ID, FriendUserName = friend.UserName, FriendImage = friend.Image };
                myFriendList.Add(newItem);

            }
            return Ok(myFriendList); 



            /*
            var x = await _context.Users.Include(m => m.FriendShips)
                .ThenInclude(m => m.Friend).SingleOrDefaultAsync(m => m.ID == thisuser.ID);
            if (x.FriendShips.Count == 0)
            {
                return Ok(null);
            }

            foreach (FriendShip friendShip in x.FriendShips)
            {
                var friend = await _context.Users.SingleOrDefaultAsync(m => m.ID == friendShip.FriendID);
                var newItem = new FriendList { FriendID = friend.ID, FriendUserName = friend.UserName, FriendImage = friend.Image };
                friendList.Add(newItem);
            }
            return Ok(friendList);*/


        }


        // DELETE: api/FriendLists/?
        [HttpDelete("{friendUserName}")]
        public async Task<IActionResult> DeleteFriend([FromRoute] string friendUserName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var thisfriend = await _context.Users.SingleOrDefaultAsync(m => m.UserName == friendUserName);
            if (thisfriend == null)
            {
                return BadRequest();

            }
            var userId = (await _userManager.GetUserAsync(User)).Id;
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(m => m.ApplicationUserID == userId);
            var thisfriendShip = await _context.FriendShips.SingleOrDefaultAsync(m =>m.UserID == user.ID  && m.FriendID == thisfriend.ID);
            if (thisfriendShip == null)
            {
                return NotFound();
            }

            _context.FriendShips.Remove(thisfriendShip);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}