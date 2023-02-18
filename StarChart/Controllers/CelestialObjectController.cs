using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute]int id)
        {
            var result = _context.CelestialObjects.FirstOrDefault(i => i.Id == id);

            if(result == null)
            {
                return NotFound();
            }

            var result1 = _context.CelestialObjects.Where(i => i.OrbitedObjectId == result.Id).ToList();

            result.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
            result.Satellites.AddRange(result1);

            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([FromRoute]string name)
        {
            var result = _context.CelestialObjects.Where(i => i.Name == name).ToList();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            foreach (var obj in result)
            {
                obj.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
                obj.Satellites.AddRange(_context.CelestialObjects.Where(i => i.OrbitedObjectId == obj.Id).ToList());
            }

            return Ok(result);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects;

            foreach(var obj in result)
            {
                obj.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
                obj.Satellites.AddRange(result.Where(i => i.OrbitedObjectId == obj.Id).ToList());
            }

            return Ok(result);
        }
    }
}
