using NoorCare.WebAPI.Models;
using System;

namespace NoorCare.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        NoorCareDbContext Init();
    }
}
