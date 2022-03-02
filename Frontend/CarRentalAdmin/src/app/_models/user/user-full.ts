import { UserMinimal } from "./user-minimal";

export interface UserFull extends UserMinimal {
  driverLicenseSerialNumber?: string;
  roles: Roles[];
}

export enum Roles {
  None = 0,
  Client = 1,
  Admin = 2,
  SuperAdmin = 3
}
