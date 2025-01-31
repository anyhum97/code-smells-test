using System;
using System.Threading;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace MP.WS.Windsor
{
    public static class ContainerContainer
    {
        private static readonly Lazy<ContainerBootstrapper> Bootstrapper = new Lazy<ContainerBootstrapper>(ContainerBootstrapper.Bootstrap, LazyThreadSafetyMode.ExecutionAndPublication);
        public static IWindsorContainer Get()
        {
            return Bootstrapper.Value.Container;
        }
    }

    /// <summary>
    /// сетаппер контейнера
    /// </summary>
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        readonly IWindsorContainer _container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// контейнер
        /// </summary>
        public IWindsorContainer Container
        {
            get { return _container; }
        }

        /// <summary>
        /// метод инсталляции
        /// </summary>
        /// <returns></returns>
        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer().
                Install(FromAssembly.This());
            return new ContainerBootstrapper(container);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Container.Dispose();
        }
    }
}