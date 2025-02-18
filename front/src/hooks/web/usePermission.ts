import { usePermissionStore } from "@/store";
import { intersection } from "lodash";

// 是否有权限
export function hasPermission(value?: string | string[], def = true): boolean {
 const permissionStore = usePermissionStore();
 // Visible by default
 if (!value) {
   return def;
 }
 const allCodeList = permissionStore.getPermCodeList as string[];
 if (!isArray(value)) {
  return allCodeList.includes(value);
 }
 return (intersection(value, allCodeList) as string[]).length > 0;
}

function isArray(val: any): val is Array<any> {
 return val && Array.isArray(val);
}