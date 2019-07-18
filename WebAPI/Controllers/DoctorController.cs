using NoorCare.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Entity;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    public class DoctorController : ApiController
    {
        [Route("api/doctor/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor
        public HttpResponseMessage GetAll()
        {
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result =  _doctorRepo.GetAll().ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/getdetail/{id}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor/5
        public HttpResponseMessage GetDetail(int id)
        {
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Find(x => x.Id == id).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
            
        }

        [Route("api/doctor/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/Doctor
        public HttpResponseMessage Register(Doctor obj)
        {
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Doctor/5
        public HttpResponseMessage Update(Doctor obj)
        {
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Update(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/delete/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Doctor/5
        public HttpResponseMessage Delete(int id)
        {
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Delete(id);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }
    }
}
