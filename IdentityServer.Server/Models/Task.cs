using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdentityServer.Server.Models
{
    public class Task
    {
        /// <summary>
        /// 主键。
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 创建日期。
        /// </summary>
        [JsonProperty("date")]
        public String Date { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        [JsonProperty("begin")]
        public String Begin { get; set; }

        /// <summary>
        /// 任务总时间。
        /// </summary>
        [JsonProperty("defaultTime")]
        public int DefaultTime { get; set; }

        /// <summary>
        /// 任务当前完成时间。
        /// </summary>
        [JsonProperty("finishTime")]
        public int FinishTime { get; set; }

        /// <summary>
        /// 任务说明。
        /// </summary>
        [JsonProperty("introduction")]
        public String Introduction { get; set; }

        /// <summary>
        /// 是否完成任务。
        /// </summary>
        [JsonProperty("finishFlag")]
        public int FinishFlag { get; set; }

        /// <summary>
        /// 所属用户ID。
        /// </summary>
        [JsonProperty("userId")]
        public int UserID { get; set; }

        public AppUser AppUser { get; set; }

    }
}
