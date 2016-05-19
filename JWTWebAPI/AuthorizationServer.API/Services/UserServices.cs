using AuthorizationServer.API.Models;
using AuthorizationServer.API.Repository;
using System.Collections.Generic;
using System.Transactions;

namespace AuthorizationServer.API.Services
{
    public class UserServices : IUserServices
    {
        private IUnitOfWork _unitOfWork;
        public UserServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public User GetById(int id)
        {
            return _unitOfWork.UserRepository.GetById(id);
        }
        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll();
        }
        public int Create(User user)
        {
            using(var scope = new TransactionScope())
            {
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                return user.UserId;
            }
        }
        public bool Update(User user)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }
        public bool Delete(int id)
        {
            bool success = false;
            using (var scope = new TransactionScope())
            {
                _unitOfWork.UserRepository.Delete(id);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }
        public User FindUser(string userName, string password)
        {
            return _unitOfWork.UserRepository.Get(u => u.UserName.Equals(userName) && u.Password.Equals(password));
        }
    }
}