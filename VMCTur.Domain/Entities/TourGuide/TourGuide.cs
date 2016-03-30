using VMCTur.Common.Resources;
using VMCTur.Common.Standard;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Enums;

namespace VMCTur.Domain.Entities.TourGuides
{
    /// <summary>
    /// Smael: guia também pode ser o motorista.
    /// </summary>
    public class TourGuide
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public TypeBondGuide BondType { get; private set; }
        public string Comments { get; private set; }
        public bool Available { get; private set; }

        public string BondTypeDisplay
        {
            get
            {
                return Standard.ObterDescricaoEnum(BondType);
            }
        
        }

        #endregion

        #region Ctor

        protected TourGuide()
        { }

        public TourGuide(int id, int companyId, string name, string cpf, TypeBondGuide bondType, bool available, string comments)
        {
            Id = id;
            CompanyId = companyId;
            Name = name;
            Cpf = cpf;
            BondType = bondType;
            Available = available;
            Comments = comments;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Name, 3, 100, Errors.InvalidName);            
        }

        #endregion
    }
}
