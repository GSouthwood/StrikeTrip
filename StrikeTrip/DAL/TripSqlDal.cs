using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StrikeTrip.Models;
using System.Data.SqlClient;
using StrikeTrip.UtilityMethods;
using System.Web.Mvc;

namespace StrikeTrip.DAL
{
    public class TripSqlDal
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Scraper;Integrated Security=True";

        #region SQL Strings

        private const string SQL_GetAllTrips = "SELECT Destination.name, Flight.departure_date, Flight.return_date, Flight.price FROM Flight " +
            "JOIN Destination ON Destination.airport_code = Flight.airport_code " +
            "WHERE Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
            "ORDER BY Flight.flight_id DESC) " +
            "ORDER BY Flight.price ASC";

        private const string SQL_GetTopCheapestFromInput = "SELECT DISTINCT TOP @numberToDisplay Destination.name, Flight.departure_date, Flight.return_date, Flight.price FROM Flight " +
            "JOIN Destination ON Destination.airport_code = Flight.airport_code " +
            "WHERE Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
            "ORDER BY Flight.flight_id DESC) " +
            "ORDER BY Flight.price ASC";

        private const string SQL_GetTripsDefault = "SELECT DISTINCT TOP 50 Destination.name, " +
            "Surf.spot_name, Destination.latitude, Destination.longitude, Flight.departure_date, Flight.return_date, " +
            "MIN(Flight.price) AS min_flight_price, " +
            "COUNT(Surf.log_id) AS num_of_days_with_surf, " +
            "ROUND(MAX(Surf.swell_height_feet), 0) AS max_height FROM Flight " +
            "JOIN Destination ON Flight.airport_code = Destination.airport_code " +
            "JOIN Surf ON Destination.location_id = Surf.location_id " +
            "WHERE swell_height_feet > 5 AND price < 500" +
            "AND Surf.forecast_for_date > Flight.departure_date " +
            "AND Surf.forecast_for_date < Flight.return_date " +
            "AND Surf.log_id IN (SELECT TOP 80 Surf.log_id FROM Surf " +
            "ORDER BY Surf.log_id DESC) " +
            "AND Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
            "ORDER BY Flight.flight_id DESC) " +
            "GROUP BY Destination.name, " +
            "Surf.spot_name, Flight.departure_date, Flight.return_date, " +
            "Destination.latitude, Destination.longitude " +
            "ORDER BY min_flight_price ASC;";

        private const string SQL_GetTripsFromInput = "SELECT DISTINCT Destination.name, " +
            "Surf.spot_name, Destination.latitude, Destination.longitude, Flight.departure_date, Flight.return_date, " +
            "MIN(Flight.price) AS min_flight_price, " +
            "COUNT(Surf.log_id) AS num_of_days_with_surf, " +
            "ROUND(MAX(Surf.swell_height_feet), 0) AS max_height FROM Flight " +
            "JOIN Destination ON Flight.airport_code = Destination.airport_code " +
            "JOIN Surf ON Destination.location_id = Surf.location_id " +
            "WHERE swell_height_feet > @inputMinSurfHeight " +
            "AND Flight.departure_date = @inputDepartureDate " +
            "AND Flight.return_date = @inputReturnDate " +
            "AND Surf.forecast_for_date > Flight.departure_date " +
            "AND Surf.forecast_for_date < Flight.return_date " +
            "AND price < @inputMaxPrice " +
            "AND Surf.log_id IN (SELECT TOP 80 Surf.log_id FROM Surf " +
            "ORDER BY Surf.log_id DESC) " +
            "AND Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
            "ORDER BY Flight.flight_id DESC) " +
            "GROUP BY Destination.name, " +
            "Surf.spot_name, Flight.departure_date, Flight.return_date, " +
            "Destination.latitude, Destination.longitude " +
            "ORDER BY min_flight_price ASC;";

        private const string SQL_GetTripsFromInputNoDates = "SELECT DISTINCT Destination.name, " +
            "Surf.spot_name, Destination.latitude, Destination.longitude, Flight.departure_date, Flight.return_date, " +
            "MIN(Flight.price) AS min_flight_price, " +
            "COUNT(Surf.log_id) AS num_of_days_with_surf, " +
            "ROUND(MAX(Surf.swell_height_feet), 0) AS max_height FROM Flight " +
            "JOIN Destination ON Flight.airport_code = Destination.airport_code " +
            "JOIN Surf ON Destination.location_id = Surf.location_id " +
            "WHERE swell_height_feet > @inputMinSurfHeight " +
            "AND Surf.forecast_for_date > @inputDepartureDate " +
            "AND Surf.forecast_for_date < @inputReturnDate " +
            "AND price < @inputMaxPrice " +
            "AND Surf.log_id IN (SELECT TOP 80 Surf.log_id FROM Surf " +
            "ORDER BY Surf.log_id DESC) " +
            "AND Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
            "ORDER BY Flight.flight_id DESC) " +
            "GROUP BY Destination.name, " +
            "Surf.spot_name, Flight.departure_date, Flight.return_date, " +
            "Destination.latitude, Destination.longitude " +
            "ORDER BY min_flight_price ASC;";




        #endregion

        public TripSqlDal(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Trip> GetTripsFromInput(int minSurfHeight, DateTime departureDate, DateTime returnDate, int maxPrice)
        {
            List<Trip> trips = new List<Trip>();

            //if no maxPrice is set (minSurfHeight will automatically be set to 0
            if (maxPrice == 0)
            {
                maxPrice = 99999;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    if (departureDate.Day == Utility.GetPastDate().Day)
                    {
                        cmd = new SqlCommand(SQL_GetTripsFromInputNoDates, conn);
                    }
                    else 
                    {
                        cmd = new SqlCommand(SQL_GetTripsFromInput, conn);
                    }
                                        
                    cmd.Parameters.AddWithValue("@inputMinSurfHeight", minSurfHeight);
                    cmd.Parameters.AddWithValue("@inputDepartureDate", departureDate);
                    cmd.Parameters.AddWithValue("@inputReturnDate", returnDate);
                    cmd.Parameters.AddWithValue("@inputMaxPrice", maxPrice);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                     
                        Trip t = new Trip();
                        t.DepartureDate = Convert.ToDateTime(reader["departure_date"]);
                        t.InputDepartureDate = departureDate;
                        t.InputMaxPrice = maxPrice;
                        t.InputMinSurfHeight = minSurfHeight;
                        t.InputReturnDate = returnDate;
                        t.Latitude = Convert.ToDouble(reader["latitude"]);
                        t.LocationName = Convert.ToString(reader["name"]);
                        t.Longitude = Convert.ToDouble(reader["longitude"]);
                        t.MaxSurfHeight = Convert.ToDecimal(reader["max_height"]);
                        t.NumberOfSurfDays = Convert.ToInt32(reader["num_of_days_with_surf"]);
                        t.Price = Convert.ToDecimal(reader["min_flight_price"]);
                        t.ReturnDate = Convert.ToDateTime(reader["return_date"]).Date;
                        t.SpotName = Convert.ToString(reader["spot_name"]);

                        trips.Add(t);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return trips;
        }

        //no set return date or
        //no set departure date
        public List<Trip> GetTripsFromInput(int minSurfHeight, DateTime date, bool isDeparture, int maxPrice)
        {
            List<Trip> trips = new List<Trip>();

            //if no maxPrice is set (minSurfHeight will automatically be set to 0
            if (maxPrice == 0)
            {
                maxPrice = 99999;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetTripsFromInput, conn);
                    
                    cmd.Parameters.AddWithValue("@inputMinSurfHeight", minSurfHeight);
                    if (isDeparture)
                    {
                        cmd.Parameters.AddWithValue("@inputDepartureDate", date);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@inputReturnDate", date);
                    }                
                    cmd.Parameters.AddWithValue("@inputMaxPrice", maxPrice);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Trip t = new Trip();
                        t.DepartureDate = Convert.ToDateTime(reader["Flight.departure_date"]);
                        if (isDeparture)
                        {
                            t.InputDepartureDate = date;
                        }
                        else
                        {
                            t.InputReturnDate = date;
                        }                      
                        t.InputMaxPrice = maxPrice;
                        t.InputMinSurfHeight = minSurfHeight;
                        t.Latitude = Convert.ToDouble(reader["latitude"]);
                        t.LocationName = Convert.ToString(reader["Destination.name"]);
                        t.Longitude = Convert.ToDouble(reader["longitude"]);
                        t.MaxSurfHeight = Convert.ToDecimal(reader["max_height"]);
                        t.NumberOfSurfDays = Convert.ToInt32(reader["num_of_days_with_surf"]);
                        t.Price = Convert.ToDecimal(reader["min_flight_price"]);
                        t.ReturnDate = Convert.ToDateTime(reader["Flight.return_date"]).Date;
                        t.SpotName = Convert.ToString(reader["Spot.spot_name"]);

                        trips.Add(t);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return trips;
        }

        //trips with default input values (5 ft surf height and 500 max price)
        public List<Trip> GetTripsDefault()
        {
            List<Trip> trips = new List<Trip>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetTripsDefault, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (cmd.Transaction != null)
                    {


                        while (reader.Read())
                        {

                            Trip t = new Trip();
                            t.InputDepartureDate = Utility.GetFutureDate();
                            t.InputMaxPrice = 500;
                            t.InputMinSurfHeight = 5;
                            t.InputReturnDate = Utility.GetFutureDate();
                            t.DepartureDate = Convert.ToDateTime(reader["Flight.departure_date"]).Date;
                            t.Latitude = Convert.ToDouble(reader["latitude"]);
                            t.LocationName = Convert.ToString(reader["Destination.name"]);
                            t.Longitude = Convert.ToDouble(reader["longitude"]);
                            t.MaxSurfHeight = Convert.ToDecimal(reader["max_height"]);
                            t.NumberOfSurfDays = Convert.ToInt32(reader["num_of_days_with_surf"]);
                            t.Price = Convert.ToDecimal(reader["min_flight_price"]);
                            t.ReturnDate = Convert.ToDateTime(reader["Flight.return_date"]).Date;
                            t.SpotName = Convert.ToString(reader["Spot.spot_name"]);

                            trips.Add(t);
                        }
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return trips;
        }

        
        //top cheapest trips from input value
        public List<Trip> GetTopCheapest(int inputNumberToDisplay)
        {
            List<Trip> trips = new List<Trip>();
            //default number to display if no input is given
            if (inputNumberToDisplay <= 0)
            {
                inputNumberToDisplay = 20;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetTopCheapestFromInput, conn);
                    cmd.Parameters.AddWithValue("@numberToDisplay", inputNumberToDisplay);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Trip t = new Trip();
                        t.DepartureDate = Convert.ToDateTime(reader["Flight.departure_date"]).Date;
                        t.Latitude = Convert.ToDouble(reader["latitude"]);
                        t.LocationName = Convert.ToString(reader["Destination.name"]);
                        t.Longitude = Convert.ToDouble(reader["longitude"]);
                        t.MaxSurfHeight = Convert.ToDecimal(reader["max_height"]);
                        t.NumberOfSurfDays = Convert.ToInt32(reader["num_of_days_with_surf"]);
                        t.Price = Convert.ToDecimal(reader["min_flight_price"]);
                        t.ReturnDate = Convert.ToDateTime(reader["Flight.return_date"]).Date;
                        t.SpotName = Convert.ToString(reader["Spot.spot_name"]);

                        trips.Add(t);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return trips;
        }

        //all up-to-date trip options
        public List<Trip> GetAllTrips()
        {
            List<Trip> trips = new List<Trip>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllTrips, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Trip t = new Trip();
                        t.DepartureDate = Convert.ToDateTime(reader["Flight.departure_date"]).Date;
                        t.Latitude = Convert.ToDouble(reader["latitude"]);
                        t.LocationName = Convert.ToString(reader["Destination.name"]);
                        t.Longitude = Convert.ToDouble(reader["longitude"]);
                        t.MaxSurfHeight = Convert.ToDecimal(reader["max_height"]);
                        t.NumberOfSurfDays = Convert.ToInt32(reader["num_of_days_with_surf"]);
                        t.Price = Convert.ToDecimal(reader["min_flight_price"]);
                        t.ReturnDate = Convert.ToDateTime(reader["Flight.return_date"]).Date;
                        t.SpotName = Convert.ToString(reader["Spot.spot_name"]);

                        trips.Add(t);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return trips;
        }
    }
}