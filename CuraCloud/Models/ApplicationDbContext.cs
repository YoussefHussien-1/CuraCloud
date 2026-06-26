using CuraCloud.API.Models;
using CuraCloud.Models;
using Microsoft.EntityFrameworkCore;

namespace CuraCloud.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            int currentTenantId = 1;
            modelBuilder.Entity<Patient>().HasQueryFilter(p => p.TenantId == currentTenantId);
            modelBuilder.Entity<Appointment>().HasQueryFilter(a => a.TenantId == currentTenantId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Tenant)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Fees)
                .HasColumnType("decimal(18,2)");
        }
    }
}