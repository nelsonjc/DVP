import { Permission } from "../user/user.model"

export interface Role {
    id: string
    name: string
    active: boolean
    rolPermissions: Permission[]
  }