using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MP.Domain.Object;
using MP.WS.Windsor.Adapter;
using MP.WS.Windsor.FactoryInterfaces;

namespace MP.WS.Windsor
{
    public class Installer : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Добавление генератора фабрик
            container.AddFacility<TypedFactoryFacility>();
            //Регистрация компонент
            container.Register(
                // адаптеры
                Classes
                    .FromAssemblyContaining(typeof(IAdapter<,>))
                    .BasedOn(typeof(IAdapter<,>))
                    .OrBasedOn(typeof(IAdapter<,,>))
                    .WithServiceAllInterfaces(),

                Component.For<TicketManager>().LifestyleSingleton(),
                Component.For<IManagerFactory>().AsFactory().LifestyleTransient()
            );
        }

        #endregion
    }
}
