using AuthorizationServer.API.Models;
using System.Collections.Generic;

namespace AuthorizationServer.API.Services
{
    public interface IUserServices
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
        int Create(User user);
        bool Update(User user);
        bool Delete(int id);
        User FindUser(string userName, string password);
    }
}
