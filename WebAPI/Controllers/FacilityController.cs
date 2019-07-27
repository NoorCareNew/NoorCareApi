using NoorCare.Data.Infrastructure;
using NoorCare.Data.Repositories;
using NoorCare.Web.Infrastructure.Core;
using NoorCare.WebAPI.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NoorCare.WebAPI.Controllers
{
    public class FacilityController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Facility> _facilityDetailRepo;
        private readonly IEntityBaseRepository<Disease> _diseaseDetailRepo;
        private readonly IEntityBaseRepository<TblCity> _cityRepository;
        private readonly IEntityBaseRepository<TblCountry> _countryRepository;
        private readonly IEntityBaseRepository<CountryCode> _countryCodeRepository;
        public FacilityController(IEntityBaseRepository<Facility> facilityDetailRepo,
                                    IEntityBaseRepository<Disease> diseaseDetailRepo,
                                    IEntityBaseRepository<TblCity> cityRepository,
                                    IEntityBaseRepository<TblCountry> countryRepository,
                                    IEntityBaseRepository<CountryCode> countryCodeRepository,
        IEntityBaseRepository<Error> _errorsRepository,
                                    IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _facilityDetailRepo = facilityDetailRepo;
            _diseaseDetailRepo = diseaseDetailRepo;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _countryCodeRepository = countryCodeRepository;
        }
        [Route("api/facility")]
        [HttpGet]
        [AllowAnonymous]
        public List<Facility> GetFacility()
        {
            return _facilityDetailRepo.GetAll().ToList();
        }

        [Route("api/diseaseType")]
        [HttpGet]
        [AllowAnonymous]
        public List<Disease> GetDisease()
        {
            return _diseaseDetailRepo.GetAll().ToList();
        }

        [Route("api/countryCode")]
        [HttpGet]
        [AllowAnonymous]
        public List<CountryCode> GetCountryCode()
        {
            return _countryCodeRepository.GetAll().ToList();
        }

        [Route("api/city/{countryId}")]
        [HttpGet]
        [AllowAnonymous]
        public List<TblCity> GetCity(int countryId)
        {
            return _cityRepository.FindBy(x=>x.CountryId == countryId).ToList();
        }

        [Route("api/countries")]
        [HttpGet]
        [AllowAnonymous]
        public List<TblCountry> GetCountries()
        {
            return _countryRepository.GetAll().ToList();
        }
    }
}
