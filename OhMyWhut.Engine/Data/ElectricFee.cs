using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Data
{
    public class ElectricFee
    {
        /// <summary>
        /// 剩余电量
        /// </summary>
        [JsonPropertyName("remainPower")]
        public string RemainPower { get; set; } = null!;

        /// <summary>
        /// RemainPower 的单位
        /// </summary>
        [JsonPropertyName("remainName")]
        public string RemainName { get; set; } = null!;

        /// <summary>
        /// 电表的显示数即总耗电量
        /// </summary>

        [JsonPropertyName("ZVlaue")]
        public string TotalValue { get; set; } = null!;

        /// <summary>
        /// TotalValue 的单位
        /// </summary>
        [JsonPropertyName("unit")]
        public string Unit { get; set; } = null!;

        /// <summary>
        /// 剩余电费，固定单位“元”
        /// </summary>
        [JsonPropertyName("meterOverdue")]
        public string MeterOverdue { get; set; } = null!;
    }
}
