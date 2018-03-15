using System;

namespace FiMA.Data.Common.Contracts
{
    // IDisposable gives us syntactic sugar with the using keyword
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
