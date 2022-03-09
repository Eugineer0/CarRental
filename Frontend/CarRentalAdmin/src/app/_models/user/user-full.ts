export interface User {
  email: string;
  username: string;
  name: string;
  surname: string;
  dateOfBirth: Date;
  passportNumber: string;
  driverLicenseSerialNumber?: string;
  roles: Roles[];
  approvalRequested: boolean;
}

export enum Roles {
  None = 0,
  Client = 1,
  Admin = 2,
  SuperAdmin = 3
}
