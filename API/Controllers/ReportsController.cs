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
    public class ReportsController : ControllerBase
    {
        private readonly GoodiesDataContext _context;

        public ReportsController(GoodiesDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var reports = await _context.Reports
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();

            return reports;
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound("Report not found.");
            }

            return report;
        }

        // GET: api/vendors/{vendorId}/Reports
        [HttpGet("~/api/vendors/{vendorId}/reports")]
        public async Task<ActionResult<IEnumerable<Report>>> GetVendorReports(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);

            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            var reports = await _context.Reports
                .Where(r => r.VendorId == vendorId)
                .ToListAsync();

            return reports;
        }

        // GET: api/vendors/{vendorId}/Reports/5
        [HttpGet("~/api/vendors/{vendorId}/reports/{id}")]
        public async Task<ActionResult<Report>> GetVendorReport(Guid vendorId, Guid id)
        {
            var report = await _context
                .Reports
                .FindAsync(id);

            if (report == null)
            {
                return NotFound("Report not found.");
            }

            return report;
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Guid customerId, Report report)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            report.CustomerId = customerId;

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReport), new { customerId, id = report.Id }, report);

        }

        // POST: api/vendors/{vendorId}/Reports
        [HttpPost("~/api/vendors/{vendorId}/reports")]
        public async Task<ActionResult<Report>> PostVendorReport(Guid vendorId, Report report)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);

            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            report.VendorId = vendorId;

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.Id }, report);
        }

        // PUT: api/Reports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(Guid id, Report report)
        {
            if (id != report.Id)
            {
                return BadRequest("Report id mismatch.");
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ReportExists(id))
            {
                return NotFound("Report not found.");
            }

            return NoContent();
        }

        // PUT: api/vendors/{vendorId}/Reports/5
        [HttpPut("~/api/vendors/{vendorId}/reports/{id}")]
        public async Task<IActionResult> PutVendorReport(Guid vendorId, Guid id, Report report)
        {
            if (id != report.Id)
            {
                return BadRequest("Report id mismatch.");
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ReportExists(id))
            {
                return NotFound("Report not found.");
            }

            return NoContent();
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Report>> DeleteReport(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound("Report not found.");
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return report;
        }

        private bool ReportExists(Guid id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}