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

namespace WebAPI.Controllers
{
    public class SecretaryController : ApiController
    {
        [Route("api/secretary/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Secretary
        public HttpResponseMessage GetAll()
        {
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            var result = _secretaryRepo.GetAll().ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/secretary/getdetail/{id}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Secretary/5
        public HttpResponseMessage GetDetail(int id)
        {
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            var result = _secretaryRepo.Find(x => x.Id == id).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/secretary/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/Secretary
        public HttpResponseMessage Register(Secretary obj)
        {
            ICountryCodeRepository _countryCodeRepository = RepositoryFactory.Create<ICountryCodeRepository>(ContextTypes.EntityFramework);
            CountryCode countryCode = _countryCodeRepository.Find(x => x.Id == obj.CountryCode).FirstOrDefault();
            EmailSender _emailSender = new EmailSender();
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);

            string secretaryId = creatIdPrix(obj) + countryCode.CountryCodes + "-" + _emailSender.Get();

            obj.SecretaryId = secretaryId;
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            var result = _secretaryRepo.Insert(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
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
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            return Ok(_secretaryRepo.Find(x => x.SecretaryId == secretaryId));
        }


        [Route("api/secretary/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Secretary/5
        public HttpResponseMessage Update(Secretary obj)
        {
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            var result = _secretaryRepo.Update(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/secretary/delete/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Secretary/5
        public HttpResponseMessage Delete(int id)
        {
            ISecretaryRepository _secretaryRepo = RepositoryFactory.Create<ISecretaryRepository>(ContextTypes.EntityFramework);
            var result = _secretaryRepo.Delete(id);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
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
