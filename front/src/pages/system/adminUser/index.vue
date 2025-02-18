<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="用户账号" name="account">
                <t-input v-model="formData.account" class="form-item-content" type="search" placeholder="请输入用户账号"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>

          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" @click="fetchData" :style="{ marginLeft: 'var(--td-comp-margin-s)' }"> 查询
          </t-button>
          <t-button @click="handleClickAdd()" v-auth="'adminUser:add'"> 新建 </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>
        </t-col>
      </t-row>
    </t-form>

    <div class="table-container">
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <template #op="slotProps">
          <a class="t-button-link" @click="handleClickUpdate(slotProps)" v-auth="'adminUser:edit'">编辑</a>
          <t-popconfirm content="确认删除吗" @confirm='handleClickDelete(slotProps)' v-if="slotProps.row.isSystem == 0">
            <a class="t-button-link" v-auth="'adminUser:delete'">删除</a>
          </t-popconfirm>
          <t-popconfirm content="确认要重置密码吗" @confirm='resetPassword(slotProps)' v-if="slotProps.row.isSystem == 0">
            <a class="t-button-link" v-auth="'adminUser:reset'">重置密码</a>
            <!-- <t-button theme="primary" variant="text">重置密码</t-button> -->
          </t-popconfirm>
        </template>
      </t-table>
    </div>
    <admin-user-form ref="adminUserForm" @parentFetchData="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { getPageList, logicDelete, restPassword } from '@/api/system/adminUser';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import AdminUserForm from './components/AdminUserForm.vue';

let data = reactive([]);                      // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION);      // 分页数据
const dataLoading = ref(false);               // 控制弹窗是否打开

onMounted(() => {
  fetchData();
});

// 查询用户列表
const fetchData = async () => {
  dataLoading.value = true;
  const filterInfo = filter();
  try {
    const params: Params = {
      pageIndex: pagination.current,
      pageSize: pagination.pageSize,
      filter: filterInfo
    }
    const { list, totalCount } = await getPageList(params);
    data = list
    pagination.total = parseInt(totalCount);
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
};
// 查询参数
const filter = () => {
  const filterInfo: Array<ParamsFilter> = [];
  if (formData.account)
    filterInfo.push({ op: 5, propertyName: 'account', value: formData.account });
  return filterInfo
}
// 获取dialogForm元素节点
const adminUserForm = ref<any>(null);;

// 新建Modal
const handleClickAdd = () => {
  adminUserForm.value.add()
};
// 编辑Modal
const handleClickUpdate = ({ row }) => {
  adminUserForm.value.update(row.id)
};

// 根据Id删除用户
const handleClickDelete = async ({ row }) => {
  try {
    await logicDelete(row.id)
    fetchData();
    MessagePlugin.success('删除成功');
  } catch (e) {
    MessagePlugin.warning('删除失败');
  }
};
// 重置密码
const resetPassword = async ({ row }) => {
  try {
    await restPassword(row.id)
    console.log('await restPassword', await restPassword(row.id));
    MessagePlugin.success('重置成功');
  } catch (e) {
    MessagePlugin.warning('重置失败');
  }
};

// 点击下一页
const onPageChange = (pageInfo: PageInfo,) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
    fetchData()
};

</script>

<style lang="less" scoped>
.list-common-table {
  background-color: var(--td-bg-color-container);
  padding: var(--td-comp-paddingTB-xxl) var(--td-comp-paddingLR-xxl);
  border-radius: var(--td-radius-medium);

  .table-container {
    margin-top: var(--td-comp-margin-xxl);
  }
}

.form-item-content {
  width: 100%;
}

.operation-container {
  display: flex;
  justify-content: flex-end;
  align-items: center;

  .expand {
    .t-button__text {
      display: flex;
      align-items: center;
    }
  }
}

.payment-col {
  display: flex;

  .trend-container {
    display: flex;
    align-items: center;
    margin-left: var(--td-comp-margin-s);
  }
}
</style>