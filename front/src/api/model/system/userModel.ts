export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 id : string,
 account :  string ,
 password :  string ,
 realName :  string ,
 nickName :  string ,
 mobile :  string ,
 img :  string ,
 memberPoint : number,
 isVerifyPhone : number,
 status : number,
 pid : string,
 referId : string,
 [propName:string]:any
}