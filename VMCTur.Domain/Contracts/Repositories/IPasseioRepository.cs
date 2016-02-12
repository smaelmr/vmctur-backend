using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Passeios;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IPasseioRepository : IDisposable
    {
        List<Passeio> Get(string search);
        Passeio Get(int id);
        List<Passeio> Get(int skip, int take);

        void Create(Passeio veiculo);
        void Update(Passeio veiculo);
        void Delete(Passeio veiculo);
    }
}
