using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/kitchens/{kitchenId}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public ProductsController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(Guid kitchenId)
        {
            return await _context.Products.Where(m => m.KitchenId == kitchenId).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            // check if the kitchen exists
            var kitchen = await _context.Kitchens.FindAsync(product.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            // check if the product exists
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            // check if the product title already exists
            var existingProductName = await _context.Products.Where(m => m.KitchenId == product.KitchenId && m.Name == product.Name).FirstOrDefaultAsync();
            if (existingProductName != null && existingProductName.Id != id)
            {
                return BadRequest("Product already exists.");
            }

            // update the product
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageURL = product.ImageURL;
            existingProduct.Recipe = product.Recipe;
            existingProduct.Ingredients = product.Ingredients;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound("Product not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingProduct);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Guid kitchenId, [FromBody] Product product)
        {
            // check if kitchen exists
            var kitchen = await _context.Kitchens.FindAsync(kitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            // check if product exists
            var existingProduct = await _context.Products.Where(m => m.KitchenId == kitchenId && m.Name == product.Name).FirstOrDefaultAsync();
            if (existingProduct != null)
            {
                return BadRequest("Product already exists.");
            }

            // set the kitchenId for the new product
            product.KitchenId = kitchenId;

            // add product to kitchen and save changes
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { kitchenId, id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted.");
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
