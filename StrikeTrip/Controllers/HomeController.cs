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
            List<Trip> trips = tripSqlDal.TripsFromInput("500", "", "", "5");

            if (trips.Count == 0)
            {

                if (trips.Count == 0)
                {
                    Trip trip = new Trip("500", "", "", "5");

                    trips.Add(trip);
                }
            }
            ViewBag.Latitude = trips[0].Latitude;
            ViewBag.Longitude = trips[0].Longitude‎;

            return View("Index", trips);
        }
        public ActionResult Trips(string inputPrice, string inputDepartureDate, string inputReturnDate, string inputHeight)
        {
            TripSqlDal tripSqlDal = new TripSqlDal(connectionString);
            List<Trip> trips = tripSqlDal.TripsFromInput(inputPrice, inputDepartureDate, inputReturnDate, inputHeight);
            
            if (trips.Count == 0)
            {
                Trip trip = new Trip(inputPrice, inputDepartureDate, inputReturnDate, inputHeight);

                trips.Add(trip);
            }
            ViewBag.Latitude = trips[0].Latitude;
            ViewBag.Longitude = trips[0].Longitude‎;
            

            return View("Trips", trips);
        }
        public ActionResult Forecast(string id)
        {
            ForecastDaySqlDal forecastDaySql = new ForecastDaySqlDal(connectionString);
            List<ForecastDay> forecast = forecastDaySql.GetDetailedForecast(id);

            return View("Forecast", forecast);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Message = "Your contact page.";

            return View("Map");
        }
    }
}