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

namespace WebAPI.Controllers
{
    public class DashboardController : ApiController
    {
        ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
        IFeedbackRepository _feedbackRepo = RepositoryFactory.Create<IFeedbackRepository>(ContextTypes.EntityFramework);
        IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
        IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
        ITimeMasterRepository _timeMasterRepo = RepositoryFactory.Create<ITimeMasterRepository>(ContextTypes.EntityFramework);
        IClientDetailRepository _clientDetailRepo = RepositoryFactory.Create<IClientDetailRepository>(ContextTypes.EntityFramework);
        
        [HttpGet]
        [Route("api/GetDashboardDetails/{Type}/{pageId}/{searchDate?}")] //Type= Doctor/Secratory, page Id is secratoryId
        [AllowAnonymous]
        public HttpResponseMessage GetDashboardDetails(string Type,string pageId, string searchDate = null)
        {
            var HospitalId = _secretaryRepo.GetAll().Find(s => s.SecretaryId == pageId).HospitalId;
            SecratoryDashboardModel secratoryDashboardModel = new SecratoryDashboardModel();
            DashboardModel dashboardModel = new DashboardModel();
            List<SecratoryDashboardAppointmentListModel> lstSecratoryDashboardAppointmentListModel = new List<SecratoryDashboardAppointmentListModel>();

            if (!string.IsNullOrEmpty(HospitalId))
            {
                secratoryDashboardModel.HospitalId = HospitalId;

                secratoryDashboardModel.TotalFeedback = _feedbackRepo.Find(x => x.PageId == HospitalId).ToList().Count();

                secratoryDashboardModel.TotalDoctor = _doctorRepo.Find(d => d.HospitalId == HospitalId).ToList().Count();

                secratoryDashboardModel.BookedAppointment = _appointmentRepo.Find(a => a.HospitalId == HospitalId && a.Status == "booked").ToList().Count();

                secratoryDashboardModel.CancelAppointment = _appointmentRepo.Find(a => a.HospitalId == HospitalId && a.Status == "cancel").ToList().Count();

                secratoryDashboardModel.NewAppointment = _appointmentRepo.Find(a => a.HospitalId == HospitalId && a.Status == "pending").ToList().Count();

                secratoryDashboardModel.TodayAppointment = _appointmentRepo.Find(a => a.HospitalId == HospitalId && a.Status == "booked" && a.AppointmentDate == searchDate).ToList().Count();
            }
            foreach (var item in _appointmentRepo.Find(a => a.HospitalId == HospitalId).ToList())
            {
                SecratoryDashboardAppointmentListModel secratoryDashboardAppointmentListModel = new SecratoryDashboardAppointmentListModel();

                secratoryDashboardAppointmentListModel.AppointmentDate = item.AppointmentDate;
                secratoryDashboardAppointmentListModel.Status = item.Status;
                secratoryDashboardAppointmentListModel.TimeId = item.TimingId;

                if (_timeMasterRepo != null)
                {
                    secratoryDashboardAppointmentListModel.AppointmentTime = "";
                }
                var clientDetail = _clientDetailRepo.Find(x => x.ClientId == item.ClientId).FirstOrDefault();
                if (clientDetail != null)
                {
                    secratoryDashboardAppointmentListModel.DOB = clientDetail.DOB;
                    secratoryDashboardAppointmentListModel.PatientName = clientDetail.FirstName;

                    secratoryDashboardAppointmentListModel.NoorCareNumber = clientDetail.ClientId;
                    secratoryDashboardAppointmentListModel.Gender = clientDetail.Gender.ToString();
                }

                secratoryDashboardAppointmentListModel.DoctorName = "";

                lstSecratoryDashboardAppointmentListModel.Add(secratoryDashboardAppointmentListModel);
            }

            dashboardModel.SecratoryDashboardModel = secratoryDashboardModel;
            dashboardModel.SecratoryDashboardAppointmentListModel = lstSecratoryDashboardAppointmentListModel;
            return Request.CreateResponse(HttpStatusCode.Accepted, dashboardModel);
        }


    }
}
