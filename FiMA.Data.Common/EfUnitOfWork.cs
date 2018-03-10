using FiMA.Data.Common.Contracts;
using System;
using System.Data.Entity;

namespace FiMA.Data.Common
{
    public class EfUnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext context;

        public EfUnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}
