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
    public class DoctorController : ApiController
    {
        Registration _registration = new Registration();
        IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
        IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
        IDoctorAvailableTimeRepository _doctorAvailabilityRepo = RepositoryFactory.Create<IDoctorAvailableTimeRepository>(ContextTypes.EntityFramework);
        
        [Route("api/doctor/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor
        public HttpResponseMessage GetAll()
        {
            var result = _doctorRepo.GetAll().ToList();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/getdetail/{doctorid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor/5
        public HttpResponseMessage GetDetail(string doctorid)
        {
            var result = _doctorRepo.Find(x => x.DoctorId == doctorid).FirstOrDefault();

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
            string password = _registration.RandomPassword(6);
            ApplicationUser user = _registration.UserAcoount(obj, Convert.ToInt16(countryCode.CountryCodes));
            IdentityResult result = manager.Create(user, password);
            user.PasswordHash = password;
            _registration.sendRegistrationEmail(user);
            obj.DoctorId = user.Id;
            IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var _doctorCreated = _doctorRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, obj.DoctorId);
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
                    var filePath = HttpContext.Current.Server.MapPath("~/ProfilePic/Doctor/" + imageName);
                    bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ProfilePic/Doctor/" + imageName));
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

        [HttpGet]
        [Route("api/Doctor/profile/{DoctorId}")]
        [AllowAnonymous]
        public IHttpActionResult getDoctorProfile(string DoctorId)
        {
          //  IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            return Ok(_doctorRepo.Find(x => x.DoctorId == DoctorId));
        }


        [Route("api/doctor/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Doctor/5
        public HttpResponseMessage Update(Doctor obj)
        {
            int tbleId = getTableId(obj.DoctorId);
            obj.Id = tbleId;
            var result = _doctorRepo.Update(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/delete/{doctorid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Doctor/5
        public HttpResponseMessage Delete(string doctorid)
        {
            int tbleId = getTableId(doctorid);

            // IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Delete(tbleId);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        private int getTableId(string doctorId)
        {
            //IDoctorRepository _doctorRepo = RepositoryFactory.Create<IDoctorRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Find(x => x.DoctorId == doctorId).FirstOrDefault();

            return result.Id;
        }

        //---------------- Doctor Availability
        [Route("api/doctor/doctorAvailablity")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage DoctorAvailablity(DoctorAvailableTime obj)
        {            
            var _Created = _doctorAvailabilityRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, obj.Id);
        }

        [Route("api/doctor/getDoctorAvailablity/{doctorid}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage getDoctorAvailablity(string doctorid)
        {
            var result = _doctorAvailabilityRepo.Find(x => x.DoctorId == doctorid).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }


        [Route("api/doctor/getDoctorDetail/{cityId}/{countryId}/{diesiesType}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage getDoctorDetail(string cityId, string countryId, string diesiesType)
        {
            var result = (
                 from d in _doctorRepo.GetAll()
                 join h in _hospitaldetailsRepo.GetAll() on d.HospitalId equals h.HospitalId
                 select new
                 {
                     DoctorId = d.DoctorId,
                     FirstName = d.FirstName,
                     LastName = d.LastName,
                     Email = d.Email,
                     PhoneNumber = d.PhoneNumber,
                     AlternatePhoneNumber = d.AlternatePhoneNumber,
                     Gender = d.Gender,
                     Experience = d.Experience,
                     FeeMoney = d.FeeMoney,
                     Language = d.Language,
                     AgeGroupGender = d.AgeGroupGender,
                     Degree = d.Degree,
                     Specialization = d.Specialization,
                     AboutUs = d.AboutUs,
                     HospitalId = h.HospitalId,
                     HospitalName = h.HospitalName,
                     Mobile = h.Mobile,
                     AlternateNumber = h.AlternateNumber,
                     Website = h.Website,
                     EstablishYear = h.EstablishYear,
                     NumberofBed = h.NumberofBed,
                     NumberofAmbulance = h.NumberofAmbulance,
                     PaymentType = h.PaymentType,
                     Emergency = h.Emergency,
                     FacilityId = h.FacilityId,
                     Address = h.Address,
                     Street = h.Street,
                     Country = h.Country,
                     City = h.City,
                     PostCode = h.PostCode,
                     Landmark = h.Landmark,
                     InsuranceCompanies = h.InsuranceCompanies,
                     Amenities = h.Amenities,
                     Services = h.Services,
                     Timing = h.Timing,
                     Monday = h.Monday,
                     Tuesday = h.Tuesday,
                     Wednesday = h.Wednesday,
                     Thursday = h.Thursday,
                     Friday = h.Friday,
                     Saturday = h.Saturday,
                     Sunday = h.Sunday,

                 }).ToList();

            var SearchResult = result.Where(x => x.Country == countryId || x.City == cityId || x.Specialization.Contains(diesiesType)).ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, SearchResult);
        }
    }
}

