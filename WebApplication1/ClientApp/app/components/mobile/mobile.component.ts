import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-mobile',
  templateUrl: './mobile.component.html',
  styleUrls: ['./mobile.component.css']
})
export class MobileComponent implements OnInit {
  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];

  //lastDate: Date;

  constructor() { }

  public sendMessage(): void {
      const data = `Sent: ${this.message}`;

      this._hubConnection.invoke('Send', data);
      this.messages.push(data);
  }

  refreshData() {
      //const data = this.lastDate;
      //console.log(data);

      //this._hubConnection.invoke('Send', data.toString());
      //this.messages.push(data.toString());
  }

  ngOnInit() {
      //this.lastDate = new Date(Date.now())
      this._hubConnection = new HubConnection('http://localhost:55202/loopy');

      this._hubConnection.on('Send', (data: any) => {
          const received = `Received: ${data}`;
          this.messages.push(received);
      });

      this._hubConnection.start()
          .then(() => {
              console.log('Hub connection started')
          })
          .catch(() => {
              console.log('Error while establishing connection')
          });

      let timer = Observable.timer(2000, 2000);
      timer.subscribe(t => this.refreshData());
  }

  eoinject = require("../../assets/images/EO_Inject.png");
}
