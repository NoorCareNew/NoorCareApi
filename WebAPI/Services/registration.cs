using NoorCare.WebAPI.Models;
using System;

namespace NoorCare.WebAPI.Services
{
    public class Registration
    {
        public ClientDetail ClientDetail(string clientId, AccountModel model)
        {
            ClientDetail _clientDetail = new ClientDetail
            {
                ClientId = clientId,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                MobileNo = Convert.ToInt32(model.PhoneNumber),
                EmailId = model.Email,
                Jobtype = model.jobType,
                CountryCode = model.CountryCode,
                CreatedDate = DateTime.Now,
            };
            return _clientDetail;
        }

        public HospitalDetails HospitalDetail(string clientId, AccountModel model)
        {
            HospitalDetails _hospitalDetail = new HospitalDetails
            {
                HospitalId = clientId,
                HospitalName = model.FirstName,
                Email = model.Email,
                jobType = model.jobType,
                Mobile = Convert.ToInt32(model.PhoneNumber),
                FacilityId = model.FacilityId
            };
            return _hospitalDetail;
        }

        public string creatIdPrix(AccountModel model)
        {
            string priFix = "NCM-";
            if (model.jobType == 2)
            {
                priFix = "NCH-";
            }
            else if (model.Gender == 1 && model.jobType == 1)
            {
                priFix = "NCM-";
            }
            else if (model.Gender == 2 && model.jobType == 1)
            {
                priFix = "NCF-";
            }
            return priFix;
        }
    }
}