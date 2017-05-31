using System;
using System.Globalization;
using System.Linq;

namespace BestTickets.Models
{
    public class VehiclePlace: IComparable<VehiclePlace>
    {
        public string Type { get; set; }
        public string Amount { get; set; }
        public double? Cost { get; set; }
        public bool isCostLessThanAverage { get; set; }

        public VehiclePlace(string type, string amount, string cost)
        {
            Type = type;
            Amount = amount;
            Cost = moneyToDouble(cost);
        }

        private double? moneyToDouble(string cost)
        {
            if (cost == string.Empty)
                return null;
            var money = cost.TakeWhile(c => char.IsDigit(c) || c == '.' || c == ',').Select(c => c == ',' ? '.' : c).Aggregate("", (x, y) => x += y);
            return Convert.ToDouble(money, CultureInfo.InvariantCulture.NumberFormat);
        }

        public int CompareTo(VehiclePlace obj)
        {
            if (this.Cost > obj.Cost) return 1;
            if (this.Cost < obj.Cost) return -1;
            else return 0;
        }
    }
}