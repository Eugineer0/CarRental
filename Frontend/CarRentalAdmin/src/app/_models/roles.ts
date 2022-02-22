export interface UserRoles {
  roles: Roles[];
}

export enum Roles
{
  None,
  Client,
  Admin,
  SuperAdmin
}
