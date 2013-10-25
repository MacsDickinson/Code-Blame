using System.Net;
using EventStore.ClientAPI;

namespace CodeBlame.EventStore
{
    public class EventStoreConnectionProvider
    {
        private const int _tcpIpPort = 1113;

        private static IEventStoreConnection _eventStoreConnection;

        public static IEventStoreConnection EventStore
        {
            get { return _eventStoreConnection ?? (_eventStoreConnection = CreateEventStoreConnection()); }
        }

        private static IEventStoreConnection CreateEventStoreConnection()
        {
            var  tcpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _tcpIpPort);
            
            return EventStoreConnection.Create(ConnectionSettings.Default, tcpEndPoint);
        }
    }
}