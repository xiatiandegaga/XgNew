import { defineStore } from 'pinia';
import { TOKEN_NAME } from '@/config/global';
import { store, usePermissionStore } from '@/store';
import { accountLogin } from '@/api/permission';
const InitUserInfo = {
  roles: [], // 前端权限模型使用 如果使用请配置modules/permission-fe.ts使用
};

export const useUserStore = defineStore('user', {
  state: () => ({
    token: localStorage.getItem(TOKEN_NAME), // 默认token不走权限
    userInfo: { ...InitUserInfo },
  }),
  getters: {
    roles: (state) => {
      return state.userInfo?.roles;
    },
  },
  actions: {
    async login(userInfo: Record<string, unknown>) {
      const res = await accountLogin(userInfo);
      this.token = `Bearer ${res.token}`;
      localStorage.setItem(TOKEN_NAME,this.token);
    },
    // async getUserInfo() {
    //   const mockRemoteUserInfo = async (token: string) => {
    //     if (token === 'main_token') {
    //       return {
    //         name: 'td_main',
    //         roles: ['all'], // 前端权限模型使用 如果使用请配置modules/permission-fe.ts使用
    //       };
    //     }
    //     return {
    //       name: 'td_dev',
    //       roles: ['UserIndex', 'DashboardBase', 'login'], // 前端权限模型使用 如果使用请配置modules/permission-fe.ts使用
    //     };
    //   };
    //   const res = await mockRemoteUserInfo(this.token);

    //   this.userInfo = res;
    // },
    async logout() {
      localStorage.removeItem(TOKEN_NAME);
      this.token = '';
      this.userInfo = { ...InitUserInfo };
    },
    async removeToken() {
      this.token = '';
    },
  },
  persist: {
    afterRestore: () => {
      const permissionStore = usePermissionStore();
      permissionStore.initRoutes();
    },
  },
});

export function getUserStore() {
  return useUserStore(store);
}
