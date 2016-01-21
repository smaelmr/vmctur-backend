using System;

namespace VMCTur.Domain.Contracts.Services
{
    public interface INotification : IDisposable
    {       
        void Send(string to, string body);
    }
}
