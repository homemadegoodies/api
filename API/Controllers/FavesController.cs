using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/customers/{customerId}/[controller]")]
    [ApiController]
    public class FavesController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public FavesController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Faves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fave>>> GetFaves(Guid customerId)
        {
            return await _context.Faves.Where(f => f.CustomerId == customerId).ToListAsync();
        }

        // GET: api/Faves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fave>> GetFave(Guid id)
        {
            var fave = await _context.Faves.FindAsync(id);

            if (fave == null)
            {
                return NotFound("Fave not found.");
            }

            return fave;
        }

        // PUT: api/Faves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFave(Guid id, Fave fave)
        {
            // check if the fave exists
            var existingFave = await _context.Faves.FindAsync(id);

            if (existingFave == null)
            {
                return NotFound("Fave not found.");
            }

            // check if the customer exists
            var customer = await _context.Customers.FindAsync(existingFave.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // check if the fave Title already exists
            // var existingFaveTitle = await _context.Faves.Where(f => f.Title == fave.Title && f.CustomerId == customer.Id).FirstOrDefaultAsync();
            // if (existingFaveTitle != null)
            // {
            //     return BadRequest("Fave already exists.");
            // }

            // update the fave
            existingFave.FaveProducts = fave.FaveProducts;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaveExists(id))
                {
                    return NotFound("Fave not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingFave);
        }

        // POST: api/Faves
        [HttpPost]
        public async Task<ActionResult<Fave>> PostFave(Guid customerId, [FromBody] Fave fave)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            // check if customer exists
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // check if fave already exists
            var existingFave = await _context.Faves.Where(f => f.FaveProducts == fave.FaveProducts && f.CustomerId == customerId).FirstOrDefaultAsync();
            if (existingFave != null)
            {
                return BadRequest("Fave already exists.");
            }

            // set the customerId of the new fave
            fave.CustomerId = customerId;

            // add fave to customer and save changes
            _context.Faves.Add(fave);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFave), new { customerId = customerId, id = fave.Id }, fave);
        }

        // DELETE: api/Faves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFave(Guid id)
        {
            var fave = await _context.Faves.FindAsync(id);
            if (fave == null)
            {
                return NotFound("Fave not found.");
            }

            _context.Faves.Remove(fave);
            await _context.SaveChangesAsync();

            return Ok("Fave deleted.");
        }

        private bool FaveExists(Guid id)
        {
            return _context.Faves.Any(e => e.Id == id);
        }
    }
}
