using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Enums;

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

        public AgeRange AgeRangeCustomer
        {
            get
            {
                return AgeRangeManager();
            }
        
        }

        /// <summary>
        /// Smael: calculates the student's age.
        /// </summary>
        public int Age
        {
            get
            {
                int idade = 0;

                int anos = (DateTime.Today.Year - 1) - BirthDate.Year;
                int meses = (DateTime.Today.Month) - BirthDate.Month;
                int dias = (DateTime.Today.Day) - BirthDate.Day;

                if (meses < 0) //Smael: se meses for menor que zero signifia que o aluno ainda não fez aniversário no ano corrente.                
                    idade = anos;
                else if (meses > 0) //Smael: se meses for maior significa que o aluno já fez aniversário no ano corrente, e soma anos + 1.
                    idade = anos + 1;
                else //Smael: caso meses seja igual a zero, significa que estamos no mes do aniversário do aluno, neste caso precisamos verificar os dias
                {
                    if (dias < 0) //Smael: se dias for menor que zero signifia que o aluno ainda não chegou o dia do aniversário no mes corrente.                
                        idade = anos;
                    else if (dias > 0) //Smael: se dias for maior que zero signifia que o aluno já fez aniversário e soma anos + 1.                
                        idade = anos + 1;
                    else // Smael: neste caso estamos no dia do aniversário do aluno. Soma anos + 1, assim como a instrução acima, porém podemos notificar... (dar parabéns ao aluno)
                        idade = anos + 1;
                }

                return idade;
            }
        }

        #endregion

        #region Ctor

        protected Customer()
        { }

        public Customer(int id, int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments)
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

        #region Properties

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidName);
            EmailAssertionConcern.AssertIsValid(this.Email);
            PhoneNumberAssertionConcern.AssertIsValid(this.PhoneNumber);
            BirthdayAssertionConcern.AssertIsValid(this.BirthDate);
        }

        public AgeRange AgeRangeManager()
        {
            if (Age <= 12)
                return AgeRange.Child;
            else if (Age > 12 && Age < 60)
                return AgeRange.Adult;
            else
                return AgeRange.Old;            
            
        }

        
        

        #endregion

    }
}
