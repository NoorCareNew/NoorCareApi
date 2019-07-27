using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NoorCare.API.Services;
using NoorCare.Data.Infrastructure;
using NoorCare.Data.Repositories;
using NoorCare.Web.Infrastructure.Core;
using NoorCare.WebAPI.Entity;
using NoorCare.WebAPI.Models;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NoorCare.WebAPI.Controllers
{
    public class DoctorController : ApiControllerBase
    {

        private readonly IEntityBaseRepository<CountryCode> _countryCodeRepository;
        private readonly IEntityBaseRepository<Doctor> _doctorRepo;
        EmailSender _emailSender = new EmailSender();

        public DoctorController(IEntityBaseRepository<CountryCode> countryCodeRepository,
                                    IEntityBaseRepository<Doctor> doctorRepo,
                                    IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _countryCodeRepository = countryCodeRepository;
            _doctorRepo = doctorRepo;
        }

        [Route("api/doctor/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor
        public HttpResponseMessage GetAll()
        {
            var result =  _doctorRepo.GetAll().ToList();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/doctor/getdetail/{doctorid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Doctor/5
        public IHttpActionResult GetDetail(string doctorid)
        {
            var result = _doctorRepo.FindBy(x => x.DoctorId == doctorid).FirstOrDefault();
            return Ok(result);            
        }

        [Route("api/doctor/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/Doctor
        public IHttpActionResult Register(Doctor obj)
        {
            CountryCode countryCode = _countryCodeRepository.FindBy(x => x.Id == obj.CountryCode).FirstOrDefault();
            
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            string doctorId = creatIdPrix(obj) + countryCode.CountryCodes + "-" + _emailSender.Get();
            obj.DoctorId = doctorId;
             _doctorRepo.Add(obj);
            _unitOfWork.Commit();
            return Ok();
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
            return Ok(_doctorRepo.FindBy(x => x.DoctorId == DoctorId));
        }


        [Route("api/doctor/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Doctor/5
        public IHttpActionResult Update(Doctor obj)
        {
            int tbleId = getTableId(obj.DoctorId);
            obj.Id = tbleId;
            _doctorRepo.Edit(obj);
            _unitOfWork.Commit();
            return Ok();
        }

        [Route("api/doctor/delete/{doctorid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Doctor/5
        public IHttpActionResult Delete(string doctorid)
        {
            int tbleId= getTableId(doctorid);
            _doctorRepo.Delete(_doctorRepo.GetSingle(tbleId));
            _unitOfWork.Commit();
            return Ok();
        }

        private int getTableId(string doctorId)
        {
            var result = _doctorRepo.FindBy(x => x.DoctorId == doctorId).FirstOrDefault();
            return result.Id;
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
