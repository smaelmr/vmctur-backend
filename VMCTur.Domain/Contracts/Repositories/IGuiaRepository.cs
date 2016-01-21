using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.Guias;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IGuiaRepository : IDisposable
    {
        List<Guia> Get(string search);
        Guia Get(int id);
        List<Guia> Get(int skip, int take);

        void Create(Guia guia);
        void Update(Guia guia);
        void Delete(Guia guia);
    }
}
