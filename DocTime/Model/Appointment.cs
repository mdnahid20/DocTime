using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocTime.Model;

public class Appointment
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public string PatientContact { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public int DoctorId { get; set; }
}
