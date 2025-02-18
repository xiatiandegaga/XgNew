<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="用户姓名" name="realName">
                <t-input v-model="formData.realName" class="form-item-content" type="search" placeholder="请输入用户真实姓名"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="用户昵称" name="nickName">
                <t-input v-model="formData.nickName" class="form-item-content" type="search" placeholder="请输入用户昵称"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="用户手机" name="mobile">
                <t-input v-model="formData.mobile" class="form-item-content" type="search" placeholder="请输入用户手机"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" @click="fetchData" :style="{ marginLeft: 'var(--td-comp-margin-s)' }"> 查询
          </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>
        </t-col>
      </t-row>
    </t-form>
    <div class="table-container">
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <template #op="slotProps">
          <a class="t-button-link" @click="handleClickUpdate(slotProps)" v-auth="'user:add'">编辑</a>
          <t-popconfirm content="确认删除吗" @confirm='handleClickDelete(slotProps)' v-if="slotProps.row.isSystem == 0">
            <a class="t-button-link" v-auth="'user:delete'">删除</a>
          </t-popconfirm>
        </template>
      </t-table>
    </div>
    <user-form ref="userForm" @parentFetchData="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { getPageList, logicDelete, } from '@/api/system/user';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import UserForm from './components/UserForm.vue';

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
  if (formData.nickName)
    filterInfo.push({ op: 5, propertyName: 'nickName', value: formData.nickName });
  if (formData.realName)
    filterInfo.push({ op: 5, propertyName: 'realName', value: formData.realName });
  if (formData.mobile) filterInfo.push({ op: 5, propertyName: 'mobile', value: formData.mobile });
  return filterInfo
}
// 获取dialogForm元素节点
const userForm = ref<any>(null);;

// 编辑Modal
const handleClickUpdate = ({ row }) => {
  userForm.value.updatePage(row.id)
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
// 点击下一页
const onPageChange = (pageInfo: PageInfo,) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
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