using Castle.Windsor;
using Common.Logging;
using MP.Domain.Object;
using MP.WS.Windsor;
using System;
using System.Web.Services.Protocols;
using MP.WS.Windsor.FactoryInterfaces;

namespace MP.WS.Old_App_Code
{
    /// <summary>
    /// Базовый класс для общего сервиса
    /// </summary>
    public class BaseWebService
    {
        private ILog _log;
        /// <summary>
        /// Лог
        /// </summary>
        protected ILog Log
        {
            get { return _log ?? (_log = LogManager.GetLogger(GetType())); }
        }

        protected IWindsorContainer GetContainer()
        {
            return ContainerContainer.Get();
        }

        protected IManagerFactory GetTicketManagerFactory()
        {
            return ContainerContainer.Get().Resolve<IManagerFactory>();
        }

        protected SoapException WrapException(Exception ex)
        {
            var soapException = new SoapException(ex.Message, SoapException.ServerFaultCode);
            return soapException;
        }
    }
}
