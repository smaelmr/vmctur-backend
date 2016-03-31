using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.VehicleCommands;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Enums;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Bussiness.Services
{
    public class VehicleService : IVehicleService
    {
        private IVehicleRepository _repository;

        public VehicleService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public void Create(CreateVehicleCommand vehicleCommand)
        {
            TypeAcquisition typeAcquisition = (TypeAcquisition)Enum.Parse(typeof(TypeAcquisition), vehicleCommand.AcquisitionType);

            var vehicle = new Vehicle(0, vehicleCommand.CompanyId, vehicleCommand.Plate, vehicleCommand.Year, vehicleCommand.Model, 
                vehicleCommand.NumberOfPassengers, vehicleCommand.Inactive, typeAcquisition, vehicleCommand.Comments);

            vehicle.Validate();

            _repository.Create(vehicle);
        }

        public void Update(UpdateVehicleCommand vehicleCommand)
        {
            TypeAcquisition typeAcquisition = (TypeAcquisition)Enum.Parse(typeof(TypeAcquisition), vehicleCommand.AcquisitionType);

            var veiculo = new Vehicle(vehicleCommand.Id, vehicleCommand.CompanyId, vehicleCommand.Plate, vehicleCommand.Year, vehicleCommand.Model,
                vehicleCommand.NumberOfPassengers, vehicleCommand.Inactive, typeAcquisition, vehicleCommand.Comments);

            veiculo.Validate();

            _repository.Update(veiculo);
        }

        public void Delete(int id)
        {
            var vehicle = _repository.Get(id);

            _repository.Delete(vehicle);
        }

        public Vehicle GetById(int id)
        {
            var vehicle = _repository.Get(id);

            return vehicle;
        }

        public List<Vehicle> GetByRange(int skip, int take)
        {
            var vehicle = _repository.Get(skip, take);

            return vehicle;
        }

        public List<Vehicle> GetBySearch(string search)
        {
            var vehicle = _repository.Get(search);

            return vehicle;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }     
    }
}
