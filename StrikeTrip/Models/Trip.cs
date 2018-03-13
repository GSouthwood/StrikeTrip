using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrikeTrip.Models
{
    public class Trip
    {
        public string LocationName { get; set; }
        public string SpotName { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal MaxSurfHeight { get; set; }
        public int NumberOfSurfDays { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal Price { get; set; }
        public int InputMinSurfHeight { get; set; }
        public int InputMaxPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime InputDepartureDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime InputReturnDate { get; set; }
    }
}