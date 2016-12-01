using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Users;

namespace VMCTur.Bussiness.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUserLogRepository _logRepository;

        public UserService(IUserRepository userRepository, IUserLogRepository logRepository)
        {
            _userRepository = userRepository;
            _logRepository = logRepository;
        }

        public User Authenticate(string email, string password)
        {
            var user = GetByEmail(email);

            if (user.Password != PasswordAssertionConcern.Encrypt(password))
                throw new Exception(Errors.InvalidCredentials);

            UserLog log = new UserLog(DateTime.Now, "Authenticate", user.Name);
            _logRepository.LogRegistry(log);


            return user;
        }

        public void ChangeInformation(string email, string name)
        {
            var user = GetByEmail(email);

            user.ChangeName(name);
            user.Validate();

            _userRepository.Update(user);
        }

        public void ChangePassword(string email, string password, string newPassword, string confirmNewPassword)
        {
            var user = Authenticate(email, password);

            user.SetPassword(newPassword, confirmNewPassword);
            user.Validate();

            _userRepository.Update(user);
        }

        public void Register(int empresaId, string name, string email, string password, string confirmPassword)
        {
            var hasUser = _userRepository.Get(email);
            if (hasUser != null)
                throw new Exception(Errors.DuplicateEmail);

            var user = new User(empresaId, 0, name, email);
            user.SetPassword(password, confirmPassword);
            user.Validate();

            _userRepository.Create(user);
        }        

        public User GetByEmail(string email)
        {
            var user = _userRepository.Get(email);

            if (user == null)
                throw new Exception(Errors.UserNotFound);

            return user;
        }

        public List<User> GetByRange(int skip, int take)
        {
            return _userRepository.Get(skip, take);
        }        

        public string ResetPassword(string email)
        {
            var user = GetByEmail(email);
            var password = user.ResetPassword();
            user.Validate();

            _userRepository.Update(user);
            return password;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
