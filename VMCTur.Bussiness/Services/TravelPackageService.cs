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
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;
using System.IO;
using VMCTur.Domain.Enums;

namespace VMCTur.Bussiness.Services
{
    public class TravelPackageService : ITravelPackageService
    {
        private ITravelPackageRepository _repositoryPack;
        private ITravelPackageTourRepository _repositoryTour;
        private ITravelPackageParticipantRepository _repositoryParticipant;
        private IBillReceiveRepository _repositoryBill;

        public TravelPackageService(ITravelPackageRepository repositoryPack, ITravelPackageTourRepository repositoryTour, ITravelPackageParticipantRepository repositoryParticipant, IBillReceiveRepository repositoryBill)
        {
            _repositoryPack = repositoryPack;
            _repositoryTour = repositoryTour;
            _repositoryParticipant = repositoryParticipant;
            _repositoryBill = repositoryBill;
    }

        public void Create(CreateTravelPackageCommand travelPackageCreate)
        {
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();
            List<BillReceive> bills = new List<BillReceive>();

            foreach (CreateParticipantCommand p in travelPackageCreate.Participants)
            {
                AgeGroup ageGoupBelong = (AgeGroup)Enum.Parse(typeof(AgeGroup), p.AgeGoupBelong);

                participants.Add(new TravelPackageParticipant(0, p.Name, p.NumberDocument, p.BirthDate, ageGoupBelong, p.Paying, p.TravelPackageId));
            }

            foreach (CreateTourCommand p in travelPackageCreate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);                

                tours.Add(new TravelPackageTour(0, p.TourId, p.TravelPackageId, dateHourStart, p.TourComments, p.Shared));
            }

            foreach (CreateBillReceiveCommand p in travelPackageCreate.Bills)
            {
                bills.Add(new BillReceive(0, DateTime.Now, p.TravelPackageId, p.Amount, 0, p.Concerning, p.DueDate, null, p.Comments));
            }

            var travelPackage = new TravelPackage(0, travelPackageCreate.CompanyId, travelPackageCreate.CustomerId, participants, tours, bills,
                                           travelPackageCreate.Host, travelPackageCreate.QuantityTickets, travelPackageCreate.VehicleUsedId, 
                                           travelPackageCreate.GuideTourId, travelPackageCreate.TotalAmount, 
                                           travelPackageCreate.AddictionalReservs, travelPackageCreate.Comments);

            travelPackage.Validate();

            _repositoryPack.Create(travelPackage);
        }

        public void Update(UpdateTravelPackageCommand travelPackageUpdate)
        {
            List<TravelPackageParticipant> participants = new List<TravelPackageParticipant>();
            List<TravelPackageTour> tours = new List<TravelPackageTour>();
            List<BillReceive> bills = new List<BillReceive>();

            ///Smael: busca o registro original.
            TravelPackage packageOld = GetById(travelPackageUpdate.Id);

            #region Participants

            foreach (UpdateParticipantCommand p in travelPackageUpdate.Participants)
            {
                AgeGroup ageGoupBelong = (AgeGroup)Enum.Parse(typeof(AgeGroup), p.AgeGoupBelong);

                participants.Add(new TravelPackageParticipant(p.Id, p.Name, p.NumberDocument, p.BirthDate, ageGoupBelong, p.Paying, travelPackageUpdate.Id));
            }

            #endregion

            #region Tours

            foreach (UpdateTourCommand p in travelPackageUpdate.Tours)
            {
                DateTime dateHourStart = new DateTime(p.DateStart.Year, p.DateStart.Month, p.DateStart.Day, p.HourStart.Hours, p.HourStart.Minutes, 0);

                tours.Add(new TravelPackageTour(p.Id, p.TourId, travelPackageUpdate.Id, dateHourStart, p.TourComments, p.Shared));
            }

            #endregion

            #region Bills

            foreach (UpdateBillReceiveCommand p in travelPackageUpdate.Bills)
            {
                bills.Add(new BillReceive(p.Id, DateTime.Now, travelPackageUpdate.Id, p.Amount, p.AmountReceived, p.Concerning, p.DueDate, p.PayDay, p.Comments));
            }

            #endregion

            var travelPackage = new TravelPackage(travelPackageUpdate.Id, travelPackageUpdate.CompanyId, travelPackageUpdate.CustomerId, participants, tours, bills,
                                           travelPackageUpdate.Host, travelPackageUpdate.QuantityTickets, travelPackageUpdate.VehicleUsedId, 
                                           travelPackageUpdate.GuideTourId, travelPackageUpdate.TotalAmount, 
                                           travelPackageUpdate.AddictionalReservs, travelPackageUpdate.Comments);

            travelPackage.Validate();

            _repositoryPack.Update(travelPackage, packageOld);
        }

        public void Delete(int id)
        {
            var travelPackage = _repositoryPack.Get(id);

            _repositoryPack.Delete(travelPackage);
        }                

        public TravelPackage GetById(int id)
        {
            var travelPackage = _repositoryPack.Get(id);

            return travelPackage;
        }

        public List<TravelPackage> GetByRange(int skip, int take)
        {
            var travelPackage = _repositoryPack.Get(skip, take);

            return travelPackage;
        }

        public List<TravelPackage> GetBySearch(string search)
        {
            var travelPackage = _repositoryPack.Get(search);

            return travelPackage;
        }

        public MemoryStream PrintPreBooking(int id, string urlRoot)
        {
            var package = GetById(id);

            return _repositoryPack.PrintPreBooking(package, urlRoot);
        }

        public MemoryStream PrintBookingConfirmation(int id, string urlRoot)
        {
            var package = GetById(id);

            return _repositoryPack.PrintBookingConfirmation(package, urlRoot);
        }

        public MemoryStream PrintVoucher(int id, string urlRoot)
        {
            var package = GetById(id);

            return _repositoryPack.PrintVoucher(package, urlRoot);
        }

        public void Dispose()
        {
            _repositoryPack.Dispose();
            _repositoryParticipant.Dispose();
            _repositoryTour.Dispose();
            _repositoryBill.Dispose();
        }
    }
}
