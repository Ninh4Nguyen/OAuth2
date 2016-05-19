using AuthorizationServer.API.Models;
using System.Collections.Generic;

namespace AuthorizationServer.API.Services
{
    public interface IAudienceServices
    {
        Audience GetById(string id);
        IEnumerable<Audience> GetAll();
        Audience Create(string name);
        bool Update(Audience client);
        bool Delete(string id);
    }
}
