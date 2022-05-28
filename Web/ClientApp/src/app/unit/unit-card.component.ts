import { Component, Input, OnInit } from '@angular/core';
import { UnitStatus } from '../_enum/unit-status';
import { UnitResponse } from '../_responses/unit-response';

@Component({
  selector: 'app-unit-card',
  templateUrl: './unit-card.component.html',
  styleUrls: ['./unit-card.component.css']
})
export class UnitCardComponent implements OnInit {

  constructor() { }

  @Input()
  unit!: UnitResponse;

  status(): string {
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
    if (this.unit.unitStatus == UnitStatus.OFFLINE)
      return `<div class="text-muted" > <i class="bi-fan" > </i> -</div>`;
    if (this.unit.fanStatus)
      return `<div class="text-warning" > <i class="bi-fan" > </i> Running</div>`;
    return `<div class="text-dark" > <i class="bi-fan" > </i> Idle</div>`;
  }

  ngOnInit(): void {
  }

}
