import { BtcDataService } from "./btc-data.service";
import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent implements OnInit {
  constructor(private _btcDataService: BtcDataService) {}

  ngOnInit() {
    // Start signalR connection and start listening for btc data
    this._btcDataService.startConnection();
    this._btcDataService.addTransferChartDataListener();
  }
}
