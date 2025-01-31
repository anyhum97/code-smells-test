using MP.Domain.Model;
using MP.Domain.Object;
using MP.Model.WS;
using MP.WS.Old_App_Code;
using MP.WS.Windsor.Adapter;
using MP.WS.Windsor.FactoryInterfaces;
using System;
using System.Linq;
using System.Web.Services;

namespace MP.WS
{
    /// <summary>
    /// Summary description
    /// </summary>
    [WebService(Namespace = "http://test.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ticket_api : BaseWebService
    {
        /// <summary>
        /// Получить список заявок
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Gets tickets")]
        public TicketViewModel[] GetTickets(DateTime from, DateTime to)
        {
            try
            {
                var ticketManager = GetContainer().Resolve<IManagerFactory>().Create<TicketManager>("MP");
                var tickets = ticketManager.GetTickets();

                var windsorContainer = GetContainer();

                var adapter = windsorContainer.Resolve<IAdapter<Ticket, TicketViewModel>>();

                return tickets.Select(adapter.Convert).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error("Exception occured in GetTickets.", ex);
                throw WrapException(ex);
            }
        }

        

        /// <summary>
        /// Сохранить заявку
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Save ticket")]
        public long SaveTicket(TicketSaveModel arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(arg.Title))
                {
                    Log.ErrorFormat("Empty title");
                    throw new Exception("Empty title");
                }

                if (arg.VisitDate < DateTime.UtcNow)
                {
                    Log.ErrorFormat("Ticket creation in the past is disabled");
                    throw new Exception("Ticket creation in the past is disabled");
                }

                if (arg.VisitorsNumber < 1 || arg.VisitorsNumber > 10)
                {
                    Log.ErrorFormat("VisitorsNumber value is not allowed. Value must be in interval [1..10]");
                    throw new Exception("VisitorsNumber value is not allowed. Value must be in interval [1..10]");
                }

                var ticket = new Ticket()
                {
                    Title = arg.Title,
                    Description = arg.Description,
                    VisitDate = arg.VisitDate,
                    VisitorsNumber = arg.VisitorsNumber
                };

                var ticketManager = GetContainer().Resolve<IManagerFactory>().Create<TicketManager>("MP");

                ticketManager.AddTicket(ticket);

                return ticket.ID ?? -1;
            }
            catch (Exception ex)
            {
                Log.Error("Exception occured in SaveTicket.", ex);
                throw WrapException(ex);
            }
        }
    }
}