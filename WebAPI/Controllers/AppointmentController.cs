using NoorCare.Repository;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class AppointmentController : ApiController
    {
        Registration _registration = new Registration();
        [Route("api/appointment/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Appointment
        public HttpResponseMessage GetAll()
        {
            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var result =  _appointmentRepo.GetAll().ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/appointment/getdetail/{appointmentid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Appointment/5
        public HttpResponseMessage GetDetail(string appointmentid)
        {
            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var result = _appointmentRepo.Find(x => x.DoctorId == appointmentid).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);            
        }

        [Route("api/appointment/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/Appointment
        public HttpResponseMessage Register(Appointment obj)
        {
            ICountryCodeRepository _countryCodeRepository = RepositoryFactory.Create<ICountryCodeRepository>(ContextTypes.EntityFramework);
            CountryCode countryCode = _countryCodeRepository.Find(x => x.Id == obj.CountryCode).FirstOrDefault();
            string AppointmentId= _registration.creatId(5, obj.CountryCode, 0);
            obj.AppointmentId = AppointmentId;
            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var _appointmentCreated = _appointmentRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, obj.AppointmentId);
        }
 

        [Route("api/appointment/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Appointment/5
        public HttpResponseMessage Update(Appointment obj)
        {
            int tbleId = getTableId(obj.AppointmentId);
            obj.Id = tbleId;
            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var result = _appointmentRepo.Update(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/appointment/delete/{appointmentid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Appointment/5
        public HttpResponseMessage Delete(string appointmentid)
        {
            int tbleId= getTableId(appointmentid);

            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var result = _appointmentRepo.Delete(tbleId);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        private int getTableId(string appointmentId)
        {
            IAppointmentRepository _appointmentRepo = RepositoryFactory.Create<IAppointmentRepository>(ContextTypes.EntityFramework);
            var result = _appointmentRepo.Find(x => x.DoctorId == appointmentId).FirstOrDefault();

            return result.Id;
        }
    }
}
