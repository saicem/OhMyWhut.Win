using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(ElectricFee))]
    public class ElectricFee
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public float RemainPower { get; set; }

        public string RemainName { get; set; }

        public float TotalValue { get; set; }

        public string Unit { get; set; }

        public float MeterOverdue { get; set; }
    }
}
