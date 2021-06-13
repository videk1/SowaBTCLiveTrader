using SowaBTCLiveTrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.Services.OrderBookService
{
    public interface IOrderBookService
    {
        OrderBookDto CalculateAndReturnOrderBookData(BitstampOrderBook bitstampOrderBook);
    }
}
