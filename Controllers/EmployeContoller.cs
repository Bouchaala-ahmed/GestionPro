namespace GestionProAPI.Controllers
{
    using GestionProAPI.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployes()
        {
            return Ok(await _context.Employes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmploye(int id)
        {
            var employe = await _context.Employes.FindAsync(id);
            if (employe == null) return NotFound();
            return Ok(employe);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmploye([FromBody] Employe employe)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Employes.Add(employe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmploye), new { id = employe.Id }, employe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmploye(int id, [FromBody] Employe employe)
        {
            if (id != employe.Id) return BadRequest();

            _context.Entry(employe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmploye(int id)
        {
            var employe = await _context.Employes.FindAsync(id);
            if (employe == null) return NotFound();

            _context.Employes.Remove(employe);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
