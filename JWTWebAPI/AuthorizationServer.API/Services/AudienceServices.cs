using AuthorizationServer.API.Models;
using AuthorizationServer.API.Repository;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;

namespace AuthorizationServer.API.Services
{
    public class AudienceServices : IAudienceServices
    {
        private IUnitOfWork _unitOfWork;
        public AudienceServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Audience GetById(string id)
        {
            return _unitOfWork.AudienceRepository.GetById(id);
        }
        public IEnumerable<Audience> GetAll()
        {
            return _unitOfWork.AudienceRepository.GetAll();
        }
        public Audience Create(string name)
        {
            var clientId = string.Format("client_{0}_tenilc", DateTime.Now.ToFileTime());
            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            Audience audience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            _unitOfWork.AudienceRepository.Insert(audience);
            _unitOfWork.Save();

            return audience;
        }
        public bool Update(Audience client)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                _unitOfWork.AudienceRepository.Update(client);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }
        public bool Delete(string id)
        {
            bool success = false;
            using (var scope = new TransactionScope())
            {
                _unitOfWork.AudienceRepository.Delete(id);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }
    }
}