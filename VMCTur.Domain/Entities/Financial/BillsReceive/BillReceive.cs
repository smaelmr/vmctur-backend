using System;
using VMCTur.Common.Standard;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Entities.Financial.BillsReceive
{
    public class BillReceive
    {
        #region Properties

        public int Id { get; private set; }

        public TravelPackage TravelPackage { get; private set; }
        public int TravelPackageId { get; private set; }

        public string CustomerName { get; private set; } 

        public decimal Amount { get; private set; }
        public decimal AmountReceived { get; private set; }

        public string Concerning { get; private set; }

        public DateTime CreateDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PayDay { get; private set; }

        public string Comments { get; private set; }

        public BillStatus Status
        {
            get
            {
                return GetStatus();
            }
        }

        public string StatusDisplay
        {
            get
            {
                return Standard.ObterDescricaoEnum(Status);
            }
        }

        #endregion

        #region Ctor

        protected BillReceive() {}

        public BillReceive(int id, DateTime createDate, int travelPackageId, decimal amount, 
                           decimal amountReceived, string concerning,
                           DateTime dueDate, DateTime? payDay, string comments)
        {
            Id = id;
            CreateDate = createDate;
            TravelPackageId = travelPackageId;
            Amount = amount;
            AmountReceived = amountReceived;
            Concerning = concerning;
            CreateDate = DateTime.Now;
            DueDate = dueDate;
            PayDay = payDay;
            Comments = comments;
        }

        #endregion

        #region Methods

        public void Receipt(DateTime payDay, decimal amountReceived)
        {
            PayDay = payDay;
            AmountReceived = amountReceived;
        }

        private BillStatus GetStatus()
        {
            //if pay day is null and due date is minor or equal that today, it's mean the status is "Em Aberto"
            if (PayDay == null && DueDate >= DateTime.Today)
                return BillStatus.EmAberto;
            //if pay day is null and due date is major that today, it's mean the status is "Em Atraso"
            else if (PayDay == null && DueDate < DateTime.Today)
                return BillStatus.EmAtraso;
            //id pay day ir diffent of null, else it's status "Quitado"
            else if (PayDay != null)
                return BillStatus.Quitado;
            else
                return BillStatus.VencendoHoje;
        }    

        public void SetCustomerName(string name)
        {
            CustomerName = name;
        }

        public void Validate()
        {            
            //AssertionConcern.AssertIsGreaterOrEqualThan(DueDate, CreateDate, "A data de vencimento deve ser maior ou igual a data de lançamento.");
            AssertionConcern.AssertIsGreaterThan(Amount, 0, "O valor do lançamento deve ser maior que zero.");
        }

        #endregion

    }
}
