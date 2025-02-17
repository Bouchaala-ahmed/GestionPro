namespace GestionProAPI.Controllers
{
    using GestionProAPI.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class RessourcesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RessourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRessources()
        {
            return Ok(await _context.Ressources.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRessource(int id)
        {
            var ressource = await _context.Ressources.FindAsync(id);
            if (ressource == null) return NotFound();
            return Ok(ressource);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRessource([FromBody] Ressource ressource)
        {
            _context.Ressources.Add(ressource);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRessource), new { id = ressource.Id }, ressource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRessource(int id, [FromBody] Ressource ressource)
        {
            if (id != ressource.Id) return BadRequest();

            _context.Entry(ressource).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRessource(int id)
        {
            var ressource = await _context.Ressources.FindAsync(id);
            if (ressource == null) return NotFound();

            _context.Ressources.Remove(ressource);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
