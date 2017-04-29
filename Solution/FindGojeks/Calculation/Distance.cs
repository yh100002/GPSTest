using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using CacheManager.Core;
using FindGojeks.Models;

namespace FindGojeks.Calculation
{   
    public class Distance
    {

        public static void GenerateSampleLocation(ICacheManager<object> cache)
        {            
            var KeysKey = "location-sample-keys";
            List<string> keys = new List<string>();
            Random random = new Random();
            for (int i = 0; i < 50000; i++)
            {
                int lat = random.Next(516400146, 630304598); 
                int lon = random.Next(224464416, 341194152);  
                double latitude = Convert.ToDouble("-6." + lat);
                double longitude = Convert.ToDouble("120." + lon); 
                string row = String.Format(@"{{""id"":{0},""latitude"":{1},""longitude"":{2}}}", i,latitude,longitude);     
                cache.Add(i.ToString(), row); 
                keys.Add(i.ToString());
                //Console.WriteLine(row);
            }
            
            if(cache.Add(KeysKey,keys.ToArray()) == false)
            {
                Console.WriteLine("Failed to Add Keys");
            }
            else
            {
                var temp = cache.Get<string[]>(KeysKey);
                Console.WriteLine("Succeed to Add Keys : {0}",temp.Count());                
            }   

        }
        
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double circumference = 40000.0 * 1000; // Earth's circumference at the equator in meter
            double distance = 0.0;

            //Calculate radians
            double latitude1Rad = deg2rad(lat1);
            double longitude1Rad = deg2rad(lon1);
            double latititude2Rad = deg2rad(lat2);
            double longitude2Rad = deg2rad(lon2);

            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }
      
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
     
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}