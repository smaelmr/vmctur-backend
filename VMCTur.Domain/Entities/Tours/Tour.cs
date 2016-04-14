using System;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;

namespace VMCTur.Domain.Entities.Tours

{
    public class Tour
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Route { get; private set; }
        public TimeSpan OpenHour { get; private set; }
        public TimeSpan CloseHour { get; private set; }
        public bool Available { get; private set; }
        public string Comments { get; private set; }

        #endregion

        #region Ctor

        protected Tour()
        { }

        public Tour(int id, int empresaId, string nome, string roteiro, TimeSpan horarioAbertura,
                       TimeSpan horarioFechamento, bool available, string obs)
        {
            Id = id;
            CompanyId = empresaId;
            Name = nome;
            Route = roteiro;
            OpenHour = horarioAbertura;
            CloseHour = horarioFechamento;
            Available = available;
            Comments = obs;
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
