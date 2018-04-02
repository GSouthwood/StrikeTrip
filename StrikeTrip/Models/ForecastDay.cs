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
        public decimal Price { get; set; }
        public string Latitude { get; set; }
        public string OriginAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public string Longitude { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int FlightId { get; set; }
        public decimal HistoricalSurfHeight { get; set; }
        public decimal HistoticalAveragePrice { get; set; }
        public decimal HistoricalMinHeight { get; set; }
        public decimal HistoricalMaxHeight { get; set; }
        public decimal HistoricalMinPrice { get; set; }
        public decimal HistoricalMaxPrice { get; set; }

    }
}