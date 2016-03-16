using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TourGuides;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IGuideRepository : IDisposable
    {
        List<TourGuide> Get(string search);
        TourGuide Get(int id);
        List<TourGuide> Get(int skip, int take);

        void Create(TourGuide guia);
        void Update(TourGuide guia);
        void Delete(TourGuide guia);
    }
}
