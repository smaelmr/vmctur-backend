using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Pacotes;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IPacoteRepository : IDisposable
    {
        List<Pacote> Get(string search);
        Pacote Get(int id);
        List<Pacote> Get(int skip, int take);

        void Create(Pacote pacote);
        void Update(Pacote pacote);
        void Delete(Pacote pacote);
    }
}
