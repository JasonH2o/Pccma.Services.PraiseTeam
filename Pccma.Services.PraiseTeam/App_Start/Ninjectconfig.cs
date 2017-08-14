using Ninject;
using Ninject.Syntax;
using Pccma.PraiseTeam.Database.Contracts;
using Pccma.PraiseTeam.Database.DataAccess;

namespace Pccma.Services.PraiseTeam.App_Start
{
    /// <summary>
    ///     Configuration for Ninject Container
    /// </summary>
    public class Ninjectconfig
    {
        public static IKernel RegisterComponents()
        {
            var container = new StandardKernel();

            BuildDataContexts(container);

            return container;
        }

        private static void BuildDataContexts(IBindingRoot container)
        {
            container.Bind<IPccmaPraiseTeamDataStoreContext>().To<PccmaPraiseTeamDataStoreContext>();
        }
    }
}