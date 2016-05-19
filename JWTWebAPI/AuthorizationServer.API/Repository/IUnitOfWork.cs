using AuthorizationServer.API.Models;
using System;

namespace AuthorizationServer.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<User> UserRepository { get; }
        GenericRepository<Role> RoleRepository { get; }
        GenericRepository<Audience> AudienceRepository { get; }
        void Save();
        void Dispose();
    }
}
