using AuthorizationServer.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace AuthorizationServer.API.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private OAuth2DbContext _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Role> _roleRepository;
        private GenericRepository<Audience> _audienceRepository;

        public UnitOfWork()
        {
            _context = new OAuth2DbContext();
        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new GenericRepository<Role>(_context);
                }
                return _roleRepository;
            }
        }
        public GenericRepository<Audience> AudienceRepository
        {
            get
            {
                if (_audienceRepository == null)
                {
                    _audienceRepository = new GenericRepository<Audience>(_context);
                }
                return _audienceRepository;
            }
        }
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, 
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if((!disposed) && disposing)
            {
               _context.Dispose();
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}