﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebAPI.Entity;

namespace WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int JobType { get; set; }
        public int CountryCodes { get; set; }
        public int? Gender { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //AspNetUsers -> User
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            //AspNetRoles -> Role
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //AspNetUserRoles -> UserRole
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            //AspNetUserClaims -> UserClaim
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            //AspNetUserLogins -> UserLogin
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ClientDetail>().ToTable("ClientDetail");
            modelBuilder.Entity<PatientPrescription>().ToTable("PatientPrescription");
            

            modelBuilder.Entity<Facility>().ToTable("Facility");
            modelBuilder.Entity<Disease>().ToTable("DiseaseType");
            modelBuilder.Entity<EmergencyContact>().ToTable("EmergencyContact");
            modelBuilder.Entity<MedicalInformation>().ToTable("MedicalInformation");
            modelBuilder.Entity<CountryCode>().ToTable("CountryCode");
            modelBuilder.Entity<TblCity>().ToTable("TblCity");
            modelBuilder.Entity<TblCountry>().ToTable("TblCountry");
            modelBuilder.Entity<InsuranceInformation>().ToTable("InsuranceInformation");
            modelBuilder.Entity<QuickHeathDetails>().ToTable("QuickHeathDetails");
            modelBuilder.Entity<HospitalDetails>().ToTable("HospitalDetails");
            modelBuilder.Entity<QuickUpload>().ToTable("QuickUpload");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Secretary>().ToTable("Secretary");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<TblHospitalAmenities>().ToTable("NoorCare.TblHospitalAmenities");
            modelBuilder.Entity<TblHospitalServices>().ToTable("TblHospitalServices");
            modelBuilder.Entity<TblHospitalSpecialties>().ToTable("TblHospitalSpecialties");
            modelBuilder.Entity<DoctorAvailableTime>().ToTable("DoctorAvailableTime");
            modelBuilder.Entity<ContactUs>().ToTable("ContactUs");
            modelBuilder.Entity<TimeMaster>().ToTable("TimeMaster");

        }
    }
}