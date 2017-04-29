using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CacheManager.Core;

using FindGojeks.Models;
using FindGojeks.Calculation;

namespace FindGojeks.Controllers
{
    [Route("api/[controller]")]
    public class DriversController : Controller
    {
        private readonly ICacheManager<object> cache;
        public DriversController(ICacheManager<object> valuesCache)
        {
            this.cache = valuesCache;
            //Console.WriteLine("Cache Injected:{0}",cache.ToString());
        }
    
        // key to store all available  keys.
        private const string KeysKey = "location-sample-keys";
        // retrieves all  keys or adds an empty int array if the key is not set
        private List<string> AllKeys
        {
            get
            {
                //Console.WriteLine("Start AllKeys");
                var keys = cache.Get<string[]>(KeysKey);
                if (keys == null)
                {    
                    return null;               
                }
                //Console.WriteLine("AllKeys: keys count = {0}",keys.Count());
                return keys.ToList();
            }
        }

        protected Driver Get(string id)
        {
            string obj = cache.Get<string>(id.ToString());
            Driver driver = (Driver)JsonConvert.DeserializeObject(obj,typeof(Driver));
            return driver;
        } 
        
        [HttpGet("{id}",Name="GetAllLocaltion")]
        public IEnumerable<Driver> GetAll()
        { 
            var keys = this.AllKeys;
            if(keys == null)
            {
                yield break;
            }
            //Console.WriteLine("GetAll: keys count = {0}",keys.Count());
            foreach (var key in keys)
            { 
                //Console.WriteLine(key);
                yield return this.Get(key);

            }                                 
        }      
       
       [HttpPost("{id}",Name="init")]
       public IActionResult GeneratingData()
       {
           Distance.GenerateSampleLocation(cache);  
           
           return Ok("Finished to initialize sample location data!");
       }  

        [HttpGet]
        public IActionResult FindDrivers(Driver mypos)
        {            
            //Console.WriteLine("Strat FindDrivers : Lat:{0},Lon:{1},Rad:{2},Lim:{3}",mypos.Latitude,mypos.Longitude,mypos.Radius,mypos.Limit);

            if(mypos == null) return BadRequest();

            if((mypos.Latitude < -90 || mypos.Latitude > 90))
            {
                return BadRequest("{\"errors\": \"Latitude should be between +/- 90\"}");
            }            
            
            List<Driver> foundDrivers = new List<Driver>();
            var keys = this.AllKeys;
            if(keys == null)
            {
                return BadRequest("{\"errors\": \"No Sample Data!!\"}");
            }

            foreach (var key in keys)
            {
                Driver driver = this.Get(key);
                double distance = Distance.CalculateDistance(mypos.Latitude, mypos.Longitude, driver.Latitude, driver.Longitude);           
                if(distance <= mypos.Radius && foundDrivers.Count() < mypos.Limit)
                {
                    driver.Distance = Convert.ToInt32(distance);
                    foundDrivers.Add(driver);
                }
                else
                {
                    break;
                }
            }                                

            return Ok(foundDrivers);  
              
        }        
    }
}