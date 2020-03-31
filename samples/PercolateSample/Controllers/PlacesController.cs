using Microsoft.AspNetCore.Mvc;
using Percolate.Attributes;
using PercolateSample.Models;
using System;

namespace PercolateSample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        [HttpGet]
        [EnablePercolate]
        public IActionResult Get()
        {
            return Ok(places);
        }

        private readonly Place[] places = new Place[]
        {
            new Place
            {
                Name = "Pittsburgh",
                CanVisit = true,
                Details = new PlaceDetails
                {
                    Country = "USA",
                    Coordinates = new Coordinates { X = 72, Y = 34}
                }
            },
            new Place
            {
                Name = "London",
                CanVisit = false,
                Details = new PlaceDetails
                {
                    Country = "England",
                    Coordinates = new Coordinates { X = 42, Y = 12}
                }
            },
            new Place
            {
                Name = "Beijing",
                CanVisit = true,
                Details = new PlaceDetails
                {
                    Country = "China",
                    Coordinates = new Coordinates { X = 32, Y = 66}
                }
            },
            new Place
            {
                Name = "New York",
                CanVisit = false,
                Details = new PlaceDetails
                {
                    Country = "USA",
                    Coordinates = new Coordinates { X = 85, Y = 10}
                }
            },
            new Place
            {
                Name = "Antarctic City",
                CanVisit = true,
                Details = new PlaceDetails
                {
                    Country = "Antarctica",
                    Coordinates = new Coordinates { X = 184, Y = 193}
                }
            }

        };
    }
}
