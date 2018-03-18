using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using StrikeTrip.UtilityMethods;
using StrikeTrip.DAL;

namespace StrikeTrip.Models
{
    public class Trip
    {

        public string LocationName { get; set; }
        public string SpotName { get; set; }
        public DateTime DepartureDate { get; set; }
        public int FlightId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal MaxSurfHeight { get; set; }
        public int NumberOfSurfDays { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public decimal Price { get; set; }
        public int InputMinSurfHeight { get; set; }
        public int InputMaxPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime InputDepartureDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime InputReturnDate { get; set; }
        public List<ForecastDay> Forecast { get; set; }

        public Trip()
        {

        }

        public Trip(string inputPrice, string inputDepartureDate, string inputReturnDate, string inputHeight)
        {

            InputMinSurfHeight = SurfHeightInt(inputHeight);
            InputMaxPrice = PriceInt(inputPrice);
            InputDepartureDate = DepartDateTime(inputDepartureDate);
            InputReturnDate = ReturnDateTime(inputReturnDate);
            Latitude = "34.0522";
            Longitude = "-118.243";

        }

        public int SurfDays()
        {
            if ((ReturnDate.Day - DepartureDate.Day) < NumberOfSurfDays)
            {
                NumberOfSurfDays = ReturnDate.Day - DepartureDate.Day;
            }
            return NumberOfSurfDays;
        }

        public string PriceString(int price)
        {
            if (price == 99999)
            {
                return "Any";
            }

            return $"${price}";

        }
        public string DateTimeString(DateTime date)
        {
            if (date <= DateAndTime.GetPastDate())
            {
                return "Any";
            }
            else if (date > DateAndTime.GetFutureCompareDate())
            {
                return "Any";
            }
            return date.ToShortDateString();
        }
        public string SurfHeightString(int height)
        {
            if (height <= 0)
            {
                return "Any";
            }
            return $"{height}ft";
        }
        public int SurfHeightInt(string height)
        {
            if (height == "")
            {
                return 0;
            }
            return Int32.Parse(height);

        }
        public int PriceInt(string price)
        {
            if (price == "")
            {
                return 99999;
            }
            return Int32.Parse(price);
        }
        public DateTime DepartDateTime(string departureDate)
        {
            if (departureDate == "")
            {
                return DateAndTime.GetPastDate();
            }
            return DateTime.Parse(departureDate);
        }
        public DateTime ReturnDateTime(string returnDate)
        {
            if (returnDate == "")
            {
                return DateAndTime.GetFutureDate();
            }
            return DateTime.Parse(returnDate);
        }
        public string BestOption(string name)
        {
            if (name == null)
            {
                return "Stay home";
            }
            return name;
        }


    }
}