using AngularJSAuthentication.API.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NoorCare.Repository;
using System;
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
    public class DoctorController : ApiController
    {
        Registration _registration = new Registration();

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
            ICountryCodeRepository _countryCodeRepository = RepositoryFactory.Create<ICountryCodeRepository>(ContextTypes.EntityFramework);
            CountryCode countryCode = _countryCodeRepository.Find(x => x.Id == obj.CountryCode).FirstOrDefault();
            EmailSender _emailSender = new EmailSender();
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);

            string doctorId = creatIdPrix(obj) + countryCode.CountryCodes + "-" + _emailSender.Get();

            obj.DoctorId = doctorId;


            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [HttpPost]
        [Route("api/doctor/uploadprofilepic")]
        [AllowAnonymous]
        public IHttpActionResult UploadProfilePic()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            string doctorId = httpRequest.Form["DoctorId"];
            try
            {
                var postedFile = httpRequest.Files["Image"];
                if (postedFile != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).
                        Take(10).ToArray()).
                        Replace(" ", "-");
                    imageName = doctorId + "." + ImageFormat.Jpeg;
                    var filePath = HttpContext.Current.Server.MapPath("~/ProfilePic/" + imageName);
                    bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ProfilePic/" + imageName));
                    if (exists)
                    {
                        File.Delete(filePath);
                    }
                    postedFile.SaveAs(filePath);
                }
            }
            catch (Exception)
            {
            }
            return Ok(doctorId);
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


        public string creatIdPrix(Doctor model)
        {
            string priFix = "NCM-";
            if (model.jobType == 2)
            {
                priFix = "NCH-";
            }
            else if (model.Gender == 1 && model.jobType == 3)
            {
                priFix = "NCM-";
            }
            else if (model.Gender == 2 && model.jobType == 3)
            {
                priFix = "NCF-";
            }
            return priFix;
        }
    }
}
