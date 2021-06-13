using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.Models
{
    public struct BitstampOrderBook
    {
        public BitstampOrderBookData data { get; set; }
        public string channel { get; set; }
    }

    public class BitstampOrderBookData
    {
        public List<string[]> bids { get; set; }
        public List<string[]> asks { get; set; }
    }
}
