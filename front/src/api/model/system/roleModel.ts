export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 id : string,
 name :  string ,
 description: string,
 level: number,
 status : number,
 [propName:string]:any
 roleMenuIds: Array<RolesMenu>
}

export interface RolesMenu  {
  id : string,
  userId : string,
  roleId : string
}
