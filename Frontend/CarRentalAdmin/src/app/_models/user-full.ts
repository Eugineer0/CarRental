import { UserMinimal } from "./user-minimal";
import { UserRoles } from "./roles";

export interface UserFull extends UserMinimal {
  driverLicenseSerialNumber?: string;
  roles: UserRoles;
}
