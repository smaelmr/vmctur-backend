using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;

namespace VMCTur.Domain.Entities.Financial.BillsReceive
{
    public class Receipt
    {
        #region Properties

        public int Id { get; private set; }
        public decimal Amount { get;  private set; }
        public DateTime DateCreate { get; private set; }

        public int BillReceiveId { get; private set; }
        public BillReceive Bill { get; private set; }
        public string Comments { get; private set; }

        #endregion

        #region Ctor

        public Receipt(int id, decimal amount, DateTime dateCreate, int billReceiveId, string comments)
        {
            Id = id;
            Amount = amount;
            DateCreate = dateCreate;
            BillReceiveId = billReceiveId;
            Comments = comments;            
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertIsGreaterThan(BillReceiveId, 0, "Recebimento deve pertencer a uma conta a pagar.");
        }

        #endregion
    }
}
