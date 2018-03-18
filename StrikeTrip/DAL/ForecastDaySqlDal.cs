using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StrikeTrip.Models;
using System.Data.SqlClient;
using StrikeTrip.UtilityMethods;

namespace StrikeTrip.DAL
{
    public class ForecastDaySqlDal
    {
        private string connectionString;

        #region SQL commands
        private const string SQL_GetAvgSurfHeightsDefaults = "SELECT TOP 30 Surf.spot_name, AVG(Surf.swell_height_feet) AS historical_avg_surf_height " +
        "FROM Surf GROUP BY Surf.spot_name";

        private const string SQL_GetForecastDaysFromId = "SELECT Surf.spot_name, forecast_for_date, Destination.name, " +
            "swell_height_feet, wind_direction FROM Surf " +
            "JOIN Destination ON Destination.location_id = Surf.location_id " +
            "WHERE Surf.spot_name = @spotName AND log_id IN " +
            "(SELECT TOP 80 log_id FROM SURF ORDER BY log_id DESC) " +
            "ORDER BY forecast_for_date ASC;";

        //private const string SQL_GetForecastDaysFromInput = "SELECT DISTINCT TOP 20 Destination.name, Surf.spot_name, Surf.forecast_for_date, Surf.wind_direction, " +
        //"ROUND(Surf.swell_height_feet, 0) AS avg_surf_height, Surf.log_date, " +
        //"COUNT(Flight.flight_id) AS num_of_flights FROM Surf " +
        //"JOIN Destination ON Destination.location_id = Surf.location_id " +
        //"JOIN Flight ON Flight.airport_code = Destination.airport_code " +
        //"WHERE swell_height_feet > @inputMinSurfHeight " +
        //"AND Flight.departure_date = @inputDepartureDate " +
        //"AND Flight.return_date = @inputReturnDate " +
        //"AND Surf.forecast_for_date > Flight.departure_date " +
        //"AND Surf.forecast_for_date<Flight.return_date " +
        //"AND price < @inputMaxPrice " +
        //"AND Surf.log_id IN (SELECT TOP 80 Surf.log_id FROM Surf " +
        //"ORDER BY Surf.log_id DESC) " +
        //"AND Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
        //"ORDER BY Flight.flight_id DESC) " +
        //"GROUP BY Destination.name, " +
        //"Surf.spot_name, Surf.forecast_for_date, Surf.swell_height_feet, Surf.log_date, Surf.wind_direction " +
        //"ORDER BY Surf.log_date";

        //private const string SQL_GetForecastDaysDefault = "SELECT DISTINCT TOP 20 Destination.name, Surf.spot_name, Surf.forecast_for_date, Surf.wind_direction, " +
        //"ROUND(Surf.swell_height_feet, 0) AS avg_surf_height, Surf.log_date, " +
        //"COUNT(Flight.flight_id) AS num_of_flights FROM Surf " +
        //"JOIN Destination ON Destination.location_id = Surf.location_id " +
        //"JOIN Flight ON Flight.airport_code = Destination.airport_code " +
        //"WHERE swell_height_feet > 5 " +
        //"AND Surf.forecast_for_date > Flight.departure_date " +
        //"AND Surf.forecast_for_date < Flight.return_date " +
        //"AND price < 500 " +
        //"AND Surf.log_id IN (SELECT TOP 80 Surf.log_id FROM Surf " +
        //"ORDER BY Surf.log_id DESC) " +
        //"AND Flight.flight_id IN (SELECT TOP 96 Flight.flight_id FROM Flight " +
        //"ORDER BY Flight.flight_id DESC) " +
        //"GROUP BY Destination.name, " +
        //"Surf.spot_name, Surf.forecast_for_date, Surf.swell_height_feet, Surf.log_date, Surf.wind_direction " +
        //"ORDER BY Surf.log_date";

        #endregion

        public ForecastDaySqlDal(string connectionString)
        {
            this.connectionString = connectionString;
        }


        ////from input
        //public List<ForecastDay> GetForecastDaysFromInput(int minSurfHeight, DateTime departureDate, DateTime returnDate, int maxPrice)
        //{
        //    List<ForecastDay> forecastDays = new List<ForecastDay>();

        //    //if no maxPrice is set (minSurfHeight will automatically be set to 0
        //    if (maxPrice == 0)
        //    {
        //        maxPrice = 99999;
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(SQL_GetForecastDaysFromInput, conn);
        //            cmd.Parameters.AddWithValue("@inputMinSurfHeight", minSurfHeight);
        //            cmd.Parameters.AddWithValue("@inputDepartureDate", departureDate);
        //            cmd.Parameters.AddWithValue("@inputReturnDate", returnDate);
        //            cmd.Parameters.AddWithValue("@inputMaxPrice", maxPrice);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                ForecastDay f = new ForecastDay();
        //                f.InputDepartureDate = departureDate;
        //                f.InputMaxPrice = maxPrice;
        //                f.InputMinSurfHeight = minSurfHeight;
        //                f.InputReturnDate = returnDate;
        //                f.LocationName = Convert.ToString(reader["name"]);
        //                f.AverageSurfHeight = Convert.ToDecimal(reader["avg_surf_height"]);
        //                f.NumberOfFlights = Convert.ToInt32(reader["num_of_flights"]);
        //                f.ForecastForDate = Convert.ToDateTime(reader["forecast_for_date"]);
        //                f.SpotName = Convert.ToString(reader["spot_name"]);
        //                f.WindDirection = Convert.ToDecimal(reader["wind_direction"]);

        //                forecastDays.Add(f);
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw;
        //    }

        //    return forecastDays;
        //}

        ////no set return date or
        ////no set departure date
        //public List<ForecastDay> GetForecastDaysFromInput(int minSurfHeight, DateTime date, bool isDeparture, int maxPrice)
        //{
        //    List<ForecastDay> forecastDays = new List<ForecastDay>();

        //    //if no maxPrice is set (minSurfHeight will automatically be set to 0
        //    if (maxPrice == 0)
        //    {
        //        maxPrice = 99999;
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(SQL_GetForecastDaysFromInput, conn);
        //            cmd.Parameters.AddWithValue("@inputMinSurfHeight", minSurfHeight);
        //            if (isDeparture)
        //            {
        //                cmd.Parameters.AddWithValue("@inputDepartureDate", date);
        //            }
        //            else
        //            {
        //                cmd.Parameters.AddWithValue("@inputReturnDate", date);
        //            }
        //            cmd.Parameters.AddWithValue("@inputMaxPrice", maxPrice);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                ForecastDay f = new ForecastDay();
        //                if (isDeparture)
        //                {
        //                    f.InputDepartureDate = date;
        //                }
        //                else
        //                {
        //                    f.InputReturnDate = date;
        //                }
        //                f.InputMaxPrice = maxPrice;
        //                f.InputMinSurfHeight = minSurfHeight;
        //                f.LocationName = Convert.ToString(reader["name"]);
        //                f.AverageSurfHeight = Convert.ToDecimal(reader["avg_surf_height"]);
        //                f.NumberOfFlights = Convert.ToInt32(reader["num_of_flights"]);
        //                f.ForecastForDate = Convert.ToDateTime(reader["forecast_for_date"]);
        //                f.SpotName = Convert.ToString(reader["spot_name"]);
        //                f.WindDirection = Convert.ToDecimal(reader["wind_direction"]);

        //                forecastDays.Add(f);
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw;
        //    }

        //    return forecastDays;
        //}

        ////default 
        //public List<ForecastDay> GetForecastDaysDefault()
        //{
        //    List<ForecastDay> forecastDays = new List<ForecastDay>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(SQL_GetForecastDaysDefault, conn);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                ForecastDay f = new ForecastDay();
        //                f.LocationName = Convert.ToString(reader["name"]);
        //                f.AverageSurfHeight = Convert.ToDecimal(reader["avg_surf_height"]);
        //                f.NumberOfFlights = Convert.ToInt32(reader["num_of_flights"]);
        //                f.ForecastForDate = Convert.ToDateTime(reader["forecast_for_date"]);
        //                f.SpotName = Convert.ToString(reader["spot_name"]);
        //                f.WindDirection = Convert.ToDecimal(reader["wind_direction"]);

        //                forecastDays.Add(f);
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw;
        //    }

        //    return forecastDays;
        //}

        //historical surf height averages

        public List<ForecastDay> GetDetailedForecast(string id)
        {
            List<ForecastDay> forecastDays = new List<ForecastDay>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetForecastDaysFromId, conn);
                    cmd.Parameters.AddWithValue("@spotName", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ForecastDay f = new ForecastDay();
                        f.SpotName = Convert.ToString(reader["spot_name"]);
                        f.ForecastForDate = Convert.ToDateTime(reader["forecast_for_date"]);
                        f.AverageSurfHeight = Convert.ToDecimal(reader["swell_height_feet"]);
                        f.WindDirection = Convert.ToInt32(reader["wind_direction"]);
                        f.LocationName = Convert.ToString(reader["name"]);


                        forecastDays.Add(f);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return forecastDays;
        }

        //public List<ForecastDay> GetAvgSurfHeightsDefaults()
        //{
        //    List<ForecastDay> forecastDays = new List<ForecastDay>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(SQL_GetForecastDaysDefault, conn);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                ForecastDay f = new ForecastDay();
        //                f.AverageSurfHeight = Convert.ToDecimal(reader["historical_avg_surf_height"]);
        //                f.SpotName = Convert.ToString(reader["spot_name"]);


        //                forecastDays.Add(f);
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw;
        //    }

        //    return forecastDays;
        //}
    }
}