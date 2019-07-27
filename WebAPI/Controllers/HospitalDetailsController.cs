using NoorCare.Data.Infrastructure;
using NoorCare.Data.Repositories;
using NoorCare.Web.Infrastructure.Core;
using NoorCare.WebAPI.Entity;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class HospitalDetailsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<HospitalDetails> _hospitalDetailsRepository;
        private readonly IEntityBaseRepository<Doctor> _doctorRepository;
        public HospitalDetailsController(IEntityBaseRepository<HospitalDetails> hospitalDetailsRepository,
                                        IEntityBaseRepository<Doctor> doctorRepository,
                                    IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _hospitalDetailsRepository = hospitalDetailsRepository;
            _doctorRepository = doctorRepository;
        }

        [Route("api/hospitaldetails/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails
        public IHttpActionResult GetAll()
        {
           return Ok(_hospitalDetailsRepository.GetAll().ToList());
        }

        [Route("api/hospitaldetails/getdetail/{hospitalid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails/5
        public IHttpActionResult GetDetail(string hospitalid)
        {
            return Ok(_hospitalDetailsRepository.FindBy(x => x.HospitalId == hospitalid).FirstOrDefault());
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
                    imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).
                        Take(10).ToArray()).
                        Replace(" ", "-");
                    imageName = hospitalId + "." + ImageFormat.Jpeg;
                    var filePath = HttpContext.Current.Server.MapPath("~/ProfilePic/Hospital/" + imageName);
                    bool exists = Directory.Exists(HttpContext.Current.Server.MapPath("~/ProfilePic/Hospital/" + imageName));
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
       
        [Route("api/hospitaldetails/delete/{hospitalid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/HospitalDetails/5
        public IHttpActionResult Delete(string hospitalid)
        {
            int tblId = getTableId(hospitalid);
            _hospitalDetailsRepository.Delete(_hospitalDetailsRepository.GetSingle(tblId));
            return Ok(tblId);
        }

        // Update Hospital Details
        [Route("api/hospitaldetails/updatehospital")]
        [HttpPatch]
        [AllowAnonymous]
        // PUT: api/UpdateHospitalProfile/5
        public IHttpActionResult UpdateHospitalProfile(HospitalDetails obj)
        {
            string hospitalId = obj.HospitalId;
            int tbleId = getTableId(hospitalId);
            obj.Id = tbleId;
            var oldData = _hospitalDetailsRepository.FindBy(x => x.Id == obj.Id).FirstOrDefault();
            obj.Id = oldData.Id;
            _hospitalDetailsRepository.Edit(obj);
            _unitOfWork.Commit();
            return Ok(tbleId);
        }


        private int getTableId(string hospitalId)
        {
            var result = _hospitalDetailsRepository.FindBy(x => x.HospitalId == hospitalId).FirstOrDefault();

            return result.Id;
        }
    }
}
