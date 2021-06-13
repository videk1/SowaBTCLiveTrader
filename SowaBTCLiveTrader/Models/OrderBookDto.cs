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
            return "Timestamp: " + DateTime.Now + " Max. Bitcons to buy: " + maxBtcToBuy + " Asks length: " + asks.Count + " Bids length: " + bids.Count;
        }
    }
}
