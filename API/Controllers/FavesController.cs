using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fave>>> GetFaves(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var faves = await _context.Faves
                .Where(f => f.CustomerId == customerId)
                .ToListAsync();

            return faves;
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

        // get kitchen faves
        [HttpGet("~/api/kitchens/{kitchenId}/faves")]
        public async Task<ActionResult<IEnumerable<Fave>>> GetKitchenFaves(Guid kitchenId)
        {
            var kitchen = await _context.Kitchens.FindAsync(kitchenId);

            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            var faves = await _context.Faves
                .Where(f => f.KitchenId == kitchenId)
                .ToListAsync();

            return faves;
        }

        // get kitchen fave
        [HttpGet("~/api/kitchens/{kitchenId}/faves/{id}")]
        public async Task<ActionResult<Fave>> GetKitchenFave(Guid kitchenId, Guid id)
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
        public async Task<IActionResult> PutFave(Guid id, Guid customerId, Fave fave)
        {
            var existingFave = await _context.Faves.FindAsync(id);

            if (existingFave == null)
            {
                return NotFound("Fave not found.");
            }

            // Check if the customer exists
            var customer = await _context.Customers.FindAsync(existingFave.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // Check if the kitchen exists
            var kitchen = await _context.Kitchens.FindAsync(fave.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            existingFave.FaveProducts = fave.FaveProducts;
            existingFave.KitchenId = fave.KitchenId;
            existingFave.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingFave);
        }

        // POST: api/Faves
        [HttpPost]
        public async Task<ActionResult<Fave>> PostFave(Fave fave)
        {
            var customer = await _context.Customers.FindAsync(fave.CustomerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }


            var kitchen = await _context.Kitchens.FindAsync(fave.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            // check if the fave already exists
            var existingFave = await _context.Faves
                .Where(f => f.CustomerId == fave.CustomerId && f.KitchenId == fave.KitchenId)
                .FirstOrDefaultAsync();

            if (existingFave != null)
            {
                foreach (var faveProduct in fave.FaveProducts)
                {
                    var product = await _context.Products.FindAsync(faveProduct.ProductId);
                    if (product == null)
                    {
                        return NotFound("Product not found.");
                    }
                    else
                    {
                        // check if the product is already in the fave
                        var existingFaveProduct = existingFave.FaveProducts.FirstOrDefault(cp => cp.ProductId == faveProduct.ProductId);
                        if (existingFaveProduct != null)
                        {
                            // remove the product from the fave
                            existingFave.FaveProducts.Remove(existingFaveProduct);
                        }
                        else
                        {
                            // add the product to the fave
                            if (existingFave.FaveProducts == null)
                            {
                                existingFave.FaveProducts = new List<FaveProduct>();
                            }
                            existingFave.FaveProducts.Add(faveProduct);
                        }
                        faveProduct.Product = product;
                    }
                    await _context.SaveChangesAsync();
                }
                _context.Entry(existingFave).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(existingFave);
            }

            // if the fave doesn't exist, create a new one
            fave.CreatedAt = DateTime.Now;
            _context.Faves.Add(fave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFave", new { customerId = fave.CustomerId, id = fave.Id }, fave);

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
    }
}
