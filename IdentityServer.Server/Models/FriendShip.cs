using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdentityServer.Server.Models
{
    public class FriendShip
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("userId")]
        public int UserID { get; set; }

        public AppUser AppUser { get; set; }

        [JsonProperty("id")]
        public int FriendID { get; set; }

        public AppUser Friend { get; set; }

    }
}
