namespace DocTime.Dtos;

public class AppointmentDto
{
    public string PatientName { get; set; }
    public string PatientContact { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public int DoctorId { get; set; }
}
