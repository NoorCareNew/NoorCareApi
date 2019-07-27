using NoorCare.Data.Infrastructure;
using NoorCare.Data.Repositories;
using NoorCare.Web.Infrastructure.Core;
using NoorCare.WebAPI.Entity;
using System.Linq;
using System.Web.Http;

namespace NoorCare.WebAPI.Controllers
{
    public class QuickHealthReportController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<HospitalDetails> _hospitalDetailsRepository;
        private readonly IEntityBaseRepository<QuickHeathDetails> _quickHeathDetailsRepository;
        private readonly IEntityBaseRepository<QuickUpload> _quickUploadRepository;
        public QuickHealthReportController(IEntityBaseRepository<HospitalDetails> hospitalDetailsRepository,
                                        IEntityBaseRepository<QuickHeathDetails> quickHeathDetailsRepository,
                                         IEntityBaseRepository<QuickUpload> quickUploadRepository,
                                    IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _hospitalDetailsRepository = hospitalDetailsRepository;
            _quickHeathDetailsRepository = quickHeathDetailsRepository;
            _quickUploadRepository = quickUploadRepository;
        }

        [HttpGet]
        [Route("api/user/get/quickHealth/{ClientId}")]
        [AllowAnonymous]
        public IHttpActionResult getQuickHealth(string ClientId)
        {
            QuickHeathDetails _quickHeathDetails = _quickHeathDetailsRepository.FindBy(x => x.ClientId == ClientId).FirstOrDefault();
            return Ok(_quickHeathDetails);
        }

        [HttpPost]
        [Route("api/user/add/quickHealth/{ClientId}")]
        [AllowAnonymous]
        public IHttpActionResult AddQuickHeathDetails(string ClientId, QuickHeathDetails _quickHeathDetails)
        {
            QuickHeathDetails quickHeathDetails = new QuickHeathDetails
            {
                ClientId = ClientId,
                HospitalId = _quickHeathDetails.HospitalId,
                Pressure = _quickHeathDetails.Pressure,
                Heartbeats = _quickHeathDetails.Heartbeats,
                Temprature = _quickHeathDetails.Temprature,
                Sugar = _quickHeathDetails.Sugar,
                Length = _quickHeathDetails.Length,
                Weight = _quickHeathDetails.Weight,
                Cholesterol = _quickHeathDetails.Cholesterol,
                Other = _quickHeathDetails.Other,
            };
            _quickHeathDetailsRepository.Add(quickHeathDetails);
            _unitOfWork.Commit();
            return Ok(quickHeathDetails);
        }

        [HttpPost]
        [Route("api/user/add/quickUpload/{ClientId}")]
        [AllowAnonymous]
        public IHttpActionResult AddQuickUpload(string ClientId, QuickUpload _quickUpload)
        {
            QuickUpload quickUpload = new QuickUpload
            {
                ClientId = ClientId,
                HospitalId = _quickUpload.HospitalId,
                DesiesType = _quickUpload.DesiesType,
            };
            _quickUploadRepository.Add(quickUpload);
            _unitOfWork.Commit();
            return Ok();
        }

        [HttpPost]
        [Route("api/user/add/hospitalDetail")]
        [AllowAnonymous]
        public IHttpActionResult AddHospital(HospitalDetails _hospitalDetail)
        {
            HospitalDetails _HospitalDetail = _hospitalDetailsRepository.FindBy(
                x => x.HospitalName.ToLower() == _hospitalDetail.HospitalName.ToLower()
                || x.HospitalName.ToLower() == _hospitalDetail.HospitalName.ToLower()
                || x.Mobile == _hospitalDetail.Mobile
                || x.Email.ToLower() == _hospitalDetail.Email.ToLower()
                || x.Website.ToLower() == _hospitalDetail.Website.ToLower()
                ).FirstOrDefault();
            if (_HospitalDetail == null)
            {
                HospitalDetails hospitalDetail = new HospitalDetails
                {
                    HospitalName = _hospitalDetail.HospitalName,
                    Address = _hospitalDetail.Address,
                    Mobile = _hospitalDetail.Mobile,
                    Email = _hospitalDetail.Email,
                    Website = _hospitalDetail.Website,
                };
                _hospitalDetailsRepository.Add(hospitalDetail);
                _unitOfWork.Commit();
                return Ok(hospitalDetail);
            }
            else
            {
                return Ok(_HospitalDetail);
            }
        }
    }
}
