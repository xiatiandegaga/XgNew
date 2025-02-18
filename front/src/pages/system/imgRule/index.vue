<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="编号" name="imgRuleCode">
                <t-input v-model="formData.imgRuleCode" class="form-item-content" type="search" placeholder="请输入编号"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="名称" name="imgRuleName">
                <t-input v-model="formData.imgRuleName" class="form-item-content" type="search" placeholder="请输入名称"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" @click="fetchData" :style="{ marginLeft: 'var(--td-comp-margin-s)' }"> 查询 </t-button>
          <t-button @click="handleClickAdd()" v-auth="'imgRule:add'"> 新建 </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>
        </t-col>
      </t-row>
    </t-form>
    <div class="table-container">
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <template #op="slotProps">
          <a class="t-button-link" @click="handleClickUpdate(slotProps)" v-auth="'imgRule:edit'">编辑</a>
          <t-popconfirm content="确认删除吗" @confirm='
            handleClickDelete(slotProps)'>
            <a class="t-button-link" v-auth="'imgRule:delete'">删除</a>
          </t-popconfirm>
        </template>
      </t-table>
    </div>
    <img-rule-form ref="imgRuleForm" @parentFetchData="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { getPageList, logicDelete } from '@/api/system/imgRule';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import ImgRuleForm from './components/ImgRuleForm.vue';

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
    console.log(e)
    MessagePlugin.warning('请求失败');
  } finally {
    dataLoading.value = false;
  }
};
// 查询参数
const filter = () => {
  const filterInfo: Array<ParamsFilter> = [];
  if (formData.imgRuleCode)
    filterInfo.push({ op: 5, propertyName: 'imgRuleCode', value: formData.imgRuleCode });
  if (formData.imgRuleName)
    filterInfo.push({ op: 5, propertyName: 'imgRuleName', value: formData.imgRuleName });
  return filterInfo
}
// 获取dialogForm元素节点
const imgRuleForm = ref<any>(null);

// 新建Modal
const handleClickAdd = () => {
  imgRuleForm.value.add()
};
// 编辑Modal
const handleClickUpdate = ({ row }) => {
  imgRuleForm.value.update(row.id)
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