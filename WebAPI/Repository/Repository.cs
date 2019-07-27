using WebAPI.Entity;
using WebAPI.Models;
using WebAPI.Repository;

namespace NoorCare.Repository
{
    public class ClientDetailRepository : EFRepositoryBase<NoorCareDbContext, ClientDetail, int>, IClientDetailRepository{}
    public class FacilityRepository : EFRepositoryBase<NoorCareDbContext, Facility, int>, IFacilityRepository{}
    public class DiseaseRepository : EFRepositoryBase<NoorCareDbContext, Disease, int>, IDiseaseRepository{}
    public class EmergencyContactRepository : EFRepositoryBase<NoorCareDbContext, EmergencyContact, int>, IEmergencyContactRepository{}
    public class MedicalInformationRepository : EFRepositoryBase<NoorCareDbContext, MedicalInformation, int>, IMedicalInformationRepository{}
    public class CountryCodeRepository : EFRepositoryBase<NoorCareDbContext, CountryCode, int>, ICountryCodeRepository { }
    public class CityRepository : EFRepositoryBase<NoorCareDbContext, TblCity, int>, ICityRepository { }
    public class CountryRepository : EFRepositoryBase<NoorCareDbContext, TblCountry, int>, ICountryRepository { }
    public class StateRepository : EFRepositoryBase<NoorCareDbContext, State, int>, IStateRepository { }
    public class InsuranceInformationRepository : EFRepositoryBase<NoorCareDbContext, InsuranceInformation, int>, IInsuranceInformationRepository { }
    public class QuickHealthRepository : EFRepositoryBase<NoorCareDbContext, QuickHeathDetails, int>, IQuickHealthRepository { }
    public class HospitalDetailsRepository : EFRepositoryBase<NoorCareDbContext, HospitalDetails, int>, IHospitalDetailsRepository { }
    public class QuickUploadRepository : EFRepositoryBase<NoorCareDbContext, QuickUpload, int>, IQuickUploadRepository { }
    public class DoctorRepository : EFRepositoryBase<NoorCareDbContext,Doctor,int>, IDoctorRepository { }
    public class SecretaryRepository : EFRepositoryBase<NoorCareDbContext, Secretary, int>, ISecretaryRepository { }
}


