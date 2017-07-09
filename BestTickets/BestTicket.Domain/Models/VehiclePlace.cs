using System;
using System.Globalization;
using System.Linq;

namespace BestTickets.Domain.Models
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
            Cost = MoneyToDouble(cost);
        }

        private double? MoneyToDouble(string cost)
        {
            double? result = null;
            if (cost != null)
            {
                var money = cost.TakeWhile(c => char.IsDigit(c) || c == '.' || c == ',').Aggregate("", (x, y) => x += y);
                result = Convert.ToDouble(money, CultureInfo.InvariantCulture.NumberFormat);
            }
            return result;
        }

        public int CompareTo(VehiclePlace obj)
        {
            if (this.Cost > obj.Cost) return 1;
            if (this.Cost < obj.Cost) return -1;
            else return 0;
        }
    }
}