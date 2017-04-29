using System;
using System.Linq;
using Newtonsoft.Json;

namespace FindGojeks.Models
{
   
    public class Driver
    {        
       
        public int Id { get; set; }
       
        public double Longitude { get; set; }
      
        public double Latitude  { get; set; }
      
        public int Radius { get; set; }
     
        public int Distance { get; set; }       
        public int Limit { get; set; }     
    }
}