using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Domain.Entities.Financial.BillsReceive
{
    public class BillReceive
    {
        #region Properties

        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public int CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Extra { get; private set; }
        public decimal Discount { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public string Comments { get; private set; }

        public List<Receipt> Receipts { get; private set; }

        #endregion

        #region Ctor

        public BillReceive(int id, int customerId, decimal amount, decimal extra, decimal discount, 
                           DateTime createDate, DateTime dueDate, List<Receipt> receipts, string comments)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            Extra = extra;
            Discount = discount;
            CreateDate = createDate;
            DueDate = dueDate;
            Receipts = receipts;
            Comments = comments;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertIsGreaterThan(CustomerId, 0, "Cliente inválido.");
            AssertionConcern.AssertIsGreaterOrEqualThan(DueDate, CreateDate, "A data de vencimento deve ser maior ou igual a data de lançamento.");
        }

        #endregion

    }
}
