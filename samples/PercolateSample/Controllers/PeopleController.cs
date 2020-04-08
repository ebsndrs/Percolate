using Microsoft.AspNetCore.Mvc;
using Percolate.Attributes;
using PercolateSample.Data;
using System.Linq;

namespace PercolateSample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly SampleDbContext dbContext;

        public PeopleController(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpGet]
        [EnablePercolate]
        public IActionResult Get()
        {
            var people = dbContext.People.ToList();

            return Ok(people);
        }
    }
}
