using Data.Contexts;
using Data.Models.Domain;
using Data.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using API.Services;

namespace API.Controllers
{
    [Route("api/kitchens/{kitchenId}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public OrdersController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(Guid kitchenId)
        {
            return await _context.Orders.Where(o => o.KitchenId == kitchenId).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return order;
        }

        // GET: api/vendors/{vendorId}/Orders
        [HttpGet("~/api/vendors/{vendorId}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetVendorOrders(Guid vendorId)
        {
            return await _context.Orders.Where(o => o.VendorId == vendorId).ToListAsync();
        }

        // GET: api/vendors/{vendorId}/Orders/5
        [HttpGet("~/api/vendors/{vendorId}/orders/{id}")]
        public async Task<ActionResult<Order>> GetVendorOrder(Guid vendorId, Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null || order.VendorId != vendorId)
            {
                return NotFound("Order not found.");
            }

            return order;
        }

        // GET: api/customers/{customerId}/Orders
        [HttpGet("~/api/customers/{customerId}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(Guid customerId)
        {
            return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        // GET: api/customers/{customerId}/Orders/5
        [HttpGet("~/api/customers/{customerId}/orders/{id}")]
        public async Task<ActionResult<Order>> GetCustomerOrder(Guid customerId, Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null || order.CustomerId != customerId)
            {
                return NotFound("Order not found.");
            }

            return order;
        }

        [HttpPost("~/api/customers/{customerId}/carts/{cartId}/orders")]
        public async Task<ActionResult<Order>> CreateOrder(Guid customerId, Guid cartId, [FromBody] OrderCreateDTO orderDTO)
        {
            var stripeService = new StripeService();

            // Retrieve the kitchen and validate its existence
            var kitchen = await _context.Kitchens.FindAsync(orderDTO.KitchenId);
            if (kitchen == null)
            {
                return NotFound("Kitchen not found.");
            }

            // Retrieve the cart and validate its existence
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null || cart.CustomerId != customerId || cart.KitchenId != orderDTO.KitchenId)
            {
                return NotFound("Cart not found.");
            }

            // Retrieve the cart products
            var cartProducts = cart.CartProducts;
            if (cartProducts.Count == 0)
            {
                return BadRequest("Cart is empty.");
            }

            var totalPrice = cart.TotalPrice;

            // Create a new Order instance
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDTO.CustomerId,
                VendorId = orderDTO.VendorId,
                KitchenId = orderDTO.KitchenId,
                OrderProducts = new List<OrderProduct>(),
                TotalPrice = totalPrice,
                Status = "Pending",
                // DeliveryDate = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now
            };

            try
            {
                // Map cart products to order products
                foreach (var cartProduct in cartProducts)
                {
                    var product = await _context.Products.FindAsync(cartProduct.ProductId);
                    if (product == null)
                    {
                        return NotFound("Product not found.");
                    }

                    var orderProduct = new OrderProduct
                    {
                        ProductId = cartProduct.ProductId,
                        Quantity = cartProduct.Quantity,
                        Price = product.Price
                    };

                    order.OrderProducts.Add(orderProduct);
                }

                // Process the payment using Stripe
                var paymentRequest = new PaymentIntentCreateOptions
                {
                    Amount = Convert.ToInt64(order.TotalPrice * 100), // Stripe requires the amount in cents
                    Currency = "CAD", // Set the currency according to your needs
                    PaymentMethodTypes = new List<string> { "card" } // Only accept card payments
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(paymentRequest);

                // Update the order's payment details
                order.PaymentDetails = new PaymentDetail
                {
                    PaymentMethod = "card",
                    PaymentStatus = paymentIntent.Status,
                    PaymentAmount = order.TotalPrice,
                    PaymentDate = DateTime.Now
                };

                // Save the order to the database
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Delete the cart
                // _context.Carts.Remove(cart);
                // await _context.SaveChangesAsync();

                // Return the created order
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id, kitchenId = order.KitchenId }, order);
            }
            catch (StripeException ex)
            {
                // Handle any Stripe-specific exceptions
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other generic exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/vendors/{vendorId}/Orders/5
        [HttpPut("~/api/vendors/{vendorId}/orders/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid vendorId, Guid id, [FromBody] OrderUpdateDTO orderDTO)
        {
            // Retrieve the vendor and validate its existence
            var vendor = await _context.Vendors.FindAsync(vendorId);
            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            // Retrieve the order and validate its existence
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.VendorId != vendorId)
            {
                return NotFound("Order not found.");
            }

            // Update the order with the provided data
            order.DeliveryDate = orderDTO.DeliveryDate;
            order.Status = orderDTO.Status;
            order.UpdatedAt = DateTime.Now;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return a success response
                return Ok("Order updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("~/api/create-payment-intent")]
        public async Task<ActionResult> CreatePaymentIntent([FromBody] PaymentIntentCreateDTO paymentIntentDTO)
        {
            var stripeService = new StripeService();
            var paymentIntent = await stripeService.CreatePaymentIntent(paymentIntentDTO.Amount);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }




        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Delete the order from the database
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
