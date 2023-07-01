using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/customers/{customerId}/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public CartsController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts(Guid customerId)
        {
            return await _context.Carts.Where(c => c.CustomerId == customerId).ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(Guid id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return cart;
        }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(Guid id, Cart cart)
        {
            // check if the cart exists
            var existingCart = await _context.Carts.FindAsync(id);

            if (existingCart == null)
            {
                return NotFound("Cart not found.");
            }

            // check if the customer exists
            var customer = await _context.Customers.FindAsync(existingCart.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // check if the cart Title already exists
            // var existingCartTitle = await _context.Carts.Where(c => c.Title == cart.Title && c.CustomerId == customer.Id).FirstOrDefaultAsync();
            // if (existingCartTitle != null)
            // {
            //     return BadRequest("Cart already exists.");
            // }

            // update the cart
            existingCart.CartProducts = cart.CartProducts;
            existingCart.CustomerId = customer.Id;
            if (cart.CartProducts != null)
            {
                if (cart.CartProducts != null)
                {
                    existingCart.TotalPrice = cart.CartProducts.Sum(cp => cp.Product.Price * cp.Quantity);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound("Cart not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingCart);
        }

        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Guid customerId, [FromBody] Cart cart)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            // check if customer exists
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // check if cart already exists
            var existingCart = await _context.Carts.Where(c => c.CartProducts == cart.CartProducts && c.CustomerId == customerId).FirstOrDefaultAsync();
            if (existingCart != null)
            {
                if (existingCart.CartProducts != null)
                {
                    foreach (var cartProduct in existingCart.CartProducts)
                    {
                        foreach (var newCartProduct in cart.CartProducts)
                        {
                            if (cartProduct.ProductId == newCartProduct.ProductId)
                            {
                                cartProduct.Quantity += newCartProduct.Quantity;
                            }
                        }
                    }
                }
            }

            // set the customerId of the new cart
            cart.CustomerId = customerId;

            // calculate the total price of the cart
            if (cart.CartProducts != null)
            {
                cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Product.Price * cp.Quantity);
            }

            // add cart to customer and save changes
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new { customerId = customerId, id = cart.Id }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok("Cart deleted.");
        }

        private bool CartExists(Guid id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
