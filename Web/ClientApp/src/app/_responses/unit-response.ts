import { UnitStatus } from "../_enum/unit-status";

export interface UnitResponse {
  id: string;
  name: string;
  organizationID: string;
  temperature: number;
  unitStatus: UnitStatus;
  fanStatus: boolean;
  updatedTime: Date;
}
