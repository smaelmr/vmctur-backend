using System;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Entities.Clientes
{
    public class Cliente
    {
        #region Properties

        public int Id { get; private set; }
        public int EmpresaId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Fone { get; private set; }
        public string Rg { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Obs { get; private set; }

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
        public int Idade
        {
            get
            {
                int idade = 0;

                int anos = (DateTime.Today.Year - 1) - DataNascimento.Year;
                int meses = (DateTime.Today.Month) - DataNascimento.Month;
                int dias = (DateTime.Today.Day) - DataNascimento.Day;

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

        protected Cliente()
        { }

        public Cliente(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            Id = id;
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Fone = fone;
            Rg = rg;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Obs = obs;
        }

        #endregion

        #region Atributes

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Nome, 3, 100, Errors.InvalidName);
            EmailAssertionConcern.AssertIsValid(this.Email);
            PhoneNumberAssertionConcern.AssertIsValid(this.Fone);
            BirthdayAssertionConcern.AssertIsValid(this.DataNascimento);
        }

        public FaixaEtaria VerificaFaixaEtaria()
        {
            if (Idade <= 12)
                return FaixaEtaria.Crianca;
            else if (Idade > 12 && Idade < 60)
                return FaixaEtaria.Adulto;
            else
                return FaixaEtaria.Idoso;            
            
        }
             
        #endregion

    }
}
