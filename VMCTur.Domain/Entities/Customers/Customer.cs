using System;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Entities.Customers
{
    public class Customer
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Rg { get; private set; }
        public string Cpf { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Comments { get; private set; }
        public bool Inactive { get; private set; }
        public FaixaEtaria FaixaEtariaCliente
        {
            get
            {
                return VerificaFaixaEtaria();
            }
        
        }

        /// <summary>
        /// Smael: Calcula a idade do cliente a partir da data de nascimento.
        /// </summary>
        public int Age
        {
            get
            {
                int age = 0;

                int years = (DateTime.Today.Year - 1) - BirthDate.Year;
                int months = (DateTime.Today.Month) - BirthDate.Month;
                int days = (DateTime.Today.Day) - BirthDate.Day;

                if (months < 0) //Smael: se meses for menor que zero signifia que o aluno ainda não fez aniversário no ano corrente.                
                    age = years;
                else if (months > 0) //Smael: se meses for maior significa que o aluno já fez aniversário no ano corrente, e soma anos + 1.
                    age = years + 1;
                else //Smael: caso meses seja igual a zero, significa que estamos no mes do aniversário do aluno, neste caso precisamos verificar os dias
                {
                    if (days < 0) //Smael: se dias for menor que zero signifia que o aluno ainda não chegou o dia do aniversário no mes corrente.                
                        age = years;
                    else if (days > 0) //Smael: se dias for maior que zero signifia que o aluno já fez aniversário e soma anos + 1.                
                        age = years + 1;
                    else // Smael: neste caso estamos no dia do aniversário do aluno. Soma anos + 1, assim como a instrução acima, porém podemos notificar... (dar parabéns ao aluno)
                        age = years + 1;
                }

                return age;
            }
        }

        #endregion

        #region Ctor

        protected Customer()
        { }

        public Customer(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            Id = id;
            CompanyId = empresaId;
            Name = nome;
            Email = email;
            Phone = fone;
            Rg = rg;
            Cpf = cpf;
            BirthDate = dataNascimento;
            Comments = obs;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidName);
            EmailAssertionConcern.AssertIsValid(this.Email);
            PhoneNumberAssertionConcern.AssertIsValid(this.Phone);
            BirthdayAssertionConcern.AssertIsValid(this.BirthDate);
        }

        public FaixaEtaria VerificaFaixaEtaria()
        {
            if (Age <= 12)
                return FaixaEtaria.Crianca;
            else if (Age > 12 && Age < 60)
                return FaixaEtaria.Adulto;
            else
                return FaixaEtaria.Idoso;            
            
        }
             
        #endregion

    }
}
