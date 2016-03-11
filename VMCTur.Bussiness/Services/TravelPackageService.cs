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
            List<ParticipantTravelPackage> participants = new List<ParticipantTravelPackage>();
            List<TourTravelPackage> tours = new List<TourTravelPackage>();

            foreach (CreateParticipantCommand p in travelPackageCreate.Participants)
                participants.Add(new ParticipantTravelPackage(0, p.Name, p.NumberDocument, p.BirthDate, 0));

            foreach (CreateTourCommand p in travelPackageCreate.Tours)
                tours.Add(new TourTravelPackage(0, p.Name, p.Comments, 0));


            var travelPackage = new TravelPackage(0, travelPackageCreate.CompanyId, travelPackageCreate.CustomerId,  participants, tours,
                                           travelPackageCreate.DateHourStart, travelPackageCreate.Host, travelPackageCreate.QuantityTickets,
                                           travelPackageCreate.VehicleUsedId, travelPackageCreate.GuideTourId, travelPackageCreate.PaymentAmount,
                                           travelPackageCreate.PayDayFirst, travelPackageCreate.PaymentFirst, travelPackageCreate.PaymentTermsRemaining,
                                           travelPackageCreate.AddictionalReservs, travelPackageCreate.Comments);

            travelPackage.Validate();

            _repository.Create(travelPackage);
        }

        public void Update(UpdateTravelPackageCommand travelPackageUpdate)
        {
            List<ParticipantTravelPackage> participants = new List<ParticipantTravelPackage>();
            List<TourTravelPackage> tours = new List<TourTravelPackage>();

            foreach (UpdateParticipantCommand p in travelPackageUpdate.Participants)
                participants.Add(new ParticipantTravelPackage(0, p.Name, p.NumberDocument, p.BirthDate, 0));

            foreach (UpdateTourCommand p in travelPackageUpdate.Tours)
                tours.Add(new TourTravelPackage(0, p.Name, p.Comments, 0));

            var travelPackage = new TravelPackage(travelPackageUpdate.Id, travelPackageUpdate.CompanyId, travelPackageUpdate.CustomerId, participants, tours,
                                           travelPackageUpdate.DateHourStart, travelPackageUpdate.HostLocal, travelPackageUpdate.QuantityTickets,
                                           travelPackageUpdate.VehicleUsedId, travelPackageUpdate.GuideTourId, travelPackageUpdate.PaymentAmount,
                                           travelPackageUpdate.PayDayFirst, travelPackageUpdate.PaymentFirst, travelPackageUpdate.PaymentTermsRemaining,
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
