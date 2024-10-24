using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace IB_TradingPlatformExtention1
{
    public class AccountStateManager
    {
        public List<Position> OpenPositions { get; private set; } = new List<Position>();
        public List<OpenOrder> OpenOrders { get; private set; } = new List<OpenOrder>();

        public void UpdatePosition(string account, Contract contract, decimal position, double avgCost)
        {
            var existingPosition = OpenPositions.Find(p => p.Contract.Symbol == contract.Symbol);
            if (existingPosition != null)
            {
                existingPosition.PositionAmount = position;
                existingPosition.AverageCost = avgCost;
            }
            else
            {
                OpenPositions.Add(new Position
                {
                    Account = account,
                    Contract = contract,
                    PositionAmount = position,
                    AverageCost = avgCost
                });
            }
        }

        public void UpdateOrder(Order order, Contract contract)
        {
            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Order = order;
                existingOrder.Contract = contract;
            }
            else
            {
                OpenOrders.Add(new OpenOrder
                {
                    Order = order,
                    Contract = contract
                });
            }
        }

        public void UpdateOrder(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == orderId);
            if (existingOrder != null)
            {
                existingOrder.Status = status;
                existingOrder.Filled = filled;
                existingOrder.Remaining = remaining;
                existingOrder.AvgFillPrice = avgFillPrice;
                existingOrder.PermId = permId;
                existingOrder.ParentId = parentId;
                existingOrder.LastFillPrice = lastFillPrice;
                existingOrder.ClientId = clientId;
                existingOrder.WhyHeld = whyHeld;
                existingOrder.MktCapPrice = mktCapPrice;
            }
        }

        public void RemoveOrder(int orderId)
        {
            OpenOrders.RemoveAll(o => o.Order.OrderId == orderId);
        }
    }

    public class Position
    {
        public string Account { get; set; }
        public Contract Contract { get; set; }
        public decimal PositionAmount { get; set; }
        public double AverageCost { get; set; }
    }

    public class OpenOrder
    {
        public Order Order { get; set; }
        public Contract Contract { get; set; }
        public string Status { get; set; }
        public decimal Filled { get; set; }
        public decimal Remaining { get; set; }
        public double AvgFillPrice { get; set; }
        public long PermId { get; set; }
        public int ParentId { get; set; }
        public double LastFillPrice { get; set; }
        public int ClientId { get; set; }
        public string WhyHeld { get; set; }
        public double MktCapPrice { get; set; }
    }
}
