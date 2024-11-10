using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB_TradingPlatformExtention1.Interfaces
{
    internal interface IBrokerApiClient
    {
        List<Position> OpenPositions { get; }
        List<OpenOrder> OpenOrders { get; }

        // Events to notify when data changes
        event Action<string, string> OnTickPriceUpdated;
        event Action OnConnected;
        event Action OnDisconnected;

        // Connection management
        void Connect(string host, int port, int clientId);
        void Disconnect();

        // Market data retrieval
        void GetDataForCurrContract();

        // Order management
        //void PlaceOrder(myContract c, myOrder o);

        // Order and position updates
        void UpdatePosition(string account, Contract contract, decimal position, double avgCost);
        void UpdateOrder(Order order, Contract contract);
        void UpdateOrder(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice);
        void RemoveOrder(int orderId);
    }
}
