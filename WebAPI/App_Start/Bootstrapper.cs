using System.Web.Http;
using AutoMapper;

namespace NoorCare.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            // Configure Autofac
            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
            //Configure AutoMapper
           // AutoMapperConfiguration.Configure();
        }
    }

    //public class AutoMapperConfiguration
    //{
    //    public static void Configure()
    //    {
    //        Mapper.Initialize(x =>
    //        {
    //            x.AddProfile<DomainToViewModelMappingProfile>();
    //        });
    //    }
    //}
}