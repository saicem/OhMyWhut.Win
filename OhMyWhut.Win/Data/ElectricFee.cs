using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(ElectricFee))]
    public class ElectricFee
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public float RemainPower { get; set; }

        public string RemainName { get; set; }

        public float TotalValue { get; set; }

        public string Unit { get; set; }

        public float MeterOverdue { get; set; }

        [NotMapped]
        public string Surplus { get => RemainPower.ToString() + RemainName; }

        [NotMapped]
        public string Total { get => TotalValue.ToString() + Unit; }
    }
}
