using NoorCare.Repository;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    public class PatientController : ApiController
    {

        IPatientPrescriptionRepository _prescriptionRepo = RepositoryFactory.Create<IPatientPrescriptionRepository>(ContextTypes.EntityFramework);

        [Route("api/patient/getall")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetAll()
        {
            var result = _prescriptionRepo.GetAll().ToList();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/patient/getPrescription/{patientId}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage getPrescription(string patientId)
        {
            var result = _prescriptionRepo.Find(x => x.PatientId == patientId).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/patient/SavePatientPrescription")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage SavePatientPrescription(PatientPrescription obj)
        {
            var _prescriptionCreated = _prescriptionRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, obj.Id);
        }
    }
}

