using System;
using System.ComponentModel.DataAnnotations;

namespace BestTickets.Domain.Models
{
    public class Route
    {
        [Required]
        [Display(Name = "Отправление")]
        public string DeparturePlace { get; set; }
        [Required]
        [Display(Name = "Прибытие")]
        public string ArrivalPlace { get; set; }
        [Required]
        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        public Route() { }

        public Route(string departurePlace, string arrivalPlace, DateTime? date)
        {
            DeparturePlace = departurePlace;
            ArrivalPlace = arrivalPlace;
            Date = date;
        }
    }
}