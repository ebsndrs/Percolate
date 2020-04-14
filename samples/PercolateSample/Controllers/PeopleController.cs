using Microsoft.AspNetCore.Mvc;
using Percolate;
using Percolate.Extensions;
using Percolate.Models;
using PercolateSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PercolateSample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPercolateService percolateService;

        public PeopleController(IPercolateService percolateService)
        {
            this.percolateService = percolateService;
        }

        [HttpGet]
        //[EnablePercolate]
        public IActionResult Get([FromQuery] QueryModel query)
        {
            var people = new List<Person>() {
                new Person { Id = 1, Name = "Cory", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 1, Zip = "15203", Years = 32 }},
                new Person { Id = 2, Name = "Rosie", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 2, Zip = "51612", Years = 15 } },
                new Person { Id = 3, Name = "Josiah", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 3, Zip = "89492", Years = 6 } },
                new Person { Id = 4, Name = "Josie", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 4, Zip = "48104", Years = 21 } },
                new Person { Id = 5, Name = "Fred", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 5, Zip = "48108", Years = 2 } },
                new Person { Id = 6, Name = "Scarlet", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 6, Zip = "10680", Years = 4 } },
                new Person { Id = 7, Name = "Keaton", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 7, Zip = "51671", Years = 6 } },
                new Person { Id = 8, Name = "Freya", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 8, Zip = "41526", Years = 45 } },
                new Person { Id = 9, Name = "Alistair", Age = 84, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 9, Zip = "47250", Years = 2 } },
                new Person { Id = 10, Name = "Amanda", Age = 42, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 10, Zip = "14239", Years = 14 } },
                new Person { Id = 11, Name = "Luke", Age = 69, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 11, Zip = "15178", Years = 1 } },
                new Person { Id = 12, Name = "Ellie", Age = 16, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 12, Zip = "74804", Years = 46 } },
                new Person { Id = 13, Name = "Harris", Age = 11, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 13, Zip = "94811", Years = 2 } },
                new Person { Id = 14, Name = "Vannesa", Age = 74, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 14, Zip = "90210", Years = 42 } },
                new Person { Id = 15, Name = "Jesse", Age = 26, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 15, Zip = "41671", Years = 35 } },
                new Person { Id = 16, Name = "Kate", Age = 28, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 16, Zip = "41466", Years = 7 } },
                new Person { Id = 17, Name = "Laurence", Age = 83, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 17, Zip = "11233", Years = 2 } },
                new Person { Id = 18, Name = "Kayla", Age = 72, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 18, Zip = "78362", Years = 4 } },
                new Person { Id = 19, Name = "Liam", Age = 5, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 19, Zip = "94843", Years = 7 } },
                new Person { Id = 20, Name = "Nina", Age = 33, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 20, Zip = "46425", Years = 4 } },
                new Person { Id = 21, Name = "Gary", Age = 57, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 21, Zip = "34574", Years = 1 } },
                new Person { Id = 22, Name = "Nancy", Age = 23, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 22, Zip = "45853", Years = 2 } },
                new Person { Id = 23, Name = "Theodore", Age = 12, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 23, Zip = "34673", Years = 4 } },
                new Person { Id = 24, Name = "Ashley", Age = 71, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 24, Zip = "42617", Years = 7 } },
                new Person { Id = 25, Name = "Hugh", Age = 65, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 25, Zip = "17153", Years = 10 } },
                new Person { Id = 26, Name = "Poppy", Age = 45, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 26, Zip = "42568", Years = 13 } },
                new Person { Id = 27, Name = "Byron", Age = 19, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 27, Zip = "89536", Years = 15 } },
                new Person { Id = 28, Name = "Michelle", Age = 29, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 28, Zip = "24545", Years = 9 } },
                new Person { Id = 29, Name = "Felix", Age = 36, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 29, Zip = "14235", Years = 5 } },
                new Person { Id = 30, Name = "Ruby", Age = 93, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 30, Zip = "25278", Years = 8 } },
                new Person { Id = 31, Name = "Frazer", Age = 31, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 31, Zip = "25243", Years = 5 } },
                new Person { Id = 32, Name = "Esme", Age = 50, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 32, Zip = "24368", Years = 14 } },
                new Person { Id = 33, Name = "Leo", Age = 40, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 33, Zip = "24236", Years = 13 } },
                new Person { Id = 34, Name = "Emily", Age = 20, CreatedDateTime = DateTime.UtcNow, Address = new Address { Id = 34, Zip = "52341", Years = 12 } }
            };


            people = people.AsQueryable().ApplyQuery(query, percolateService).ToList();

            return Ok(people);
        }
    }
}
