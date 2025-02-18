import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/system/imgRuleModel';
import type { Params } from '@/api/model/common';
import { ContentTypeEnum } from '@/constants';

const Api = {
 GetPageList: '/ImgRule/GetPageList',
 AddOrUpdate: '/ImgRule/AddOrUpdate',
 FindSingleById: '/ImgRule/FindSingleById',
 LogicDelete: '/ImgRule/Delete',
 UploadFile:'/File/uploadFile'
};



export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data 
 });
}

export function addOrUpdate(data: ListModel) {
 return request.post({
   url: Api.AddOrUpdate,
   data 
 });
}

export function findSingleById(id:String) {
 return request.post({
   url: Api.FindSingleById,
   data: { id }
 });
}

export function logicDelete(id:String) {
 return request.post({
   url: Api.LogicDelete,
   data: { id }
 });
}

export function uploadFile(file) {
  return request.post({
    url: Api.UploadFile,
    headers: { 'Content-Type': ContentTypeEnum.FormData },
    data: file
  });
 }
