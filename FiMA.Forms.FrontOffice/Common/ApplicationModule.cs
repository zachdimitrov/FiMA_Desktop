using FiMA.Data.Common;
using FiMA.Data.Common.Contracts;
using Ninject.Modules;

namespace FiMA.Forms.FrontOffice.Common
{
    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>)).To(typeof(EfGenericRepository<>));
            Bind(typeof(IUnitOfWork)).To(typeof(EfUnitOfWork));
        }
    }
}
