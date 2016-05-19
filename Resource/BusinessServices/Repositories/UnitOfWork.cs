using DataModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace BusinessServices.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private DevilDbContext _context = null;
        private GenericRepository<Product> _productRepository;
        public UnitOfWork()
        {
            _context = new DevilDbContext();
        }
        public GenericRepository<Product> ProductRepository()
        {
            if (_productRepository == null)
            {
                _productRepository = new GenericRepository<Product>(_context);
            }
            return _productRepository;
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
            if ((!disposed) && disposing)
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
