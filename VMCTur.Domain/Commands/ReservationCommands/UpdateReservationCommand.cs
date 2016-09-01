﻿using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillPayCommands;

namespace VMCTur.Domain.Commands.ReservationCommands
{
    public class UpdateReservationCommand
    {
        public int id { get; set; }
        public DateTime DateReservation { get; set; }
        public int QuantityTickets { get; set; }
        public string DeparturePlace { get; set; }
        public string Notification { get; set; }
        public string ContractNumber { get; set; }
        public string Status { get; set; }
        public List<UpdateBillPayCommand> BillsPay { get; set; }
    }
}