using FiMA.Data;
using FiMA.Data.Common;
using FiMA.Data.Common.Contracts;
using Ninject.Modules;
using System.Data.Entity;

namespace FiMA.Forms.FrontOffice
{
    public class FormsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>().To<KpEntities>().InSingletonScope();
            Bind(typeof(IRepository<>)).To(typeof(EfGenericRepository<>));
            Bind<IUnitOfWork>().To<EfUnitOfWork>().InSingletonScope();
            Bind<InvestorCreate>().ToSelf().InSingletonScope();
        }
    }
}
