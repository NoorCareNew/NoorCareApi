using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using NoorCare.Data.Repositories;
using NoorCare.WebAPI.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoorCare.WebAPI
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        //IClientDetailRepository _clientDetailRepo = RepositoryFactory.Create<IClientDetailRepository>(ContextTypes.EntityFramework);
        //IHospitalDetailsRepository _hospitalDetailsRepository = RepositoryFactory.Create<IHospitalDetailsRepository>(ContextTypes.EntityFramework);

        private readonly IEntityBaseRepository<ClientDetail> _clientDetailRepo;
        private readonly IEntityBaseRepository<HospitalDetails> _hospitalDetailsRepository;

        //ApplicationOAuthProvider(IEntityBaseRepository<ClientDetail> clientDetailRepo,
        //                            IEntityBaseRepository<HospitalDetails> hospitalDetailsRepository)
        //{
        //    _clientDetailRepo = clientDetailRepo;
        //    _hospitalDetailsRepository = hospitalDetailsRepository;
        //}

        ClientDetail clientDetailRepo = null;
        HospitalDetails hospitalDetails;
        int jobType = 1;
        bool isEmailConfirmed = false;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userStore = new UserStore<ApplicationUser>(new NoorCareDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            var userFindByEmail = manager.FindByEmail(context.UserName);
            var user = userFindByEmail != null ? await manager.FindAsync(userFindByEmail.UserName, context.Password)
                : await manager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("Please check username and password");
            }
            else
            {
                if (user.Id.Contains("NCH")) {
                    hospitalDetails = _hospitalDetailsRepository.FindBy(x => x.HospitalId.Trim() == user.Id.Trim()).FirstOrDefault();
                    jobType = hospitalDetails.jobType;
                    isEmailConfirmed = hospitalDetails.EmailConfirmed;
                } else {
                    clientDetailRepo = _clientDetailRepo.FindBy(x => x.ClientId.Trim() == user.Id.Trim()).FirstOrDefault();
                    jobType = clientDetailRepo.Jobtype;
                    isEmailConfirmed = clientDetailRepo.EmailConfirmed;
                }
                if (!isEmailConfirmed)
                {
                    context.SetError("Please verify your email address");
                }
                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("UserId", user.Id));
                    identity.AddClaim(new Claim("Username", user.UserName));
                    identity.AddClaim(new Claim("Email", user.Email));
                    identity.AddClaim(new Claim("FirstName", user.FirstName));
                    identity.AddClaim(new Claim("LastName", user.LastName));
                    identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                    identity.AddClaim(new Claim("PhoneNo", user.PhoneNumber == null? " " : user.PhoneNumber));
                    identity.AddClaim(new Claim("JobType", jobType.ToString()));
                    context.Validated(identity);
                }
            }
        }
    }
}