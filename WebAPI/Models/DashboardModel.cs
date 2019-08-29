using AngularJSAuthentication.API.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NoorCare.Repository;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Entity;
using WebAPI.Models;
using WebAPI.Repository;
using WebAPI.Services;
namespace WebAPI.Models
{
    public class DashboardModel
    {
        public SecratoryDashboardModel SecratoryDashboardModel { get; set; }
        public List<SecratoryDashboardAppointmentListModel> SecratoryDashboardAppointmentListModel { get; set; }
        
    }

    public class SecratoryDashboardModel
    {
        public string HospitalId { get; set; }
        public int TotalDoctor { get; set; }
        public int BookedAppointment { get; set; }
        public int CancelAppointment { get; set; }
        public int NewAppointment { get; set; }
        public int TodayAppointment { get; set; }
        public int DoctorOnLeave { get; set; }
        public int TotalFeedback { get; set; }
    }
    public class SecratoryDashboardAppointmentListModel
    {
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string TimeId { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string NoorCareNumber { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
    }
}