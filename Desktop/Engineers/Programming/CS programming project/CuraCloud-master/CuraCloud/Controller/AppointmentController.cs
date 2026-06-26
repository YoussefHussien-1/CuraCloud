using CuraCloud.API.Data;
using CuraCloud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuraCloud.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .IgnoreQueryFilters()
                .Include(a => a.Tenant)
                .Include(a => a.Patient)
                .Select(a => new DTOS.AppointmentDto
                {
                    id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Fees = a.Fees,
                    IsPaid = a.IsPaid,
                    PatientName = a.Patient.FullName,
                    ClinicName = a.Tenant.ClinicName
                }).ToListAsync();
            return Ok(appointments);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var FindAppointmentWithId = await _context.Appointments
                .IgnoreQueryFilters()
                .Include(a => a.Tenant)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (FindAppointmentWithId == null) return NotFound();

            var Dto = new DTOS.AppointmentDto
            {
                id = FindAppointmentWithId.Id,
                AppointmentDate = FindAppointmentWithId.AppointmentDate,
                Status = FindAppointmentWithId.Status,
                Fees = FindAppointmentWithId.Fees,
                IsPaid = FindAppointmentWithId.IsPaid,
                PatientName = FindAppointmentWithId.Patient.FullName,
                ClinicName = FindAppointmentWithId.Tenant.ClinicName
            };
            return Ok(Dto);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostNewAppointment(DTOS.CreateAppointmentDto dto)
        {

            if (dto.Status != "Pending" && dto.Status != "Completed" && dto.Status != "Cancelled")
            {
                return BadRequest("Invalid status value.");
            }

            var PatientExists = await _context.Patients.IgnoreQueryFilters().AnyAsync(p => p.Id == dto.PatientId);
            var TenantExists = await _context.Tenants.AnyAsync(t => t.Id == dto.TenantId);

            if (!PatientExists || !TenantExists)
                return BadRequest("The patient or clinic does not exist in the system!");

            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                Fees = dto.Fees,
                PatientId = dto.PatientId,
                TenantId = dto.TenantId,
                Status = dto.Status,
                IsPaid = dto.IsPaid
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "The appointment has been created successfully." });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, DTOS.CreateAppointmentDto dto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.Fees = dto.Fees;
            appointment.PatientId = dto.PatientId;
            appointment.TenantId = dto.TenantId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/pay")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var appointment = await _context.Appointments.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null) return NotFound();

            appointment.IsPaid = true;

            await _context.SaveChangesAsync();
            return Ok(new { message = "The appointment status changed to Paid successfully." });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string newStatus)
        {
            var appointment = await _context.Appointments.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null) return NotFound();

            if (newStatus != "Pending" && newStatus != "Completed" && newStatus != "Cancelled")
            {
                return BadRequest("Invalid status value.");
            }

            appointment.Status = newStatus;

            await _context.SaveChangesAsync();
            return Ok(new { message = $"The appointment status updated to {newStatus}." });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}