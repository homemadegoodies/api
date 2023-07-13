using Data.Contexts;
using Data.Models.Domain;
using Data.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly GoodiesDataContext _context;
        public VendorsController(GoodiesDataContext context)
        {
            _context = context;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(Guid id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(Guid id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vendors
        //[HttpPost]
        //public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        //{
        //    _context.Vendors.Add(vendor);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        //}

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(Guid id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return Ok("Vendor deleted.");
        }

        private bool VendorExists(Guid id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(VendorRegisterRequest request)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the required fields";
                return Ok(status);
            }

            if (_context.Vendors.Any(u => u.Username == request.Username))
            {
                status.StatusCode = 0;
                status.Message = "Invalid username.";
                return Ok(status);
            }

            CreatePasswordHash(request.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

            var vendor = new Vendor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
                Country = request.Country,
                ProfilePicture = request.ProfilePicture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                VerificationToken = CreateRandomToken()
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            status.StatusCode = 1;
            status.Message = "Vendor successfully registered! :D";
            return Ok(status);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(VendorLoginRequest request)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (vendor == null)
            {
                return Ok(new VendorLoginResponse
                {
                    StatusCode = 0,
                    Message = "Vendor not found.",
                });
            }

            if (!VerifyPasswordHash(request.Password, vendor.PasswordHash, vendor.PasswordSalt))
            {
                return Ok(new VendorLoginResponse
                {
                    StatusCode = 0,
                    Message = "Invalid password.",
                });
            }

            //if (vendor.VerifiedAt == null)
            //{
            //    return BadRequest("Not verified!");
            //}

            return Ok(new VendorLoginResponse
            {
                Id = vendor.Id,
                FirstName = vendor.FirstName,
                LastName = vendor.LastName,
                Username = vendor.Username,
                Email = vendor.Email,
                ProfilePicture = vendor.ProfilePicture,
                PhoneNumber = vendor.PhoneNumber,
                Address = vendor.Address,
                City = vendor.City,
                Province = vendor.Province,
                PostalCode = vendor.PostalCode,
                Country = vendor.Country,
                IsVendor = vendor.IsVendor,
                StatusCode = 1,
                Message = "Vendor successfully logged in! :D",
            });
        }

        // [HttpPost("verify")]
        // public async Task<IActionResult> Verify(string token)
        // {
        //     var vendor = await _context.Vendors.FirstOrDefaultAsync(u => u.VerificationToken == token);
        //     if (vendor == null)
        //     {
        //         return BadRequest("Invalid token.");
        //     }

        //     vendor.VerifiedAt = DateTime.Now;
        //     await _context.SaveChangesAsync();

        //     return Ok("Vendor verified! :)");
        // }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(u => u.Username == username);
            if (vendor == null)
            {
                return BadRequest("Vendor not found.");
            }

            vendor.PasswordResetToken = CreateRandomToken();
            vendor.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (vendor == null || vendor.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            vendor.PasswordHash = passwordHash;
            vendor.PasswordSalt = passwordSalt;
            vendor.PasswordResetToken = null;
            vendor.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset. :)");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

    }
}
