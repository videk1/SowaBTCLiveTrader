import { OrderBook } from "./models/OrderBookData";
import { Injectable, Inject } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class BtcDataService {
  data: Subject<OrderBook> = new Subject<OrderBook>();

  private url: string;

  private hubConnection: signalR.HubConnection;

  constructor(@Inject("BASE_URL") baseUrl: string) {
    this.url = baseUrl.replace("https://", "");
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://${this.url}btchub`)
      .build();
    this.hubConnection
      .start()
      .then(() => console.log("Connection started"))
      .catch((err) => console.log("Error while starting connection: " + err));
  };

  public addTransferChartDataListener = () => {
    this.hubConnection.on("sendbtcdata", (data) => {
      this.data.next(<OrderBook>JSON.parse(data));
    });
  };
}
