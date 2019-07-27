﻿using NoorCare.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoorCare.WebAPI.Entity
{
    [Serializable]
    [Table("Facility")]
    public class Facility : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string facility { get; set; }

    }

    [Serializable]
    [Table("CountryCode")]
    public class CountryCode : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string CountryCodes { get; set; }

    }

    [Serializable]
    [Table("TblCity")]
    public class TblCity : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int CountryId { get; set; }
    }

    [Serializable]
    [Table("TblCountry")]
    public class TblCountry : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }

    [Serializable]
    [Table("tblState")]
    public class State : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string state { get; set; }

    }

    [Serializable]
    [Table("InsuranceInformation")]
    public class InsuranceInformation : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string CompanyName { get; set; }
        public int InsuraceNo { get; set; }
    }

    [Serializable]
    [Table("QuickHeathDetails")]
    public class QuickHeathDetails : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int HospitalId { get; set; }
        public string Pressure { get; set; }
        public string Heartbeats { get; set; }
        public string Temprature { get; set; }
        public string Sugar { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string Cholesterol { get; set; }
        public string Other { get; set; }
    }

    //[Serializable]
    //[Table("HospitalDetail")]
    //public class HospitalDetail : IEntityBase
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public string HospitalName { get; set; }
    //    public string Address { get; set; }
    //    public int Mobile { get; set; }
    //    public string Email { get; set; }
    //    public string Website { get; set; }
    //    public string FullName { get; set; }
    //    public string HospitalId { get; set; }
    //    public string EstablishYear { get; set; }
    //    public int NumberofBed { get; set; }
    //    public int NumberofAmbulance { get; set; }
    //    public string PaymentType { get; set; }
    //    public int Emergency { get; set; }
    //    public int FacilityId { get; set; }
    //    public int jobType { get; set; }
    //    public bool EmailConfirmed { get; set; }
    //}

    [Serializable]
    [Table("QuickUpload")]
    public class QuickUpload : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int HospitalId { get; set; }
        public int DesiesType { get; set; }
        public string FilePath { get; set; }
    }
}