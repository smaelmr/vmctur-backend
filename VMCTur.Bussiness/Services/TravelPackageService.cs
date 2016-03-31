using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Commands.TravelPackageCommands.Create;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Domain.Commands.TravelPackageCommands.Update;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Create;
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Update;

namespace VMCTur.Bussiness.Services
{
    public class TravelPackageService : ITravelPackageService
    {
        private ITravelPackageRepository _repository;

        public TravelPackageService(ITravelPackageRepository repository)
        {
            _repository = repository;
        }

        public void Create(CreateTravelPackageCommand travelPackageCreate)
        {
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();
            List<BillReceive> bills = new List<BillReceive>();

            foreach (CreateParticipantCommand p in travelPackageCreate.Participants)
            {
                participants.Add(new TravelPackageParticipant(0, p.Name, p.NumberDocument, p.BirthDate, p.TravelPackageId));
            }

            foreach (CreateTourCommand p in travelPackageCreate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);
                tours.Add(new TravelPackageTour(0, p.TourId, p.TravelPackageId, dateHourStart));
            }

            foreach (CreateBillReceiveCommand p in travelPackageCreate.Bills)
            {
                bills.Add(new BillReceive(0, p.TravelPackageId, p.Amount, p.AmountReceived, p.Concerning, p.DueDate, p.PayDay, p.Comments));
            }

            var travelPackage = new TravelPackage(0, travelPackageCreate.CompanyId, travelPackageCreate.CustomerId, participants, tours, bills,
                                           travelPackageCreate.Host, travelPackageCreate.QuantityTickets, travelPackageCreate.VehicleUsedId, 
                                           travelPackageCreate.GuideTourId, travelPackageCreate.TotalAmount, 
                                           travelPackageCreate.AddictionalReservs, travelPackageCreate.Comments);

            travelPackage.Validate();

            _repository.Create(travelPackage);
        }

        public void Update(UpdateTravelPackageCommand travelPackageUpdate)
        {
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();
            List<BillReceive> bills = new List<BillReceive>();

            foreach (UpdateParticipantCommand p in travelPackageUpdate.Participants)
                participants.Add(new TravelPackageParticipant(0, p.Name, p.NumberDocument, p.BirthDate, p.TravelPackageId));

            foreach (UpdateTourCommand p in travelPackageUpdate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);

                tours.Add(new TravelPackageTour(p.Id, p.TourId, p.TravelPackageId, dateHourStart));
            }

            foreach (UpdateBillReceiveCommand p in travelPackageUpdate.Bills)
            {
                bills.Add(new BillReceive(p.Id, p.TravelPackageId, p.Amount, p.AmountReceived, p.Concerning, p.DueDate, p.PayDay, p.Comments));
            }

            var travelPackage = new TravelPackage(travelPackageUpdate.Id, travelPackageUpdate.CompanyId, travelPackageUpdate.CustomerId, participants, tours, bills,
                                           travelPackageUpdate.HostLocal, travelPackageUpdate.QuantityTickets, travelPackageUpdate.VehicleUsedId, 
                                           travelPackageUpdate.GuideTourId, travelPackageUpdate.PaymentAmount, 
                                           travelPackageUpdate.AddictionalReservs, travelPackageUpdate.Comments);

            travelPackage.Validate();

            _repository.Create(travelPackage);
        }

        public void Delete(int id)
        {
            var travelPackage = _repository.Get(id);

            _repository.Delete(travelPackage);
        }                

        public TravelPackage GetById(int id)
        {
            var travelPackage = _repository.Get(id);

            return travelPackage;
        }

        public List<TravelPackage> GetByRange(int skip, int take)
        {
            var travelPackage = _repository.Get(skip, take);

            return travelPackage;
        }

        public List<TravelPackage> GetBySearch(string search)
        {
            var travelPackage = _repository.Get(search);

            return travelPackage;
        }        

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
