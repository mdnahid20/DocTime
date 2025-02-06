using Azure.Core;
using DocTime.Dtos;
using DocTime.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocTime.Controllers;

[Authorize]
public class AppointmentsController : ApiController
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<List<Appointment>>> GetAppointments()
    {
        var appointments = await _context.Appointments.ToListAsync();
        return Ok(appointments);  
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetAppointment(int id)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);


        if (appointment == null)
        {
            return NotFound();  // Return 404 if not found
        }

        return Ok(appointment);  // Return the appointment
    }

    // POST: api/appointments
    [HttpPost("Create-Appointment")]
    public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentDto requestAppointment)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid appointment data.");
        }

        var existingDoctor = await _context.Doctor.FirstOrDefaultAsync(u => u.Id == requestAppointment.DoctorId);
        
        if(existingDoctor == null)
        {
            return BadRequest("Doctor is not available");
        }

        var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.DoctorId == requestAppointment.DoctorId && u.AppointmentDateTime == requestAppointment.AppointmentDateTime);

        if (existingAppointment != null || requestAppointment.AppointmentDateTime<DateTime.Now)
        {
            return BadRequest("Doctor is not available at this time");
        }

        Appointment appointment = new Appointment
        {
            PatientName = requestAppointment.PatientName,
            AppointmentDateTime = requestAppointment.AppointmentDateTime,
            PatientContact = requestAppointment.PatientContact,
            DoctorId = requestAppointment.DoctorId
        };

         _context.Add(appointment);
        await _context.SaveChangesAsync();

        return Ok("Appointment created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAppointment([FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid appointment data.");
        }

        var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id == appointment.Id);  
        
        if(existingAppointment == null)
        {
            return BadRequest("Appointment does not exist");
        }

        var existingDoctor = await _context.Doctor.FirstOrDefaultAsync(u => u.Id == appointment.DoctorId);

        if (existingDoctor == null)
        {
            return BadRequest("Doctor is not available");
        }

        existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.DoctorId == appointment.DoctorId && u.AppointmentDateTime == appointment.AppointmentDateTime);

        if (existingAppointment != null || appointment.AppointmentDateTime < DateTime.Now)
        {
            return BadRequest("Doctor is not available at this time");
        }

        existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id == appointment.Id);
        
        existingAppointment.AppointmentDateTime = appointment.AppointmentDateTime;
        existingAppointment.PatientName = appointment.PatientName;
        existingAppointment.PatientContact = appointment.PatientContact;
        existingAppointment.DoctorId = appointment.DoctorId;    

        _context.Appointments.Update(existingAppointment);
        _context.SaveChanges();
        return Ok("Appointment update Successfully");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id == id);

        if (existingAppointment == null)
        {
            return BadRequest("There is no Appointment with this Id");
        }

        _context.Appointments.Remove(existingAppointment);
        _context.SaveChanges();

        return Ok("Appointment is Deleted");
    }

    [HttpPost("Create-Doctor")]
    public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] DoctorDto requestDoctor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Doctor data.");
        }

        Doctor doctor = new Doctor
        {
            Name = requestDoctor.Name,
        };


        _context.Add(doctor);
        await _context.SaveChangesAsync();

        return Ok("Doctor is Created");
    }

    [HttpGet("Get-Doctor")]
    public async Task<ActionResult<List<Doctor>>> GetDoctors()
    {
        var doctors = await _context.Doctor.ToListAsync();
        return Ok(doctors);
    }



}
