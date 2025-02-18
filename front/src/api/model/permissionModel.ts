import { defineComponent } from 'vue';

export interface MenuListResult {
  data: Array<RouteItem>;
}

export type Component<T = any> =
  | ReturnType<typeof defineComponent>
  | (() => Promise<typeof import('*.vue')>)
  | (() => Promise<T>);

export interface RouteItem {
  path: string;
  name: string;
  component?: Component | string;
  components?: Component;
  redirect?: string;
  meta: RouteMeta;
  children?: Array<RouteItem>;
  type: number;
}
export interface RouteMeta {
  title: string;
  icon?: string;
  expanded?: boolean;
  orderNo?: number;
  hidden?: boolean;
  hiddenBreadcrumb?: boolean;
  single?: boolean;
}

export interface MenuModal {
  pid: string;
  id: string;
  category: number;
  permission: string
}

