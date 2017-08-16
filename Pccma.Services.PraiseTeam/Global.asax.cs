using Ninject.Web.Common;
using System.Web.Http;
using Ninject;
using Pccma.Services.PraiseTeam.App_Start;
using Ninject.Web.WebApi;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.NinjectAdapter.Unofficial;

namespace Pccma.Services.PraiseTeam
{
    public class WebApiApplication : NinjectHttpApplication
    {
        /// <summary>
        ///     Called when the application is started
        /// </summary>
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.RegisterMappingConfig();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application
        /// </summary>
        /// <returns></returns>
        protected override IKernel CreateKernel()
        {
            var kernel = Ninjectconfig.RegisterComponents();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            return kernel;
        }
    }
}
