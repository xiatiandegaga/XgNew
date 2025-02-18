export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 account :  string ,
 [propName:string]:any
 userRoles: Array<Roles>
}
export interface Roles  {
  id : string,
  userId : string,
  roleId : string
}