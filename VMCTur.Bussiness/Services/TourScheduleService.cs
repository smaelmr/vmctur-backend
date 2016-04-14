using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Bussiness.Services
{
    public class TourScheduleService : ITourScheduleService
    {
        private ITourScheduleRepository _repository;

        public TourScheduleService(ITourScheduleRepository repository)
        {
            _repository = repository;
        }

        public List<TourSchedule> Get(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.Get(startPeriod, finishPeriod);
        }
        
        public List<TourSchedule> GetNextSevenDays()
        {
            return _repository.Get(7);
        }

        public List<TourSchedule> GetNextFifteenDays()
        {
            return _repository.Get(15);
        }

        public List<TourSchedule> GetNextThirtyDays()
        {
            return _repository.Get(30);
        }

        public List<TourSchedule> GetAll()
        {
            return _repository.GetAll();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
