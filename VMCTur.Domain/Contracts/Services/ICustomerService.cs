using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Models.Customers;

namespace VMCTur.Domain.Contracts.Services
{
    public interface ICustomerService
    {
        void Create(int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments);
        void Update(int id, int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments);
        void Delete(int id);
        List<Customer> GetByRange(int skip, int take);
        List<Customer> GetBySearch(string search);
        Customer GetById(int id);

    }
}
