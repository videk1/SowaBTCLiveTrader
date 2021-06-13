using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SowaBTCLiveTrader.Models
{
    public class OrderBookDto
    {
        public decimal maxBtcToBuy { get; set; }
        public List<decimal[]> asks { get; set; }
        public List<decimal[]> bids { get; set; }

        public override string ToString()
        {
            var asksToString = asks.Select(x => "Price: " + x[0] + " | Amount: " + x[1]);
            var bidsToString = bids.Select(x => "Price: " + x[0] + " | Amount: " + x[1]);
            return "Max. Bitcons to buy: " + maxBtcToBuy + " Asks: " + string.Join(", ", asksToString) + " Bids: " + string.Join(", ", bidsToString);
        }
    }
}
