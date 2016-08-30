using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Entities.Reservations
{
    public class Reservation
    {
        #region Properties

        public int Id { get; private set; }
        public DateTime DateReservation { get; private set; }
        public int QuantityTickets { get; private set; }
        public string DeparturePlace { get; private set; }  //local de partida (Bento ou Carlos Barbosa)

        public string Notification { get; private set; } //NOTIFICAÇÃO = campo string aberto para texto 

        public string ContractNumber { get; private set; }
        public string Status { get; private set; } //(Cancelado, efetivado)

        //DATA SINAL = data que deve ser pago o sinal 

        //Lista de contas a receber.

        #endregion
    }
}
