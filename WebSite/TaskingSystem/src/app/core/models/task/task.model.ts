export interface Task {
  title: string
  description: string
  dueDate: string
  idUser: string
  user: string
  active: boolean
  idStatus: string
  status: string
  statusCode: string,
  id: string
  dateCreated: string
  idUserCreated: string
  userCreated: string
  dateUpdated: any
  idUserUpdated: any
  userUpdated: any
  flows: TaskFlow[]
  logs: TaskLog[]
}

export interface TaskFlow {
  id: string;
  idPreviousStatus: string;
  previousStatus: string;
  idNewStatus: string;
  newStatus: string;
  observation: string;
  dateCreated: any;
  idUserCreated: string;
  userCreated: string;
}

export interface TaskLog {
  id: string
  typeEvent: string
  logEvent: string
  dateCreated: string
  idUserCreated: string
  userCreated: string
}