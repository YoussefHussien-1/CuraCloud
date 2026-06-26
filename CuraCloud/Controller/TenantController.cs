using CuraCloud.API.Data;
using CuraCloud.API.Models;
using CuraCloud.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuraCloud.Controller
{
    [ApiController]
    [Route("api/[controller]")]

    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TenantController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTenant()
        {
            var tenant = await _context.Tenants.Select(tenantSelection => new DTOS.TenantDtos
            {
                Id = tenantSelection.Id,
                clinicName = tenantSelection.ClinicName,
                doctorName = tenantSelection.DoctorName
            }).ToListAsync();
            return Ok(tenant);
        }




        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTenantWithID(int id)
        {
            var findTenantWithId = await _context.Tenants.FindAsync(id);
            if (findTenantWithId == null) return NotFound("tenant is not exist.");
            var GetTenantDto = new TenantDtos { Id = findTenantWithId.Id, clinicName = findTenantWithId.ClinicName, doctorName = findTenantWithId.DoctorName, email = findTenantWithId.Email, dateOfBirth = findTenantWithId.dateOfBirth };
            return Ok(GetTenantDto);
        }





        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostTenant(DTOS.CreateTenantDtos createTenant)
        {
            var tenant = new Tenant
            {
                ClinicName = createTenant.clinicName,
                DoctorName = createTenant.doctorName,
                Email = createTenant.email,
                dateOfBirth = createTenant.DataOfBirth
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            var responseDtos = new TenantDtos
            {
                Id = tenant.Id,
                clinicName = tenant.ClinicName,
                doctorName = tenant.DoctorName,
            };
            return CreatedAtAction(nameof(GetTenantWithID), new { id = tenant.Id }, responseDtos);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var DeleteTenantWithId = await _context.Tenants.FindAsync(id);
            if (DeleteTenantWithId != null)
            {
                _context.Tenants.Remove(DeleteTenantWithId);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else return NotFound("i can not found this tenant.");
        }





        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(TenantDtos tenant)
        {
            var existTenant = await _context.Tenants.FindAsync(tenant.Id);
            if (existTenant != null)
            {
                existTenant.ClinicName = tenant.clinicName;
                existTenant.DoctorName = tenant.doctorName;
                existTenant.Email = tenant.email;
                existTenant.dateOfBirth = tenant.dateOfBirth;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else return NotFound("this tenant is not exist");
        }
        
    }
}
