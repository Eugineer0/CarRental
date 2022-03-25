import { RentPeriodFilter } from "./rent-period-filter";

export interface RentalCenterFilter {
  country: string | undefined;
  city: string  | undefined;
  minimumCarsAvailable: number | undefined;
  rentPeriodFilter: RentPeriodFilter | undefined;
}
