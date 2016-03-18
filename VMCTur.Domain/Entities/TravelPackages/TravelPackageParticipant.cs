using System;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;

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

        #endregion

        #region Ctor

        protected TravelPackageParticipant()
        { }

        public TravelPackageParticipant(int id, string name, string numberDocument, DateTime birthDate, int travelPackageId)
        {
            Id = id;
            Name = name;
            NumberDocument = numberDocument;
            BirthDate = birthDate;
            TravelPackageId = travelPackageId;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidName);
            BirthdayAssertionConcern.AssertIsValid(this.BirthDate);
            AssertionConcern.AssertArgumentNotEmpty(this.NumberDocument, "Número do documento deve ser informado.");
        }
    


        #endregion
    }
}
