using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Coin
    {
        private decimal value;
        private string longName;
        private string shortName;

        public decimal Value { get => value; set => this.value = value; }
        public string LongName { get => longName; set => longName = value; }
        [Key]
        public string ShortName { get => shortName; set => shortName = value; }

        public Coin(decimal Value, string LongName, string ShortName)
        {
            longName = LongName;
            value = Value;
            shortName = ShortName;
        }
        public Coin()
        {

        }
    }
}
