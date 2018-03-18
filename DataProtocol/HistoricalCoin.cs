using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class HistoricalCoin
    {
        private string shortName;
        private DateTime dateTime;
        private decimal value;
        private string longName;

        [Key, Column(Order = 0)]
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        [Key, Column(Order = 1)]
        public string ShortName { get => shortName; set => shortName = value; }

        public decimal Value { get => value; set => this.value = value; }
        public string LongName { get => longName; set => longName = value; }

        public override string ToString()
        {
            return "Full Name: " + this.longName + ", Short Name: " + this.shortName + ", Value: " + this.value;
        }
        public HistoricalCoin(decimal Value, string LongName, string ShortName, DateTime DateTime)
        {
            dateTime = DateTime;
            longName = LongName;
            value = Value;
            shortName = ShortName;
        }

        public HistoricalCoin()
        {

        }
    }
}
