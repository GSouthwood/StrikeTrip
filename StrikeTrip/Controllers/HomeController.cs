using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrikeTrip.DAL;
using StrikeTrip.Models;
using StrikeTrip.UtilityMethods;

namespace StrikeTrip.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Scraper;Integrated Security=True";

        public ActionResult Index()
        {
            TripSqlDal tripSqlDal = new TripSqlDal(connectionString);
            List<Trip> trips = tripSqlDal.GetTripsDefault();

            return View("Index", trips);
        }
        public ActionResult Trips(string inputPrice, string inputDepartureDate, string inputReturnDate, string inputHeight)
        {
            bool isDeparture = false;
            int departure = 0;
            int returning = 0;
            int price = 0;
            int both = 0;
            if (inputPrice != "")
            {
                price = Int32.Parse(inputPrice);
            }
            int height = 0;
            if (inputHeight != "")
            {
                height = Int32.Parse(inputHeight);
            }
            DateTime departDate = Utility.GetPastDate();
            if (inputDepartureDate != "" && inputReturnDate == "")
            {
                departDate = DateTime.Parse(inputDepartureDate);
                isDeparture = true;
                departure++;
            }
            DateTime returnDate = Utility.GetFutureDate();
            if (inputReturnDate != "" && inputDepartureDate != "")
            {
                returnDate = DateTime.Parse(inputReturnDate);
                departDate = DateTime.Parse(inputDepartureDate);
                both++;
            }
            if (inputDepartureDate == "" && inputReturnDate != "")
            {
                departDate = DateTime.Parse(inputDepartureDate);
                isDeparture = false;
                returning++;
            }
            TripSqlDal tripSqlDal = new TripSqlDal(connectionString);
            List<Trip> trips = new List<Trip>();
            if ((departure == 0 && returning == 0) || both > 0)
            {
                trips = tripSqlDal.GetTripsFromInput(height, departDate, returnDate, price);
            }
            if (departure > 0)
            {
                trips = tripSqlDal.GetTripsFromInput(height, departDate, isDeparture, price);
            }
            if (returning > 0)
            {
                trips = tripSqlDal.GetTripsFromInput(height, returnDate, isDeparture, price);
            }
            

            return View("Trips", trips);
        }
        public ActionResult Forecasts()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}