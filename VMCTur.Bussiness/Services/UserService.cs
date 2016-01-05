using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Models.Users;

namespace VMCTur.Bussiness.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public User Authenticate(string email, string password)
        {
            var user = GetByEmail(email);

            if (user.Password != PasswordAssertionConcern.Encrypt(password))
                throw new Exception(Errors.InvalidCredentials);

            return user;
        }

        public void ChangeInformation(string email, string name)
        {
            var user = GetByEmail(email);

            user.ChangeName(name);
            user.Validate();

            _repository.Update(user);
        }

        public void ChangePassword(string email, string password, string newPassword, string confirmNewPassword)
        {
            var user = Authenticate(email, password);

            user.SetPassword(newPassword, confirmNewPassword);
            user.Validate();

            _repository.Update(user);
        }

        public void Register(int empresaId, string name, string email, string password, string confirmPassword)
        {
            var hasUser = _repository.Get(email);
            if (hasUser != null)
                throw new Exception(Errors.DuplicateEmail);

            var user = new User(empresaId, name, email);
            user.SetPassword(password, confirmPassword);
            user.Validate();

            _repository.Create(user);
        }        

        public User GetByEmail(string email)
        {
            var user = _repository.Get(email);

            if (user == null)
                throw new Exception(Errors.UserNotFound);

            return user;
        }

        public List<User> GetByRange(int skip, int take)
        {
            return _repository.Get(skip, take);
        }        

        public string ResetPassword(string email)
        {
            var user = GetByEmail(email);
            var password = user.ResetPassword();
            user.Validate();

            _repository.Update(user);
            return password;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
