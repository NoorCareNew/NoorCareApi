using NoorCare.Repository;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using WebAPI.Repository;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class HospitalDetailsController : ApiController
    {
        IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
        ICountryCodeRepository _countryCodeRepository = RepositoryFactory.Create<ICountryCodeRepository>(ContextTypes.EntityFramework);
        Registration _registration = new Registration();
        [Route("api/hospitaldetails/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails
        public HttpResponseMessage GetAll()
        {
            var result =_hospitaldetailsRepo.GetAll().ToList();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/hospitaldetails/getdetail/{hospitalid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails/5
        public HttpResponseMessage GetDetail(string hospitalid)
        {
            var result = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalid).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);            
        }

        [HttpPost]
        [Route("api/hospitaldetails/uploadprofilepic")]
        [AllowAnonymous]
        public IHttpActionResult UploadProfilePic()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            string hospitalId = httpRequest.Form["HospitalId"];
            try
            {
                var postedFile = httpRequest.Files["Image"];
                if (postedFile != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).
                        Take(10).ToArray()).
                        Replace(" ", "-");
                    imageName = hospitalId + "." + ImageFormat.Jpeg;
                    var filePath = HttpContext.Current.Server.MapPath("~/ProfilePic/Hospital/" + imageName);
                    bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ProfilePic/Hospital/" + imageName));
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
            return Ok(hospitalId);
        }

        [HttpGet]
        [Route("api/hospitaldetails/profile/{hospitalId}")]
        [AllowAnonymous]
        public IHttpActionResult getHospitalDetailsProfile(string hospitalId)
        {
            return Ok(_hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId));
        }

        
        [Route("api/hospitaldetails/delete/{hospitalid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/HospitalDetails/5
        public HttpResponseMessage Delete(string hospitalid)
        {
            int tblId= getTableId(hospitalid);
            var result = _hospitaldetailsRepo.Delete(tblId);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        // Update Hospital Details
        [Route("api/hospitaldetails/{hospitalId}/updatehospital")]
        [HttpPatch]
        [AllowAnonymous]
        // PUT: api/UpdateHospitalProfile/5
        public IHttpActionResult UpdateHospitalProfile(string hospitalId, Delta<HospitalDetails> obj)
        {

            HospitalDetails _hospitalDetails = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId).FirstOrDefault();
            obj.Patch(_hospitalDetails);
            var result = _hospitaldetailsRepo.Update(_hospitalDetails);
            return Ok(result);
        }


        private int getTableId(string hospitalId)
        {
            var result = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId).FirstOrDefault();

            return result.Id;
        }
    }
}
