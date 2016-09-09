using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.ReservationCommands;
using VMCTur.Domain.Entities.Reservations;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IReservationService : IDisposable
    {
        void Create(CreateReservationCommand reserve);
        void Update(UpdateReservationCommand reserve);
        void Delete(int id);

        List<Reservation> Get(string search);
        Reservation Get(int id);
        List<Reservation> Get(int skip, int take);
        
    }
}
