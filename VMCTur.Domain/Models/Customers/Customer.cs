using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Models.Customers
{
    public class Customer
    {
        #region Atributes

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Rg { get; private set; }
        public string Cpf { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Comments { get; private set; }

        #endregion

        #region Ctor

        protected Customer()
        { }

        public Customer(int id, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Rg = rg;
            Cpf = cpf;
            BirthDate = birthDate;
            Comments = comments;
        }

        #endregion

    }
}
