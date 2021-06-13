import { OrderBook } from "./../models/OrderBookData";
import { BtcDataService } from "./../btc-data.service";
import { Component, OnInit } from "@angular/core";
import * as Highcharts from "highcharts";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent implements OnInit {
  Highcharts: typeof Highcharts = Highcharts;
  chartOptions;
  constructor(private _btcDataService: BtcDataService) {}

  errorTextForLargerThenMaxAmountToBuy =
    "Amount you entered is larger than total avalible coins.";

  asks: number[][];

  buyAmount: number = 0;

  maxAmountToBuy: number = 0;

  totalPriceForBuyAmount: number = 0;
  errorText: string = "";

  ngOnInit() {
    this._btcDataService.data.subscribe((newData: OrderBook) => {
      this.asks = newData.asks;
      this.maxAmountToBuy = newData.maxBtcToBuy;
      this.calculateTotalPriceForBuyAmount();
      this.chartOptions = this.getChartOptionsForData(newData);
    });
  }
  calculateTotalPriceForBuyAmount() {
    if (this.buyAmount <= 0) {
      this.totalPriceForBuyAmount = 0;
      return;
    }
    if (this.buyAmount > this.maxAmountToBuy) {
      this.totalPriceForBuyAmount = 0;
      this.errorText = this.errorTextForLargerThenMaxAmountToBuy;
      return;
    } else {
      this.errorText = "";
      this.totalPriceForBuyAmount = 0;
    }

    for (let index = 0; index < this.asks.length; index++) {
      const element = this.asks[index];
      if (element[1] >= this.buyAmount) {
        if (element[1] === this.buyAmount) {
          // In this case we entered amount that is the same as the searched element, so we can just calculate price with this element.
          this.totalPriceForBuyAmount = element[0] * this.buyAmount;
        } else {
          if (index === 0) {
            // In this case buyAmount is smaller than first value in asks array so we can just use first price in array.
            this.totalPriceForBuyAmount = element[0] * this.buyAmount;
          } else {
            // We need to save element from before, because user will buy all of those coins
            var previousElement = this.asks[index - 1];
            // Price from previous element
            var tpmPrice = previousElement[0] * previousElement[1];
            // We need to calculate how much we need to buy from current element
            var amountLeft = this.buyAmount - previousElement[1];
            // We must add the remaining price to previous.
            tpmPrice += amountLeft * element[0];
            this.totalPriceForBuyAmount = tpmPrice;
          }
        }
      }
    }
  }

  private getChartOptionsForData(ob: OrderBook) {
    return {
      chart: {
        type: "area",
        zoomType: "xy",
      },
      title: {
        text: "",
      },
      xAxis: {
        minPadding: 0,
        maxPadding: 0,
        title: {
          text: "Price",
        },
      },
      yAxis: [
        {
          lineWidth: 1,
          gridLineWidth: 1,
          title: null,
          tickWidth: 1,
          tickLength: 5,
          tickPosition: "inside",
          labels: {
            align: "left",
            x: 8,
          },
        },
        {
          opposite: true,
          linkedTo: 0,
          lineWidth: 1,
          gridLineWidth: 0,
          title: null,
          tickWidth: 1,
          tickLength: 5,
          tickPosition: "inside",
          labels: {
            align: "right",
            x: -8,
          },
        },
      ],
      legend: {
        enabled: false,
      },
      plotOptions: {
        area: {
          fillOpacity: 0.2,
          lineWidth: 1,
          step: "center",
        },
      },
      tooltip: {
        headerFormat:
          '<span style="font-size=10px;">Price: {point.key}</span><br/>',
        valueDecimals: 2,
      },
      series: [
        {
          name: "Bids",
          data: ob.bids,
          color: "#03a7a8",
        },
        {
          name: "Asks",
          data: ob.asks,
          color: "#fc5857",
        },
      ],
    };
  }
}
