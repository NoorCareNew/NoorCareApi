using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using NoorCare.WebAPI.Entity;

namespace NoorCare.WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class NoorCareDbContext : IdentityDbContext<ApplicationUser> {

        public NoorCareDbContext(): base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ClientDetail>().ToTable("ClientDetail");
            modelBuilder.Entity<Facility>().ToTable("Facility");
            modelBuilder.Entity<Disease>().ToTable("DiseaseType");
            modelBuilder.Entity<EmergencyContact>().ToTable("EmergencyContact");
            modelBuilder.Entity<MedicalInformation>().ToTable("MedicalInformation");
            modelBuilder.Entity<CountryCode>().ToTable("CountryCode");
            modelBuilder.Entity<TblCity>().ToTable("TblCity");
            modelBuilder.Entity<TblCountry>().ToTable("TblCountry");
            modelBuilder.Entity<InsuranceInformation>().ToTable("InsuranceInformation");
            modelBuilder.Entity<QuickHeathDetails>().ToTable("QuickHeathDetails");
            modelBuilder.Entity<QuickUpload>().ToTable("QuickUpload");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Secretary>().ToTable("Secretary");            
        }
    }
}