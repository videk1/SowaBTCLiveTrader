using Microsoft.VisualStudio.TestTools.UnitTesting;
using SowaBTCLiveTrader.Models;
using SowaBTCLiveTrader.Services.OrderBookService;
using System.Collections.Generic;

namespace SowaBTCLiveTraderUnitTests
{
    [TestClass]
    public class OrderBookServiceTest
    {
        private readonly IOrderBookService _orderBookService = new OrderBookService();

        [TestMethod]
        public void ShouldReturnEmptySets()
        {
            var testData = new BitstampOrderBook()
            {
                data = new BitstampOrderBookData()
                {
                    asks = new List<string[]>(),
                    bids = new List<string[]>()
                }
            };
            var response = _orderBookService.CalculateAndReturnOrderBookData(testData);
            Assert.AreEqual(testData.data.bids.Count, response.bids.Count);
            Assert.AreEqual(testData.data.asks.Count, response.asks.Count);
        }

        [TestMethod]
        public void ShouldCalculateTheRightMaxToBuyValue()
        {
            var asks = new List<string[]>();
            var bids = new List<string[]>();

            decimal totalValue = 0;

            for(decimal i = 1; i < 11; i++)
            {
                totalValue += i+1;
                asks.Add(new string[] { i.ToString(), (i + 1).ToString() });
                bids.Add(new string[] { i.ToString(), (i + 2).ToString() });
            }
            
            var testData = new BitstampOrderBook()
            {
                data = new BitstampOrderBookData()
                {
                    asks = asks,
                    bids = bids
                }
            };

            var response = _orderBookService.CalculateAndReturnOrderBookData(testData);
            Assert.AreEqual(totalValue, response.maxBtcToBuy);
        }

        [TestMethod]
        public void ShouldGroupDataWithSamePrice()
        {
            var asks = new List<string[]>();
            var bids = new List<string[]>();

            decimal askPrice = 10;
            decimal askAmount1 = 5;

            decimal askAmount2 = 15;

            asks.Add(new string[] { askPrice.ToString(), askAmount1.ToString() });
            asks.Add(new string[] { askPrice.ToString(), askAmount2.ToString() });

            var testData = new BitstampOrderBook()
            {
                data = new BitstampOrderBookData()
                {
                    asks = asks,
                    bids = bids
                }
            };

            var response = _orderBookService.CalculateAndReturnOrderBookData(testData);
            Assert.AreEqual(1, response.asks.Count);
            Assert.AreEqual(askAmount1 + askAmount2, response.asks[0][1]);
        }
    }
}
