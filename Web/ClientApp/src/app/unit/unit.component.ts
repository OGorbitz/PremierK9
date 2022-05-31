import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { UnitStatus } from '../_enum/unit-status';
import { UnitResponse } from '../_responses/unit-response';

@Component({
  selector: 'app-unit',
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.css']
})
export class UnitComponent implements OnInit {
  unitId: string = "";
  unit: UnitResponse | undefined;

  constructor(private route: ActivatedRoute, private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      if (this.unitId == null)
        this.router.navigate(["/index"]);
      this.unitId = params.get('id') ?? "";
      this.refresh();
    });
  }
  refresh() {
    this.http.get<UnitResponse>(environment.apiUrl + 'unit/getunit?id=' + this.unitId).subscribe(response => {
      if (response) {
        this.unit = response;
        this.unit.updatedTime = new Date(this.unit.updatedTime);
      }
    }, error => { console.error(error) });
  }

  openBtn() {
    this.http.get<UnitResponse>(environment.apiUrl + 'unit/openunit?id=' + this.unitId).subscribe(response => {
      if (response) {
        this.unit = response;
        this.unit.updatedTime = new Date(this.unit.updatedTime);
      }
    }, error => { console.error(error) });
  }

  updated(): string {
    if (this.unit == null)
      return "";

    return String(this.unit.updatedTime.getHours()).padStart(2, '0') + ":" + String(this.unit.updatedTime.getMinutes()).padStart(2, '0');
  }

  status(): string {
    if (this.unit == null)
      return "";

    switch (this.unit.unitStatus) {
      case UnitStatus.CLOSED:
        return `<div class="text-success" > <i class="bi-lock-fill" > </i> Closed</div>`
      case UnitStatus.AUTO_OPENED:
        return `<div class="text-danger" > <i class="bi-exclamation-circle-fill" > </i> Auto Opened</div>`;
      case UnitStatus.MAN_OPENED:
        return `<div class="text-warning" > <i class="bi-unlock-fill" > </i> Opened</div>`;
      case UnitStatus.OFFLINE:
        return `<div class="text-muted" > <i class="bi-question-circle-fill" > </i> Offline</div>`;
      default:
        return `<div class="text-warning" > <i class="bi-exclamation-circle-fill" > </i> Error</div>`;
    }
  }

  temp(): string {
    if (this.unit == null)
      return "";

    if (this.unit.unitStatus == UnitStatus.OFFLINE)
      return `<div class="text-muted" > <i class="bi-thermometer-low" > </i> -</div>`
    let val = `<div class="text-info" > <i class="bi-thermometer-snow" > </i>`
    if (this.unit.temperature > 50)
      val = `<div class="text-dark" > <i class="bi-thermometer-low" > </i>`
    if (this.unit.temperature > 70)
      val = `<div class="text-warning" > <i class="bi-thermometer-half" > </i>`
    if (this.unit.temperature > 90)
      val = `<div class="text-danger" > <i class="bi-thermometer-high" > </i>`
    val += this.unit.temperature + `</div>`
    return val;
  }

  fan(): string {
    if (this.unit == null)
      return "";

    if (this.unit.unitStatus == UnitStatus.OFFLINE)
      return `<div class="text-muted" > <i class="bi-fan" > </i> -</div>`;
    if (this.unit.fanStatus)
      return `<div class="text-warning" > <i class="bi-fan" > </i> Running</div>`;
    return `<div class="text-dark" > <i class="bi-fan" > </i> Idle</div>`;
  }

}
