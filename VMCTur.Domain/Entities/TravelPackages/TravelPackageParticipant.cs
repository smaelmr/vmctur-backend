using System;
using VMCTur.Common.Resources;
using VMCTur.Common.Standard;
using VMCTur.Common.Validation;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TravelPackageParticipant
    {
        #region Properties

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string NumberDocument { get; private set; }
        public DateTime BirthDate { get; private set; }
        public int TravelPackageId { get; private set; }
        public TravelPackage TravelPackage { get; private set; }
        public AgeGroup AgeGoupBelong { get; private set; }
        public bool Paying { get; private set; }

        public string AgeGoupBelongDisplay
        {
            get
            {
                return Standard.ObterDescricaoEnum(AgeGoupBelong);
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

        protected TravelPackageParticipant()
        { }

        public TravelPackageParticipant(int id, string name, string numberDocument, DateTime birthDate, AgeGroup ageGroupBelong, bool paying, int travelPackageId)
        {
            Id = id;
            Name = name;
            NumberDocument = numberDocument;
            BirthDate = birthDate;
            AgeGoupBelong = ageGroupBelong;
            Paying = paying;
            TravelPackageId = travelPackageId;
        }
       
        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidName);
            
            //BirthdayAssertionConcern.AssertIsValid(this.BirthDate);
            
            //if (this.AgeGoupBelong == AgeGroup.Idoso)
            //    AssertionConcern.AssertArgumentNotEmpty(this.NumberDocument, "Número do documento deve ser informado para partipante idoso.");
        }

        public AgeGroup VerificaFaixaEtaria()
        {
            if (Age <= 12)
                return AgeGroup.Crianca;
            else if (Age > 12 && Age < 60)
                return AgeGroup.Adulto;
            else
                return AgeGroup.Idoso;

        }

        public void SetId(long id)
        {
            Id = Int32.Parse(id.ToString());
        }

        #endregion
    }
}
