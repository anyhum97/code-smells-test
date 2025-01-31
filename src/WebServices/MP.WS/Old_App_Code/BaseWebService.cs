using Castle.Windsor;
using Common.Logging;
using MP.WS.Windsor;
using System;
using System.Web.Services.Protocols;

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

        protected SoapException WrapException(Exception ex)
        {
            var soapException = new SoapException(ex.Message, SoapException.ServerFaultCode);
            return soapException;
        }
    }
}
