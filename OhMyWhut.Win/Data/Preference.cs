using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(Preference))]
    public class Preference
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
