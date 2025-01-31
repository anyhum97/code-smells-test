using MP.Domain.Object;

namespace MP.WS.Windsor.FactoryInterfaces
{
    public interface IManagerFactory
    {
        TicketManager Create(string context);
    }
}
