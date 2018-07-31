using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdentityServer.Server.Models
{
    public class BlackListProgress
    {
        /// <summary>
        /// 主键。
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 进程名称。
        /// </summary>
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// 用户定义名称。
        /// </summary>
        [JsonProperty("resetName")]   
        public string ResetName { get; set; }

        /// <summary>
        /// 种类。
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }


    }
}
