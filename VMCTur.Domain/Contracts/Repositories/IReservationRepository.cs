using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Reservations;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IReservationRepository : IDisposable
    {
        List<Reservation> Get(string search);
        Reservation Get(int id);
        List<Reservation> Get(int skip, int take);

        void Create(Reservation reserve);
        void Update(Reservation reserve, Reservation reserveOld);
        void Delete(Reservation reserve);
    }
}
