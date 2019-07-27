using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using NoorCare.Data.Repositories;
using NoorCare.WebAPI.Services;
using NoorCare.WebAPI.Models;
using NoorCare.WebAPI.Entity;
using NoorCare.API.Services;
using NoorCare.Data.Infrastructure;
using NoorCare.Web.Infrastructure.Core;

namespace NoorCare.WebAPI.Controllers
{
    public class AccountController : ApiControllerBase
    {
        EmailSender _emailSender = new EmailSender();
        Registration _registration = new Registration();
        string tokenCode = "";
        private readonly IEntityBaseRepository<ClientDetail> _clientDetailRepository;
        private readonly IEntityBaseRepository<HospitalDetails> _hospitalDetailsRepository;
        private readonly IEntityBaseRepository<CountryCode> _countryCodeRepository;

        public AccountController(IEntityBaseRepository<ClientDetail> clientDetailRepository,
                                    IEntityBaseRepository<HospitalDetails> hospitalDetailsRepository,
                                    IEntityBaseRepository<CountryCode> countryCodeRepository,
                                    IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _clientDetailRepository = clientDetailRepository;
            _hospitalDetailsRepository = hospitalDetailsRepository;
            _countryCodeRepository = countryCodeRepository;
        }

      

        [Route("api/account/register")]
        [HttpPost]
        [AllowAnonymous]
        public string Register(AccountModel model)
        {
            CountryCode countryCode = _countryCodeRepository.FindBy(x=>x.Id == model.CountryCode).FirstOrDefault();

            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            
            string clientId = _registration.creatIdPrix(model) + countryCode.CountryCodes + "-" + _emailSender.Get();
            var user = new ApplicationUser() {
                UserName = model.UserName, Email = model.Email
            };
            user.FirstName = model.FirstName;
            user.PhoneNumber = model.PhoneNumber;
            user.LastName = model.LastName;
            user.Id = clientId;
            IdentityResult result = manager.Create(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return "Registration has been Faild";
            }
            else
            {
                if (model.jobType == 1)
                {
                    _clientDetailRepository.Add(_registration.ClientDetail(clientId, model));
                    _unitOfWork.Commit();
                }
                else if (model.jobType == 2)
                {
                    _hospitalDetailsRepository.Add(_registration.HospitalDetail(clientId, model));
                    _unitOfWork.Commit();
                }
                try
                {
                    _emailSender.email_send(model.Email, user.FirstName+ " "+ user.LastName ,user.Id);
                }
                catch (Exception)
                {
                                      
                }
            }
            
            return "Registration has been done, And Account activation link" +
                        "has been sent your eamil id: " + 
                            model.Email;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [HttpGet]
        [Route("api/GetUserClaims")]
        public ViewAccount GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            ViewAccount model = new ViewAccount()
            {
                UserName = identityClaims.FindFirst("Username").Value,
                Email = identityClaims.FindFirst("Email").Value,
                FirstName = identityClaims.FindFirst("FirstName").Value,
                LastName = identityClaims.FindFirst("LastName").Value,
                ClientId = identityClaims.FindFirst("UserId").Value,
                PhoneNo = identityClaims.FindFirst("PhoneNo").Value,
                JobType = identityClaims.FindFirst("JobType").Value,
            };
            return model;
        }

        private void createDocPath(string clientId, int desiesId)
        {
            string subPath = $"ProfilePic/{clientId}";
            bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
            if (!exists)
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
        }

        [HttpPost]
        [Route("api/user/updateProfile")]
        [AllowAnonymous]
        public IHttpActionResult UpdateProfile()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            string clientId = httpRequest.Form["ClientId"];
            try
            {
                var postedFile = httpRequest.Files["Image"];
                if (postedFile != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).
                        Take(10).ToArray()).
                        Replace(" ", "-");
                    imageName = clientId + "." + ImageFormat.Jpeg;
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

            ClientDetail _clientDetail = _clientDetailRepository.FindBy(p => p.ClientId == clientId).FirstOrDefault();
            _clientDetail.FirstName = httpRequest.Form["FirstName"] == null ? _clientDetail.FirstName : httpRequest.Form["FirstName"];
            _clientDetail.LastName = httpRequest.Form["LastName"] == null ? _clientDetail.LastName : httpRequest.Form["LastName"];
            _clientDetail.PinCode = httpRequest.Form["PinCode"] == null ? _clientDetail.PinCode : Convert.ToInt16(httpRequest.Form["PinCode"]);
            _clientDetail.Gender = httpRequest.Form["Gender"] == null ? _clientDetail.Gender : Convert.ToInt16(httpRequest.Form["Gender"]);
            _clientDetail.Address = httpRequest.Form["Address"] == null ? _clientDetail.Address : httpRequest.Form["Address"];
            _clientDetail.City = httpRequest.Form["City"] == null ? _clientDetail.City : httpRequest.Form["City"];
            _clientDetail.State = httpRequest.Form["State"] == null ? _clientDetail.State : httpRequest.Form["State"];
            _clientDetail.Country = httpRequest.Form["Country"] == null ? _clientDetail.Country : httpRequest.Form["Country"];
            _clientDetail.MobileNo = httpRequest.Form["MobileNo"] == null ? _clientDetail.MobileNo : Convert.ToInt32(httpRequest.Form["MobileNo"]);
            _clientDetail.EmailId = httpRequest.Form["EmailId"] == null ? _clientDetail.EmailId : httpRequest.Form["EmailId"];
            _clientDetail.MaritalStatus = httpRequest.Form["MaritalStatus"] == null ? _clientDetail.MaritalStatus : Convert.ToInt16(httpRequest.Form["MaritalStatus"]);
            _clientDetail.DOB = httpRequest.Form["DOB"] == null ? _clientDetail.DOB : httpRequest.Form["DOB"];
            _clientDetailRepository.Edit(_clientDetail);
            _unitOfWork.Commit();
            return Ok();
        }

        [HttpGet]
        [Route("api/user/profile/{ClientId}")]
        public IHttpActionResult getProfileData(string ClientId)
        {
            return Ok(_clientDetailRepository.FindBy(x => x.ClientId == ClientId));
        }

        [HttpGet]
        [Route("api/hospital/profile/{ClientId}")]
        public IHttpActionResult gethospitalData(string ClientId)
        {
            return Ok(_hospitalDetailsRepository.FindBy(x => x.HospitalId == ClientId));
        }

        [HttpGet]
        [Route("api/user/emailverfication/{userName}")]
        [AllowAnonymous]
        public IHttpActionResult emailVerification(string userName)
        {
            if (userName.Contains("NCH"))
            {
                HospitalDetails hospitalDetails = _hospitalDetailsRepository.FindBy(x => x.HospitalId.Trim() == userName.Trim()).FirstOrDefault();
                if (hospitalDetails != null)
                {
                    hospitalDetails.EmailConfirmed = true;
                    _hospitalDetailsRepository.Edit(hospitalDetails);
                    _unitOfWork.Commit();
                }
            }
            else
            {
                ClientDetail clientDetail = _clientDetailRepository.FindBy(p => p.ClientId == userName).FirstOrDefault();
                if (clientDetail != null)
                {
                    clientDetail.EmailConfirmed = true;
                    _clientDetailRepository.Edit(clientDetail);
                    _unitOfWork.Commit();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/user/changePassword")]
        public IHttpActionResult ChangePassword(ChangePassword model)
        {
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            IdentityResult result = manager.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/user/forgetPassword")]
        [AllowAnonymous]
        public IHttpActionResult ForgetPassword(ForgetPassword model)
        {
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser cUser = manager.FindByName(model.UserName);
            string hashedNewPassword = manager.PasswordHasher.HashPassword(model.NewPassword);
            userStore.SetPasswordHashAsync(cUser, hashedNewPassword);
            return Ok();
        }

        [HttpPost]
        [Route("api/userNameExist")]
        [AllowAnonymous]
        public IHttpActionResult GetUserNameEmailIdExit(AccountModel model)
        {
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            return Ok(manager.FindByName(model.UserName));
        }

        [HttpPost]
        [Route("api/userEmailExists")]
        [AllowAnonymous]
        public IHttpActionResult GetUserEmailId(AccountModel model)
        {
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            return Ok(manager.FindByEmail(model.Email));
        }
    }
}
