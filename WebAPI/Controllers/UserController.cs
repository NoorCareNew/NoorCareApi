using NoorCare.Data.Infrastructure;
using NoorCare.Data.Repositories;
using NoorCare.Web.Infrastructure.Core;
using NoorCare.WebAPI.Entity;
using System.Linq;
using System.Web.Http;

namespace NoorCare.WebAPI.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<EmergencyContact> _emergencyContactRepo;
        private readonly IEntityBaseRepository<MedicalInformation> _medicalInformationRepo;
        private readonly IEntityBaseRepository<InsuranceInformation> _insuranceInformationRepo;
        public UserController(IEntityBaseRepository<EmergencyContact> emergencyContactRepo,
                              IEntityBaseRepository<MedicalInformation> medicalInformationRepo,
                              IEntityBaseRepository<InsuranceInformation> insuranceInformationRepo,
                              IEntityBaseRepository<Error> _errorsRepository,
                              IUnitOfWork _unitOfWork): base(_errorsRepository, _unitOfWork) {
            _emergencyContactRepo = emergencyContactRepo;
            _medicalInformationRepo = medicalInformationRepo;
            _insuranceInformationRepo = insuranceInformationRepo;
        }

        /// <summary>
        /// Emergency Contact Detail
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="emergencyContact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/add/emergencyContact/{ClientId}")]
        public IHttpActionResult AddEmergencyContact(string ClientId, EmergencyContact emergencyContact)
        {
            EmergencyContact _clientDetail = new EmergencyContact
            {
                clientId = ClientId,
                FirstName = emergencyContact.FirstName,
                LastName = emergencyContact.LastName,
                Gender = emergencyContact.Gender,
                Relationship = emergencyContact.Relationship,
                Email = emergencyContact.Email,
                Mobile = emergencyContact.Mobile,
                AlternateNumber = emergencyContact.AlternateNumber,
                WorkNumber = emergencyContact.WorkNumber,
                Address = emergencyContact.Address,
            };
            _emergencyContactRepo.Add(_clientDetail);
            _unitOfWork.Commit();
            return Ok(_clientDetail);
        }

        [HttpPost]
        [Route("api/user/update/emergencyContact/{ClientId}")]
        public IHttpActionResult UpdateEmergencyContact(string ClientId, EmergencyContact emergencyContact)
        {
            EmergencyContact eContact = _emergencyContactRepo.FindBy(x => x.clientId == ClientId).FirstOrDefault();

            eContact.FirstName = emergencyContact.FirstName;
            eContact.LastName = emergencyContact.LastName;
            eContact.Gender = emergencyContact.Gender;
            eContact.Relationship = emergencyContact.Relationship;
            eContact.Email = emergencyContact.Email;
            eContact.Mobile = emergencyContact.Mobile;
            eContact.AlternateNumber = emergencyContact.AlternateNumber;
            eContact.WorkNumber = emergencyContact.WorkNumber;
            eContact.Address = emergencyContact.Address;
            _emergencyContactRepo.Edit(eContact);
            _unitOfWork.Commit();
            return Ok(eContact);
        }

        [HttpGet]
        [Route("api/user/get/emergencycontact/{ClientId}")]
        public IHttpActionResult getEmergencyContact(string ClientId)
        {
            EmergencyContact _emContact = _emergencyContactRepo.FindBy(x => x.clientId == ClientId).FirstOrDefault();
            return Ok(_emContact);
        }

        /// <summary>
        /// Medical Information
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="emergencyContact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/add/medicalinfo/{ClientId}")]
        public IHttpActionResult AddMedicalInformation(string ClientId, MedicalInformation medicalInformation)
        {
            MedicalInformation _medicalInformation = new MedicalInformation
            {
                clientId = ClientId,
                Hight = medicalInformation.Hight,
                Wight = medicalInformation.Wight,
                BloodGroup = medicalInformation.BloodGroup,
                Diseases = medicalInformation.Diseases,
                AnyAllergies = medicalInformation.AnyAllergies,
                AnyHealthCrisis = medicalInformation.AnyHealthCrisis,
                AnyRegularMedication = medicalInformation.AnyRegularMedication,
                Disability = medicalInformation.Disability,
                Smoke = medicalInformation.Smoke,
                Drink = medicalInformation.Drink,
                OtherDetails = medicalInformation.OtherDetails
            };
            _medicalInformationRepo.Add(_medicalInformation);
            _unitOfWork.Commit();
            return Ok(_medicalInformation);
        }

        [HttpPost]
        [Route("api/user/update/medicalinfo/{ClientId}")]
        public IHttpActionResult UpdateMedicalInformation(string ClientId, MedicalInformation medicalInformation)
        {
            MedicalInformation mInfo = _medicalInformationRepo.FindBy(x => x.clientId == ClientId).FirstOrDefault();

            mInfo.Hight = medicalInformation.Hight;
            mInfo.Wight = medicalInformation.Wight;
            mInfo.BloodGroup = medicalInformation.BloodGroup;
            mInfo.Diseases = medicalInformation.Diseases;
            mInfo.AnyAllergies = medicalInformation.AnyAllergies;
            mInfo.AnyHealthCrisis = medicalInformation.AnyHealthCrisis;
            mInfo.AnyRegularMedication = medicalInformation.AnyRegularMedication;
            mInfo.Disability = medicalInformation.Disability;
            mInfo.Smoke = medicalInformation.Smoke;
            mInfo.Drink = medicalInformation.Drink;
            mInfo.OtherDetails = medicalInformation.OtherDetails;
            _medicalInformationRepo.Edit(mInfo);
            _unitOfWork.Commit();
            return Ok(mInfo);
        }

        [HttpGet]
        [Route("api/user/get/medicalinfo/{ClientId}")]
        public IHttpActionResult getMedicalInformationt(string ClientId)
        {
            MedicalInformation _miContact = _medicalInformationRepo.FindBy(x => x.clientId == ClientId).FirstOrDefault();
            return Ok(_miContact);
        }


        [HttpGet]
        [Route("api/user/get/insuranceinfo/{ClientId}")]
        public IHttpActionResult getInsurancenformationt(string ClientId)
        {
            InsuranceInformation _insuranceContact = _insuranceInformationRepo.FindBy(x => x.ClientId == ClientId).FirstOrDefault();
            return Ok(_insuranceContact);
        }

        [HttpPost]
        [Route("api/user/add/insuranceinfo/{ClientId}")]
        [AllowAnonymous]
        public IHttpActionResult AddInsurancenformationt(string ClientId, InsuranceInformation insuranceInformation)
        {
            InsuranceInformation _insuranceInformation = new InsuranceInformation
            {
                ClientId = ClientId,
                CompanyName = insuranceInformation.CompanyName,
                InsuraceNo = insuranceInformation.InsuraceNo
            };
            _insuranceInformationRepo.Add(_insuranceInformation);
            _unitOfWork.Commit();
            return Ok(_insuranceInformation);
        }

        [HttpPost]
        [Route("api/user/update/insuranceinfo/{ClientId}")]
        public IHttpActionResult UpdateInsurancenformationt(string ClientId, InsuranceInformation insuranceInformation)
        {
            InsuranceInformation _insuranceInformation = _insuranceInformationRepo.FindBy(x => x.ClientId == ClientId).FirstOrDefault();

            _insuranceInformation.ClientId = ClientId;
            _insuranceInformation.CompanyName = insuranceInformation.CompanyName;
            _insuranceInformation.InsuraceNo = insuranceInformation.InsuraceNo;
            _insuranceInformationRepo.Edit(_insuranceInformation);
            _unitOfWork.Commit();
            return Ok(_insuranceInformation);
        }

    }
}
