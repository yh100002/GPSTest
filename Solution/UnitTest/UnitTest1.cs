using System;
using Xunit;
using FindGojeks.Models;
using FindGojeks.Controllers;
using FindGojeks.Calculation;
using CacheManager.Core;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest
{
    public class UnitTest1
    {
        
        [Fact]
        public void GeneratingDataTest()
        {
            // Arrange
            ICacheManager<object> cache; cache = CreateCache();
            Distance.GenerateSampleLocation(cache);            
            DriversController drivers = new DriversController(cache);
            // Act
            var result = drivers.GeneratingData();
            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);            
        }

        [Fact]
        public void FindingDriverExceptionTest()
        {
            // Arrange           
            ICacheManager<object> cache; cache = CreateCache(); 
            DriversController drivers = new DriversController(cache);
            Driver dummy = new Driver();
            dummy.Latitude = -91;                   
            // Act
            var result1 = drivers.FindDrivers(dummy); 

            //Assert
            var viewResult = Assert.IsType<BadRequestObjectResult>(result1); 
        }

        [Fact]
        public void FindDriverTest()
        {
            //Arrange
            Driver mypos = new Driver();
            mypos.Latitude = -6.5;
            mypos.Longitude = 120.0;
            mypos.Radius = 50000;
            mypos.Limit = 1000;
            Console.WriteLine("Start FindDriversTest : Lat:{0},Lon:{1},Rad:{2},Lim:{3}",mypos.Latitude,mypos.Longitude,mypos.Radius,mypos.Limit);
            ICacheManager<object> cache = CreateCache();
            Distance.GenerateSampleLocation(cache);            
            DriversController drivers = new DriversController(cache);

            //Act
            var result = drivers.FindDrivers(mypos) as OkObjectResult;
            dynamic jsonCollection = result.Value;  
            int count = 0;          
            foreach (dynamic value in jsonCollection) 
            {
                //dynamic json = new DynamicObjectResultValue(value);
                count++; //just for test                
            }

            //Assert
            Assert.IsType<OkObjectResult>(result);            
            Assert.InRange(count,0,mypos.Limit);
        }
       

        ICacheManager<object> CreateCache()
        {
            var cache = CacheFactory.Build(settings =>
            {
                settings
                    .WithUpdateMode(CacheUpdateMode.Up)
                    .WithDictionaryHandle()
                    .EnablePerformanceCounters()
                    .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(10));
            });
            return cache;
        }   

    }

    
}
