export interface User {
  id: string;
  fullName: string;
  userName: string;
  active: boolean;
  idRole: string;
  role: string;
  token: string;
  refreshToken: string;
  permissions: Permission[];
  taskPending: number;
  taskInProcess: number;
  taskCompleted: number;
  taskDue: number;
  dateCreated: string;
  idUserCreated: string;
  userCreated: string;
  dateUpdated: any;
  idUserUpdated: any;
  userUpdated: any;
}

export interface Permission {
  entity: string
  idActionType: number
  actionType: string
  id: string
  dateCreated: string
  idUserCreated: string
  userCreated: string
  dateUpdated: any
  idUserUpdated: any
  userUpdated: any
}
