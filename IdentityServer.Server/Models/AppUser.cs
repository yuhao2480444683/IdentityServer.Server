using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdentityServer.Server.Models
{
    public class AppUser
    {
        /// <summary>
        /// 主键。
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 自定义用户名。
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户任务完成总时间。
        /// </summary>
        [JsonProperty("totalTime")]
        public int TotalTime { get; set; }

        /// <summary>
        /// 用户任务完成每周时间。
        /// </summary>
        [JsonProperty("weekTime")]
        public int WeekTime { get; set; }



        /// <summary>
        ///     用户。
        /// </summary>
        [JsonProperty("applicationUserID")]
        public string ApplicationUserID { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// 好友列表。
        /// </summary>
        public List<FriendShip> FriendShips { get; set; }


    }
}
