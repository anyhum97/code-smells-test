using Common.Logging;
using System;
using System.Web;
using MP.WS.Windsor;

namespace MP.WS
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = ContainerContainer.Get();

            ILog log = LogManager.GetLogger(GetType());
            log.Info("Web application starting...");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
			
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
			Exception ex = Server.GetLastError();
			ILog log = LogManager.GetLogger(GetType());			
            //Если это HttpException то отвечаем 500
            if (ex is HttpException)
            {
                log.Error(ex);
                log.Fatal("Unhandled exception. Will be response 500", ex);
                Response.Write("Unknown error");
                Response.StatusCode = 500;
                Server.ClearError();
            }
            else if (ex is System.Web.Services.Protocols.SoapException)
            {
                log.Warn("SoapException. Will be response 500", ex);
                Response.Write("Format error");
                Response.StatusCode = 500;
                Server.ClearError();
            }
            else
            {
                log.Error(ex);
            }
        }           

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
