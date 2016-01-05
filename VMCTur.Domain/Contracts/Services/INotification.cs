using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VMCTur.Domain.Contracts.Services
{
    public interface INotification : IDisposable
    {       
        void Send(string to, string body);
    }
}
