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
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();

            foreach (CreateParticipantCommand p in travelPackageCreate.Participants)
                participants.Add(new TravelPackageParticipant(0, p.Name, p.NumberDocument, p.BirthDate, 0));

            

            foreach (CreateTourCommand p in travelPackageCreate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);

                tours.Add(new TravelPackageTour(0, p.TourId, 0, dateHourStart));

            }

            var travelPackage = new TravelPackage(0, travelPackageCreate.CompanyId, travelPackageCreate.CustomerId, participants, tours,
                                           travelPackageCreate.Host, travelPackageCreate.QuantityTickets, travelPackageCreate.VehicleUsedId, 
                                           travelPackageCreate.GuideTourId, travelPackageCreate.PaymentAmount, travelPackageCreate.PayDayFirst, 
                                           travelPackageCreate.PaymentFirst, travelPackageCreate.PaymentTermsRemaining,
                                           travelPackageCreate.AddictionalReservs, travelPackageCreate.Comments);

            travelPackage.Validate();

            _repository.Create(travelPackage);
        }

        public void Update(UpdateTravelPackageCommand travelPackageUpdate)
        {
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();

            foreach (UpdateParticipantCommand p in travelPackageUpdate.Participants)
                participants.Add(new TravelPackageParticipant(0, p.Name, p.NumberDocument, p.BirthDate, 0));

            foreach (UpdateTourCommand p in travelPackageUpdate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);

                tours.Add(new TravelPackageTour(p.Id, p.TourId, p.TravelPackageId, dateHourStart));
            }

            var travelPackage = new TravelPackage(travelPackageUpdate.Id, travelPackageUpdate.CompanyId, travelPackageUpdate.CustomerId, participants, tours,
                                           travelPackageUpdate.HostLocal, travelPackageUpdate.QuantityTickets, travelPackageUpdate.VehicleUsedId, 
                                           travelPackageUpdate.GuideTourId, travelPackageUpdate.PaymentAmount, travelPackageUpdate.PayDayFirst, 
                                           travelPackageUpdate.PaymentFirst, travelPackageUpdate.PaymentTermsRemaining,
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
