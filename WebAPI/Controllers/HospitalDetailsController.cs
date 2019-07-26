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
    public class HospitalDetailsController : ApiController
    {
        [Route("api/hospitaldetails/getall")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails
        public HttpResponseMessage GetAll()
        {
            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result =_hospitaldetailsRepo.GetAll().ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/hospitaldetails/getdetail/{hospitalid}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/HospitalDetails/5
        public HttpResponseMessage GetDetail(string hospitalid)
        {
            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalid).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);            
        }

        [Route("api/hospitaldetails/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/HospitalDetails
        public HttpResponseMessage Register(HospitalDetails obj)
        {
            ICountryCodeRepository _countryCodeRepository = RepositoryFactory.Create<ICountryCodeRepository>(ContextTypes.EntityFramework);
            CountryCode countryCode = _countryCodeRepository.Find(x => x.Id == obj.CountryCode).FirstOrDefault();
            EmailSender _emailSender = new EmailSender();
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);

            string hospitalId = creatIdPrix(obj) + countryCode.CountryCodes + "-" + _emailSender.Get();

            obj.HospitalId = hospitalId;
            
            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result = _hospitaldetailsRepo.Insert(obj);
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
            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            return Ok(_hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId));
        }

        
        [Route("api/hospitaldetails/delete/{hospitalid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/HospitalDetails/5
        public HttpResponseMessage Delete(string hospitalid)
        {
            int tblId= getTableId(hospitalid);

            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result = _hospitaldetailsRepo.Delete(tblId);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        // Update Hospital Details
        [Route("api/hospitaldetails/updatehospital")]
        [HttpPatch]
        [AllowAnonymous]
        // PUT: api/UpdateHospitalProfile/5
        public IHttpActionResult UpdateHospitalProfile(HospitalDetails obj)
        {
           // var httpRequest = HttpContext.Current.Request;
            string hospitalId = obj.HospitalId;

            int tbleId = getTableId(hospitalId);
            obj.Id = tbleId;

            IHospitalDetailsRepository _HSRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);

            var oldData = _HSRepo.Find(x => x.Id == obj.Id).FirstOrDefault();

            obj.Id = oldData.Id;
            //var objNew = new HospitalDetails();
            //objNew.Id = obj.Id;
            //objNew.HospitalName = string.IsNullOrEmpty(obj.HospitalName) ? oldData.HospitalName : obj.HospitalName;

            //objNew.Address = string.IsNullOrEmpty(obj.Address) ? oldData.Address : obj.Address;
            //objNew.AlternateNumber = obj.AlternateNumber == 0 ? oldData.AlternateNumber : obj.AlternateNumber;
            //objNew.Amenities = string.IsNullOrEmpty(obj.Amenities) ? oldData.Amenities : obj.Amenities;
            //objNew.City = string.IsNullOrEmpty(obj.City) ? oldData.City : obj.City;
            //objNew.Country = string.IsNullOrEmpty(obj.Country) ? oldData.Country : obj.Country;
            //objNew.CountryCode = obj.CountryCode == 0 ? oldData.CountryCode : obj.CountryCode;
            //objNew.CreatedBy = string.IsNullOrEmpty(obj.CreatedBy) ? oldData.CreatedBy : obj.CreatedBy;
            //objNew.CreatedDate = obj.CreatedDate == null ? oldData.CreatedDate : obj.CreatedDate;
            //objNew.Email = string.IsNullOrEmpty(obj.Email) ? oldData.Email : obj.Email;
            //objNew.EmailConfirmed = obj.EmailConfirmed == false ? oldData.EmailConfirmed : obj.EmailConfirmed;
            //objNew.Emergency = obj.Emergency == false ? oldData.Emergency : obj.Emergency;
            //objNew.EstablishYear = string.IsNullOrEmpty(obj.EstablishYear) ? oldData.EstablishYear : obj.EstablishYear;
            //objNew.FacilityId = obj.FacilityId == 0 ? oldData.FacilityId : obj.FacilityId;
            //objNew.Friday = obj.Friday == false ? oldData.Friday : obj.Friday;
            //objNew.HospitalName = string.IsNullOrEmpty(obj.HospitalName) ? oldData.HospitalName : obj.HospitalName;
            //objNew.HospitalId = string.IsNullOrEmpty(obj.HospitalId) ? oldData.HospitalId : obj.HospitalId;
            //objNew.Id = obj.Id;
            //objNew.InsuranceCompanies = obj.InsuranceCompanies == "" ? oldData.InsuranceCompanies : obj.InsuranceCompanies;
            //objNew.IsDeleted = obj.IsDeleted == false ? oldData.IsDeleted : obj.IsDeleted;
            //objNew.jobType = obj.jobType == 0 ? oldData.jobType : obj.jobType;
            //objNew.Landmark = string.IsNullOrEmpty(obj.Landmark) ? oldData.Landmark : obj.Landmark;
            //objNew.MapLocation = string.IsNullOrEmpty(obj.MapLocation) ? oldData.MapLocation : obj.MapLocation;
            //objNew.Mobile = obj.Mobile == 0 ? oldData.Mobile : obj.Mobile;
            //objNew.ModifiedBy = string.IsNullOrEmpty(obj.ModifiedBy) ? oldData.ModifiedBy : obj.ModifiedBy;
            //objNew.ModifiedDate = obj.ModifiedDate == null ? oldData.ModifiedDate : obj.ModifiedDate;
            //objNew.Monday = obj.Monday == false ? oldData.Monday : obj.Monday;
            //objNew.NumberofAmbulance = obj.NumberofAmbulance == 0 ? oldData.NumberofAmbulance : obj.NumberofAmbulance;
            //objNew.NumberofBed = obj.NumberofBed == 0 ? oldData.NumberofBed : obj.NumberofBed;
            //objNew.PaymentType = string.IsNullOrEmpty(obj.PaymentType) ? oldData.PaymentType : obj.PaymentType;
            //objNew.PostCode = obj.PostCode == 0 ? oldData.PostCode : obj.PostCode;
            //objNew.Saturday = obj.Saturday == false ? oldData.Saturday : obj.Saturday;
            //objNew.Services = string.IsNullOrEmpty(obj.Services) ? oldData.Services : obj.Services;
            //objNew.Specialization = string.IsNullOrEmpty(obj.Specialization) ? oldData.Specialization : obj.Specialization;
            //objNew.Street = string.IsNullOrEmpty(obj.Street) ? oldData.Street : obj.Street;
            //objNew.Sunday = obj.Sunday == false ? oldData.Sunday : obj.Sunday;
            //objNew.Thursday = obj.Thursday == false ? oldData.Thursday : obj.Thursday;
            //if (oldData.Timing != obj.Timing)
            //{
            //    objNew.Timing = obj.Timing;
            //}
            ////   Timing = obj.Timing == false ? oldData.Timing : obj.Timing,
            //objNew.Tuesday = obj.Tuesday == false ? oldData.Tuesday : obj.Tuesday;
            //objNew.Website = string.IsNullOrEmpty(obj.Website) ? oldData.Website : obj.Website;
            //objNew.Wednesday = obj.Wednesday == false ? oldData.Wednesday : obj.Wednesday;

            IHospitalDetailsRepository _HSRepo2 = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result = _HSRepo2.Update(obj);
            return Ok(result);// Request.CreateResponse(HttpStatusCode.OK, result);

            //IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            //HospitalDetails hd = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId).FirstOrDefault();
            //hd.HospitalName= httpRequest.Form["HospitalName"] == null ? hd.HospitalName : httpRequest.Form["HospitalName"];
            //return Request.CreateResponse(HttpStatusCode.Accepted, hd);
            ////IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            //var result = _hospitaldetailsRepo.Update(obj);
            // return Request.CreateResponse(HttpStatusCode.Accepted, result);


            //IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            //HospitalDetails hd = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId).FirstOrDefault();

            //hd.HospitalName = httpRequest.Form["HospitalName"] == null ? hd.HospitalName : httpRequest.Form["HospitalName"];

            //hd.EstablishYear = httpRequest.Form["EstablishYear"] == null ? hd.EstablishYear : httpRequest.Form["EstablishYear"];
            //hd.NumberofBed = Convert.ToInt32(httpRequest.Form["NumberofBed"]) == 0 ? hd.NumberofBed : Convert.ToInt32(httpRequest.Form["NumberofBed"]);
            //hd.NumberofAmbulance = Convert.ToInt32(httpRequest.Form["NumberofAmbulance"]) == 0 ? hd.NumberofAmbulance : Convert.ToInt32(httpRequest.Form["NumberofAmbulance"]);
            //hd.Email = httpRequest.Form["Email"] == null ? hd.Email : httpRequest.Form["Email"];
            //hd.Website = httpRequest.Form["Website"] == null ? hd.Website : httpRequest.Form["Website"];

            //hd.CountryCode = Convert.ToInt32(httpRequest.Form["CountryCode"]) == 0 ? hd.CountryCode : Convert.ToInt32(httpRequest.Form["CountryCode"]);
            //hd.Mobile = Convert.ToInt32(httpRequest.Form["Mobile"]) == 0 ? hd.NumberofAmbulance : Convert.ToInt32(httpRequest.Form["Mobile"]);
            //hd.AlternateNumber = Convert.ToInt32(httpRequest.Form["AlternateNumber"]) == 0 ? hd.NumberofAmbulance : Convert.ToInt32(httpRequest.Form["AlternateNumber"]);

            //hd.PaymentType = httpRequest.Form["PaymentType"] == null ? hd.PaymentType : httpRequest.Form["PaymentType"];
            //hd.Emergency = Convert.ToBoolean(httpRequest.Form["Emergency"]) == false ? hd.Emergency : Convert.ToBoolean(httpRequest.Form["Emergency"]);
            //hd.InsuranceCompanies = httpRequest.Form["InsuranceCompanies"] == null ? hd.InsuranceCompanies : httpRequest.Form["InsuranceCompanies"];

            //return Ok(_hospitaldetailsRepo.Update(hd));
        }


        private int getTableId(string hospitalId)
        {
            IHospitalDetailsRepository _hospitaldetailsRepo = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);
            var result = _hospitaldetailsRepo.Find(x => x.HospitalId == hospitalId).FirstOrDefault();

            return result.Id;
        }

        public string creatIdPrix(HospitalDetails model)
        {
            string priFix = "NCH-";
            if (model.jobType == 3)
            {
                priFix = "NCH-";
            }

            return priFix;
        }
    }
}
