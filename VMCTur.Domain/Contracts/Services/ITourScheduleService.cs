using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Services
{
    public interface ITourScheduleService : IDisposable
    {
        List<TourSchedule> GetNextDays();
        List<TourSchedule> GetNextSevenDays();
        List<TourSchedule> GetNextFifteenDays();
        List<TourSchedule> GetNextThirtyDays();
        List<TourSchedule> GetAll();
        List<TourSchedule> Get(DateTime startPeriod, DateTime finishPeriod);
        string ExportExcel(DateTime startPeriod, DateTime finishPeriod);
    }
}
