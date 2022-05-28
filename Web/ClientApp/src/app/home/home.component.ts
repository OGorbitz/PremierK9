import { UnitStatus } from '../_enum/unit-status'
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../../environments/environment';
import { UnitResponse } from '../_responses/unit-response';
import { Observable, Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  units: UnitResponse[] = [];
  refTimer: Observable<number>;
  refSub: Subscription | undefined;
  errorText: string = "";

  constructor(private http: HttpClient) {
    this.refresh();
    this.refTimer = timer(1000, 1000);
  }

  ngOnInit() {
    //this.refSub = this.refTimer.subscribe(() => this.refresh());
  }

  ngOnDestroy() {
    if (this.refSub)
      this.refSub.unsubscribe();
  }

  refresh() {
    this.http.get<UnitResponse[]>(environment.apiUrl + 'unit/getunits').subscribe(response => {
      if (response) {
        this.units = response;
      }
    }, error => console.error(error));
  }

}
