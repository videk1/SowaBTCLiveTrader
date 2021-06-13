using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.Models
{
    public class BitstampConnectionClass
    {
        public string @event { get; } = "bts:subscribe";
        public BitstampConnectionDataClass data { get; } = new BitstampConnectionDataClass();
    }

    public class BitstampConnectionDataClass
    {
        public string channel { get; } = "order_book_btceur";
    }
}
