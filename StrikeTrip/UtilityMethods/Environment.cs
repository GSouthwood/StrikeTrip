using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace StrikeTrip.UtilityMethods
{
    public static class Environment
    {
        /// <summary>
        /// Determines SQL Server connection string based on host name
        /// </summary>
        /// <returns>List of strings with two entries
        /// [0] is connection string
        /// [1] is message describing connection and how it was determined
        /// </returns>

        public static List<string> GetConnectionString()
        {
            //determines connection string based on system (server) name
            //defaults to Dev

            string machineName = System.Environment.MachineName;
            List<string> result = new List<string>();

            if (ConfigurationManager.AppSettings["ProdServers"].Contains(machineName))
            {
                //AppHarbor
                result.Add(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]);
                result.Add(machineName + " is a production server.");
            }
            else if (machineName.Substring(0, 3) == "IP-")
            {
                // pattern match on AppHarbor
                result.Add(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]);
                result.Add(machineName + " is a production server based on a pattern match for 'IP-'.");
            }
            else if (ConfigurationManager.AppSettings["DevServers"].Contains(machineName))
            {
                // local
                result.Add(ConfigurationManager.ConnectionStrings["ScraperDatabase"].ConnectionString);
                result.Add(machineName + " is a development server.");
            }
            else
            {
                // local by default
                result.Add(ConfigurationManager.ConnectionStrings["ScraperDatabase"].ConnectionString);
                result.Add(machineName + " was not found in list. Defaulted to Dev environment");
            }

            return result;
        }
    }

}