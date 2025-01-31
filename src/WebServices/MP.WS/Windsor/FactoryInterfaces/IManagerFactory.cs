using MP.Domain.Object;

namespace MP.WS.Windsor.FactoryInterfaces
{
    public interface IManagerFactory
    {
        T Create<T>(string context) where T : IManager;
    }
}
