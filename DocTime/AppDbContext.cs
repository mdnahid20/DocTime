using DocTime.Model;
using Microsoft.EntityFrameworkCore;

namespace DocTime;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Doctor> Doctor { get; set; }

    internal async Task GetAllAppointmentsAsync()
    {
        throw new NotImplementedException();
    }
}
