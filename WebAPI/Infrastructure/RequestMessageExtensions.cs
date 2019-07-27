using NoorCare.Data.Repositories;
using NoorCare.Entity;
using System.Net.Http;
using System.Web.Http.Dependencies;

namespace NoorCare.Web.Infrastructure.Extensions
{
    public static class RequestMessageExtensions
    {
        internal static IEntityBaseRepository<T> GetDataRepository<T>(this HttpRequestMessage request) where T : class, 
            IEntityBase, new()
        {
            return request.GetService<IEntityBaseRepository<T>>();
        }

        private static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService)dependencyScope.GetService(typeof(TService));
            return service;
        }
    }
}