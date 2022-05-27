import { UnitStatus } from "../_enum/unit-status";

export interface UnitResponse {
  ID: string;
  Name: string;
  OrganizationID: string;
  Temperature: number;
  UnitStatus: UnitStatus;
  FanStatus: boolean;
  UpdatedTime: Date;
}
