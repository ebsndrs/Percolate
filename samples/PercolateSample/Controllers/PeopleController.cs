using Microsoft.AspNetCore.Mvc;
using Percolate.Attributes;
using PercolateSample.Models;
using System;

namespace PercolateSample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        [HttpGet]
        [EnablePercolate]
        public IActionResult Get()
        {
            return Ok(people);
        }

        private readonly Person[] people = new Person[]
        {
            new Person { Name = "Cory", Age = 52, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Rosie", Age = 13, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Josiah", Age = 71, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Josie", Age = 54, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Fred", Age = 24, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Scarlet", Age = 26, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Keaton", Age = 74, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Freya", Age = 34, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Alistair", Age = 84, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Amanda", Age = 42, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Luke", Age = 69, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Ellie", Age = 16, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Harris", Age = 11, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Vannesa", Age = 74, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Jesse", Age = 26, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Kate", Age = 28, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Laurence", Age = 83, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Kayla", Age = 72, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Liam", Age = 5, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Nina", Age = 33, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Gary", Age = 57, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Nancy", Age = 23, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Theodore", Age = 12, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Ashley", Age = 71, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Hugh", Age = 65, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Poppy", Age = 45, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Byron", Age = 19, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Michelle", Age = 29, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Felix", Age = 36, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Ruby", Age = 93, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Frazer", Age = 31, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Esme", Age = 50, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Leo", Age = 40, CreatedDateTime = DateTime.UtcNow },
            new Person { Name = "Emily", Age = 20, CreatedDateTime = DateTime.UtcNow },

        };
    }
}
