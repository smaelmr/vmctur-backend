﻿using System;
using VMCTur.Common.Validation;
using VMCTur.Common.Resources;

namespace VMCTur.Domain.Entities.Users
{
    public class User
    {
        #region Properties

        public int CompanyId { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        #endregion

        #region Ctor

        protected User() { }

        public User(int companyId, int id, string name, string email)
        {
            CompanyId = companyId;
            this.Id = id;
            this.Name = name;
            this.Email = email;

        }

        #endregion

        #region Properties

        public void SetPassword(string password, string confirmPassword)
        {
            AssertionConcern.AssertArgumentNotNull(password, Errors.InvalidPassword);
            AssertionConcern.AssertArgumentNotNull(confirmPassword, Errors.InvalidPassword);
            AssertionConcern.AssertArgumentEquals(password, confirmPassword, Errors.PasswordDoesNotMatch);
            AssertionConcern.AssertArgumentLength(password, 6, 20, Errors.InvalidPassword);

            this.Password = PasswordAssertionConcern.Encrypt(password);
        }

        public string ResetPassword()
        {
            string password = Guid.NewGuid().ToString().Substring(0, 8);
            this.Password = PasswordAssertionConcern.Encrypt(password);

            return password;
        }

        public void ChangeName(string name)
        {
            this.Name = name;
        }

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidUserName);
            EmailAssertionConcern.AssertIsValid(this.Email);
            PasswordAssertionConcern.AssertIsValid(this.Password);
        }

        #endregion
    }
}
