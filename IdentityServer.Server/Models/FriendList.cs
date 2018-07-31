using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Server.Models
{
    public class FriendList
    { 
        public int ID { get; set; }

        public int FriendID { get; set; }

        public string FriendUserName { get; set; }

        public string FriendImage { get; set; }

    }
}
