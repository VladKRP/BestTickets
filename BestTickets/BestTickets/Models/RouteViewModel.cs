using System;
using System.ComponentModel.DataAnnotations;

namespace BestTickets.Models
{
    public class RouteViewModel
    {
        [Required]
        [Display(Name = "Отправление")]
        public string DeparturePlace { get; set; }
        [Required]
        [Display(Name = "Прибытие")]
        public string ArrivalPlace { get; set; }
        [Required]
        [Display(Name = "Дата")]
        public string Date { get; set; }

        public string SetCurrentDate()
        {
            var date = DateTime.Today;
            var day = date.Day.ToString();
            if (day.Length == 1)
                day = string.Concat("0", day);
            var month = date.Month.ToString();
            if (month.Length == 1)
                month = string.Concat("0", month);
            return string.Join(".", date.Year, month, day);
        }
    }
}