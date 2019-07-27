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
    public class SecretaryController : ApiControllerBase
    {

        private readonly IEntityBaseRepository<Secretary> _secretaryRepo;
        private readonly IEntityBaseRepository<CountryCode> _countryCodeRepository;
        public SecretaryController(IEntityBaseRepository<Secretary> secretaryRepository,
                                        IEntityBaseRepository<CountryCode> countryCodeRepo,
                                    IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _secretaryRepo = secretaryRepository;
            _countryCodeRepository = countryCodeRepo;
        }

        [Route("api/secretary/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Secretary
        public IHttpActionResult GetAll()
        {
            var result = _secretaryRepo.GetAll().ToList();
            return Ok(result);
        }

        [Route("api/secretary/getdetail/{id}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Secretary/5
        public IHttpActionResult GetDetail(string secretaryId)
        {
            var result = _secretaryRepo.FindBy(x => x.SecretaryId == secretaryId).FirstOrDefault();
            return Ok(result);
        }

        [Route("api/secretary/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/Secretary
        public IHttpActionResult Register(Secretary obj)
        {
            CountryCode countryCode = _countryCodeRepository.FindBy(x => x.Id == obj.CountryCode).FirstOrDefault();
            EmailSender _emailSender = new EmailSender();
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);

            string secretaryId = creatIdPrix(obj) + countryCode.CountryCodes + "-" + _emailSender.Get();

            obj.SecretaryId = secretaryId;
            _secretaryRepo.Add(obj);
            _unitOfWork.Commit();
            return Ok(obj);
        }

        [HttpPost]
        [Route("api/secretary/uploadprofilepic")]
        [AllowAnonymous]
        public IHttpActionResult UploadProfilePic()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            string secretaryId = httpRequest.Form["SecretaryId"];
            try
            {
                var postedFile = httpRequest.Files["Image"];
                if (postedFile != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).
                        Take(10).ToArray()).
                        Replace(" ", "-");
                    imageName = secretaryId + "." + ImageFormat.Jpeg;
                    var filePath = HttpContext.Current.Server.MapPath("~/ProfilePic/Secretary/" + imageName);
                    bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ProfilePic/Secretary/" + imageName));
                    if (exists)
                    {
                        File.Delete(filePath);
                    }
                    postedFile.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(secretaryId);
        }

        [HttpGet]
        [Route("api/secretary/profile/{secretaryId}")]
        [AllowAnonymous]
        public IHttpActionResult getSecretaryProfile(string secretaryId)
        {
            return Ok(_secretaryRepo.FindBy(x => x.SecretaryId == secretaryId));
        }
        
        [Route("api/secretary/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Secretary/5
        public IHttpActionResult Update(Secretary obj)
        {
            int tbleId = getTableId(obj.SecretaryId);
            obj.Id = tbleId;

            _secretaryRepo.Edit(obj);
            _unitOfWork.Commit();
            return Ok(obj);
        }

        [Route("api/secretary/delete/{doctorid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Secretary/5
        public IHttpActionResult Delete(string doctorid)
        {
            int tbleId = getTableId(doctorid);
            _secretaryRepo.Delete(_secretaryRepo.GetSingle(tbleId));
            _unitOfWork.Commit();
            return Ok(doctorid);
        }

        private int getTableId(string secretaryId)
        {
            var result = _secretaryRepo.FindBy(x => x.SecretaryId == secretaryId).FirstOrDefault();

            return result.Id;
        }

        public string creatIdPrix(Secretary model)
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
