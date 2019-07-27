using NoorCare.WebAPI.Models;

namespace NoorCare.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        NoorCareDbContext dbContext;

        public NoorCareDbContext Init()
        {
            return dbContext ?? (dbContext = new NoorCareDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
