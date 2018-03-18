using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrikeTrip.Models
{
    public class ForecastDay
    {
        public string LocationName { get; set; }
        public string SpotName { get; set; }
        public DateTime ForecastForDate { get; set; }
        public decimal AverageSurfHeight { get; set; }
        //public int NumberOfFlights { get; set; }
        //public int InputMinSurfHeight { get; set; }
        //public int InputMaxPrice { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime InputDepartureDate { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime InputReturnDate { get; set; }
        //public bool IsMostlyOffshore { get; set; }
        //public bool IsOffshore { get; set; }
        //public bool IsMostlyOnshore { get; set; }
        //public bool IsOnshore { get; set; }
        public decimal WindDirection { get; set; }

    }
}