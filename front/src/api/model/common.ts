export interface Params{
 pageIndex?: number,
 pageSize?: number,
 filter ?: Array<ParamsFilter>
}
export interface ParamsFilter{
 propertyName: string,
 op: number,
 value: string | number
}