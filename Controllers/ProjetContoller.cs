namespace GestionProAPI.Controllers
{
    using GestionProAPI.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class ProjetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjets()
        {
            return Ok(await _context.Projets.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjet(int id)
        {
            var projet = await _context.Projets.FindAsync(id);
            if (projet == null) return NotFound();
            return Ok(projet);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjet([FromBody] Projet projet)
        {
            _context.Projets.Add(projet);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjet), new { id = projet.Id }, projet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjet(int id, [FromBody] Projet projet)
        {
            if (id != projet.Id) return BadRequest();

            _context.Entry(projet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjet(int id)
        {
            var projet = await _context.Projets.FindAsync(id);
            if (projet == null) return NotFound();

            _context.Projets.Remove(projet);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
