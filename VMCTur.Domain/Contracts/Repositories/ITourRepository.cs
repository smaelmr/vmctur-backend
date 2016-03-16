using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITourRepository : IDisposable
    {
        List<Tour> Get(string search);
        Tour Get(int id);
        List<Tour> Get(int skip, int take);

        void Create(Tour veiculo);
        void Update(Tour veiculo);
        void Delete(Tour veiculo);
    }
}
