using SowaBTCLiveTrader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.Services.OrderBookService
{
    public class OrderBookService : IOrderBookService
    {
        public OrderBookDto CalculateAndReturnOrderBookData(BitstampOrderBook bitstampOrderBook)
        {
            decimal maxBtcToBuy = decimal.Zero;

            var asks = new List<decimal[]>();
            var bids = new List<decimal[]>();

            IFormatProvider culture = CultureInfo.InvariantCulture;

            if (bitstampOrderBook.data != null)
            {
                if (bitstampOrderBook.data.asks != null)
                {
                    // We need to group by price so it is unique
                    // Then we need to sort them so we can calculate cumulative values
                    var orderedAsks = bitstampOrderBook.data.asks
                        .GroupBy(x => x[0])
                        .Select(x => new decimal[]{
                            decimal.Parse(x.Key, culture),
                            x.Sum(y => decimal.Parse(y[1], culture))})
                        .OrderBy(x => x[0]);

                    decimal tmpAmount = 0;
                    // Once we have them sorted we can easily calculate cumulative values
                    foreach (var orderedAsk in orderedAsks)
                    {
                        tmpAmount += orderedAsk[1];
                        asks.Add(new decimal[] {
                            orderedAsk[0],
                            tmpAmount
                        });
                    }

                    maxBtcToBuy = tmpAmount;
                }

                if (bitstampOrderBook.data.bids != null)
                {
                    // We need to group by price so it is unique
                    // Then we need to sort them so we can calculate cumulative values
                    var orderedBids = bitstampOrderBook.data.bids
                        .GroupBy(x => x[0])
                        .Select(x => new decimal[]{
                            decimal.Parse(x.Key, culture),
                            x.Sum(y => decimal.Parse(y[1], culture))})
                        .OrderByDescending(x => x[0]);

                    decimal tmpAmount = 0;
                    // Once we have them sorted we can easily calculate cumulative values
                    foreach (var orderedAsk in orderedBids)
                    {
                        tmpAmount += orderedAsk[1];
                        bids.Add(new decimal[] {
                            orderedAsk[0],
                            tmpAmount
                        });
                    }
                }

            }

            return new OrderBookDto()
            {
                maxBtcToBuy = maxBtcToBuy,
                asks = asks,
                bids = bids
            };
        }
    }
}
