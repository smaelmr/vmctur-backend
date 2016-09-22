using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITourScheduleRepository : IDisposable
    {
        List<TourSchedule> Get(double days);
        List<TourSchedule> GetAll();
        List<TourSchedule> Get(DateTime startPeriod, DateTime finishPeriod);
        string ExportExcel(DateTime startPeriod, DateTime finishPeriod);
        List<TourSchedule> GetToursByContractNumber(string contractNumber);
    }
}
