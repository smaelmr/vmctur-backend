using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class TourScheduleRepository
    {
        private AppDataContext _context;

        public TourScheduleRepository(AppDataContext context)
        {
            this._context = context;
        }

        public List<TourSchedule> Get(DateTime startPeriod, DateTime finishPeriod)
        {
            return null;
        }

        public List<TravelPackage> GetToday()
        {
            return null;
        }

        public List<TravelPackage> GetNextWeek()
        {
            return null;
        }

        public List<TravelPackage> GetNextMonth()
        {
            return null;
        }
    }
}
