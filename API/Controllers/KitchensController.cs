using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchensController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public KitchensController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Kitchens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kitchen>>> GetKitchens()
        {
            return await _context.Kitchens.ToListAsync();
        }

        // GET: api/Kitchens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kitchen>> GetKitchen(Guid id)
        {
            var kitchen = await _context.Kitchens.FindAsync(id);

            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            return kitchen;
        }

        // PUT: api/Kitchens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKitchen(Guid id, Kitchen kitchen)
        {
            var existingKitchen = await _context.Kitchens.FindAsync(id);

            if (existingKitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            existingKitchen.Name = kitchen.Name;
            existingKitchen.Description = kitchen.Description;
            existingKitchen.ImageURL = kitchen.ImageURL;
            existingKitchen.Category = kitchen.Category;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KitchenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingKitchen);
        }

        // POST: api/Kitchens
        [HttpPost]
        public async Task<ActionResult<Kitchen>> PostKitchen(Kitchen kitchen)
        {
            // check if vendor exists
            var vendor = await _context.Vendors.FindAsync(kitchen.VendorId);

            if (vendor == null)
            {
                return BadRequest("Vendor not found.");
            }

            // check if kitchen already exists
            var existingKitchen = await _context.Kitchens.FirstOrDefaultAsync(c => c.Name == kitchen.Name);

            if (existingKitchen != null)
            {
                return BadRequest("Kitchen already exists.");
            }


            kitchen.Vendor = vendor;
            _context.Kitchens.Add(kitchen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKitchen", new { id = kitchen.Id }, kitchen);
        }


        // DELETE: api/Kitchens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKitchen(Guid id)
        {
            var kitchen = await _context.Kitchens.FindAsync(id);
            if (kitchen == null)
            {
                return NotFound();
            }

            _context.Kitchens.Remove(kitchen);
            await _context.SaveChangesAsync();

            return Ok("Kitchen deleted.");
        }

        private bool KitchenExists(Guid id)
        {
            return _context.Kitchens.Any(e => e.Id == id);
        }
    }
}
