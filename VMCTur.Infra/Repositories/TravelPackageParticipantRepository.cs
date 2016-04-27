using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class TravelPackageParticipantRepository : ITravelPackageParticipantRepository
    {
        private AppDataContext _context;

        public TravelPackageParticipantRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(TravelPackageParticipant participant)
        {
            _context.TravelPackageParticipants.Add(participant);
            _context.SaveChanges();
        }

        public void Update(TravelPackageParticipant participant)
        {            
            _context.Entry<TravelPackageParticipant>(participant).State = System.Data.Entity.EntityState.Detached;
            _context.SaveChanges();            
        }

        public void Delete(TravelPackageParticipant participant)
        {
            _context.TravelPackageParticipants.Remove(participant);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
