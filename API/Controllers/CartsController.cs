using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var carts = await _context.Carts.Where(c => c.CustomerId == customerId).ToListAsync();
            return carts;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(Guid id, Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("Cart not found.");
            }

            var customer = await _context.Customers.FindAsync(cart.CustomerId);
            // Check if customer exists
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // Check if the kitchen exists
            var kitchen = await _context.Kitchens.FindAsync(cart.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            foreach (var cartProduct in cart.CartProducts)
            {
                var product = await _context.Products.FindAsync(cartProduct.ProductId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }
            }

            // Check if the cart exists
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == cart.CustomerId && c.KitchenId == cart.KitchenId);

            if (existingCart == null)
            {
                return NotFound("Cart not found.");
            }

            // Update the cart products
            existingCart.CartProducts = cart.CartProducts;
            existingCart.UpdatedAt = DateTime.Now;

            // Recalculate the total price
            existingCart.TotalPrice = existingCart.CartProducts.Sum(cp => cp.Quantity * _context.Products.Find(cp.ProductId).Price);


            _context.Entry(existingCart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(existingCart);
        }


        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("Cart not found.");
            }

            var customer = await _context.Customers.FindAsync(cart.CustomerId);
            // Check if customer exists
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }
            // Check if the kitchen exists
            var kitchen = await _context.Kitchens.FindAsync(cart.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }
            // Check if the cart exists
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == cart.CustomerId && c.KitchenId == cart.KitchenId);
            if (existingCart != null)
            {
                if (cart.CartProducts != null)
                {
                    foreach (var cartProduct in cart.CartProducts)
                    {
                        var product = await _context.Products.FindAsync(cartProduct.ProductId);
                        if (product == null)
                        {
                            return NotFound("Product not found.");
                        }
                        else
                        {
                            // check if the product is already in the cart
                            var existingCartProduct = existingCart.CartProducts.FirstOrDefault(cp => cp.ProductId == cartProduct.ProductId);
                            if (existingCartProduct != null)
                            {
                                // update the quantity of the existing product in the cart
                                existingCartProduct.Quantity += cartProduct.Quantity;
                            }
                            else
                            {
                                // add the product to the cart
                                if (existingCart.CartProducts == null)
                                {
                                    existingCart.CartProducts = new List<CartProduct>();
                                }
                                existingCart.CartProducts.Add(cartProduct);
                            }
                            cartProduct.Product = product;
                        }
                    }
                    _context.Entry(existingCart).Property(c => c.CartProducts).IsModified = true;

                    // Recalculate the total price
                    existingCart.TotalPrice = existingCart.CartProducts.Sum(cp => cp.Quantity * _context.Products.Find(cp.ProductId).Price);

                    await _context.SaveChangesAsync();
                }

                _context.Entry(existingCart).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(existingCart);
            }
            // Create the cart
            cart.Id = Guid.NewGuid();
            cart.CartProducts = cart.CartProducts ?? new List<CartProduct>(); // Create an empty list if null

            // Assign the product for each cartProduct
            if (cart.CartProducts != null)
            {
                foreach (var cartProduct in cart.CartProducts)
                {
                    var product = await _context.Products.FindAsync(cartProduct.ProductId);
                    if (product == null)
                    {
                        return NotFound("Product not found.");
                    }
                    cartProduct.Product = product;
                }
            }

            cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Product?.Price * cp.Quantity ?? 0);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new { customerId = cart.CustomerId, id = cart.Id }, cart);
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

        // DELETE: api/Carts/5/Product/5
        [HttpDelete("{cartId}/products/{productId}")]
        public async Task<IActionResult> DeleteProductFromCart(Guid cartId, Guid productId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
            if (cartProduct == null)
            {
                return NotFound("Product not found in cart.");
            }
            cart.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();
            return Ok("Product deleted from cart.");
        }

        private bool CartExists(Guid id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
