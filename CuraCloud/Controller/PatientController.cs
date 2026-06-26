using CuraCloud.API.Data;
using CuraCloud.DTOS;
using CuraCloud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;
namespace CuraCloud.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllPatients()
        {
            // 👈 ضفنا .IgnoreQueryFilters() قبل الـ Select عشان يطنش الفلتر الخفي
            var patients = await _context.Patients
                .IgnoreQueryFilters()
                .Select(patientSelection => new DTOS.PatientDto
                {
                    id = patientSelection.Id,
                    FullName = patientSelection.FullName,
                    phoneNumber = patientSelection.Phone,
                    email = patientSelection.Email,
                    gender = patientSelection.gender,
                    DateOfBirth = patientSelection.DateOfBirth
                }).ToListAsync();

            return Ok(patients);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostNewPatient([FromQuery] int tenantId, DTOS.CreatePatientDtos createPatient)
        {
            
            var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == tenantId);
            if (!tenantExists) return BadRequest("The specified clinic (Tenant) does not exist.");

            
            var patient = new Patient
            {
                FullName = createPatient.FullName,
                Phone = createPatient.phoneNumber,
                Email = createPatient.email,
                gender = createPatient.gender,
                DateOfBirth = createPatient.DateOfBirth,
                TenantId = tenantId 
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var resposeDtos = new PatientDto
            {
                id = patient.Id,
                FullName = patient.FullName,
                email = patient.Email,
                phoneNumber = patient.Phone,
                DateOfBirth = patient.DateOfBirth,
                gender = patient.gender
            };

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, resposeDtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var findPatientWithId = await _context.Patients.FindAsync(id);
            if (findPatientWithId == null) return NotFound();

            var GetPatientDto = new PatientDto { id = findPatientWithId.Id, FullName = findPatientWithId.FullName, phoneNumber = findPatientWithId.Phone, email = findPatientWithId.Email, gender = findPatientWithId.gender, DateOfBirth = findPatientWithId.DateOfBirth };
            return Ok(GetPatientDto);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var DeletePatientWithId = await _context.Patients.FindAsync(id);
            if (DeletePatientWithId != null)
            {
                _context.Patients.Remove(DeletePatientWithId);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound("I Can not found this patient.");
            }

        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(PatientDto patientDto)
        {
            var ExistPatient = await _context.Patients.FindAsync(patientDto.id);
            if (ExistPatient != null) 
            {
                ExistPatient.FullName = patientDto.FullName;
                ExistPatient.Phone = patientDto.phoneNumber;
                ExistPatient.Email = patientDto.email;
                ExistPatient.gender = patientDto.gender;
                ExistPatient.DateOfBirth = patientDto.DateOfBirth;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound("This patient is not exist.");
            }

        }
    }
}
