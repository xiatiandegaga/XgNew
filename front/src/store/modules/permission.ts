import { defineStore } from 'pinia';
import { RouteRecordRaw } from 'vue-router';
import router, { fixedRouterList, homepageRouterList } from '@/router';
import { store } from '@/store';
import { RouteItem,MenuModal } from '@/api/model/permissionModel';
import { getMenuList,getAllList } from '@/api/permission';
import { transformObjectToRoute } from '@/utils/route';
import { getTreeList } from '@/utils/tree';
export const usePermissionStore = defineStore('permission', {
  state: () => ({
    // 权限代码列表
    permCodeList: [],
    whiteListRouters: ['/login'],
    routers: [],
    removeRoutes: [],
    asyncRoutes: [],
  }),
  getters: {
    getPermCodeList(): string[] | number[] {
      return this.permCodeList;
    }
  },
  actions: {
    setPermCodeList(codeList: string[]) {
      this.permCodeList = codeList;
    },
    async initRoutes() {
      const accessedRouters = this.asyncRoutes;

      // 在菜单展示全部路由
      // this.routers = [...homepageRouterList, ...accessedRouters, ...fixedRouterList];
      // 在菜单只展示动态路由和首页
      this.routers = [...homepageRouterList, ...accessedRouters];
      // 在菜单只展示动态路由
      // this.routers = [...accessedRouters];
    },
    async buildAsyncRoutes() {
      try {
        // 发起菜单权限请求 获取菜单列表
        const userMenu = await getMenuList();
        const dynamicMenu = await this.resetMenuList(userMenu)
        dynamicMenu.forEach((x) => {
          const meta = {
            icon: x.metaIcon==''?null:x.metaIcon,
            title: x.metaTitle,
            showLink: true,
            showParent: true
          };
          x.meta = meta;
        });
        const asyncRoutes: Array<RouteItem>= getTreeList(dynamicMenu);;
        this.asyncRoutes = transformObjectToRoute(asyncRoutes);
        await this.initRoutes();
        return this.asyncRoutes;
      } catch (error) {
        // throw new Error("Can't build routes");
      }
    },
   // 根据菜单路由 重组 动态路由
   async resetMenuList(userMenu:Array<MenuModal>){
    try {
      // 发起菜单权限请求 获取菜单列表
      const allMenu = await getAllList();
      userMenu.forEach((item:MenuModal) =>{
        var pItem=userMenu.filter(x=>x.id==item.pid)
        if(!pItem||pItem.length==0)
          {
            const parent = allMenu.filter((x:MenuModal) =>x.id == item.pid) 
            if(parent && parent.length > 0){
              userMenu.push(parent[0])
              this.getUserMenu(userMenu,allMenu,parent[0].pid)
            }
          }
      })
      const btnList = userMenu.filter(x =>x.category !== 1)
      const codeList = btnList.map((v) => {return v.permission})
      this.setPermCodeList(codeList);
      const newList = userMenu.filter(x =>x.category !== 2)
      const dynamicMenu = [...new Set(newList)]
      return dynamicMenu
    } catch (error) {
      // throw new Error("Can't build routes");
    }
  },
  getUserMenu(userMenu,allMenu,pid)
  {
    if(pid!=0)
      {
        var pItem=userMenu.filter(x=>x.id==pid)
        if(!pItem||pItem.length==0)
          {
            const parent = allMenu.filter((x:MenuModal) =>x.id == pid) 
            if(parent && parent.length > 0){
              userMenu.push(parent[0])
              this.getUserMenu(userMenu,allMenu,parent[0].pid)
            }
          }
      } 
  },
    async restoreRoutes() {
      this.removeRoutes.forEach((item: RouteRecordRaw) => {
        router.addRoute(item);
      });
    },
  },
});

export function getPermissionStore() {
  return usePermissionStore(store);
}
