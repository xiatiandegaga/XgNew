<template>
  <div class="list-common-table">
    <t-row>
      <t-col :span="4">
        <t-row>
          <t-col :span="12">
            <t-select-input :value="searchValue" placeholder="搜索" allow-input clearable style="width: 300px"
              @input-change="searchGlobal">
              <template #suffixIcon><search-icon /></template>
            </t-select-input>
            <t-button @click="handleClickAdd()" v-auth="'globalData:add'"> 新建 </t-button>
            <t-button @click="handleClickUpdate()" v-auth="'globalData:edit'"> 修改 </t-button>
          </t-col>
          <t-col :span="6" style="margin-top: var(--td-comp-margin-xxl)">
            <t-list :split="true">
              <t-list-item :style="item.id == selectGlobal ? 'background-color: var(--td-brand-color-active);' : ''"
                v-for="item in globalData" :key="item.id" @click="fetchDetailData(item.code, item.id)">
                <p :style="item.id == selectGlobal ? 'color: #fff;' : 'color: #000;'">
                  {{ item.name }}</p>
              </t-list-item>
            </t-list>
          </t-col>
        </t-row>
        <global-data-form ref="globalDataForm" @parentFetchData="fetchData" />
      </t-col>
      <t-col :span="8">
        <t-row>
          <t-col :span="12">
            <t-select-input :value="searchDetailValue" placeholder="请输入任意关键词" allow-input clearable style="width: 300px"
              @input-change="searchDetailGlobal">
              <template #suffixIcon><search-icon /></template>
            </t-select-input>
            <t-button @click="handleClickDetailAdd()" v-auth="'globalData:detailAdd'"> 新建 </t-button>
          </t-col>
        </t-row>
        <div class="table-container">
          <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true"
            :pagination="pagination" :loading="dataLoading" @page-change="onPageChange">
            <template #op="slotProps">
              <a class="t-button-link" @click="handleClickDetailUpdate(slotProps)" v-auth="'globalData:detailEdit'">编辑</a>
              <t-popconfirm content="确认删除吗" @confirm='handleClickDelete(slotProps)'>
                <a class="t-button-link" v-auth="'globalData:detailDelete'">删除</a>
              </t-popconfirm>
            </template>
          </t-table>
        </div>
        <global-data-detail-form ref="globalDataDetailForm" @parentFetchData="fetchDetailData" />

      </t-col>
    </t-row>
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { getAllList, } from '@/api/system/globalData';
import { getPageList, deleteById } from '@/api/system/globalDataDetail';
import { COLUMNS, PAGINATION, } from './index.d';
import type { Params, } from '@/api/model/common';
import { PageInfo, InputValue, MessagePlugin } from 'tdesign-vue-next';
import { SearchIcon } from 'tdesign-icons-vue-next';
import GlobalDataForm from './components/GlobalDataForm.vue';
import GlobalDataDetailForm from './components/GlobalDataDetailForm.vue';
import { useFrameKeepAlive } from '@/layouts/frame/useFrameKeepAlive';

let globalData = ref([]); // 列表数据
const searchValue = ref()
const selectGlobal = ref('') // 选中的数据字典
const selectCode = ref('') // 选中的数据字典code
let data = reactive([]);     // 子表列表数据
const searchDetailValue = ref()
const pagination = reactive(PAGINATION);      // 分页数据
const dataLoading = ref(false);               // 控制弹窗是否打开

onMounted(() => {
  fetchData();
});

// 查询数据字典主表列表
const fetchData = async () => {
  dataLoading.value = true;
  try {
    const params: Params = {
      filter: []
    }
    if (searchValue.value)
      params.filter.push({ op: 5, propertyName: 'name', value: searchValue.value })
    globalData.value = await getAllList(params);
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
};
// 搜索
const searchGlobal = (value: InputValue) => {
  searchValue.value = value
  fetchData();
}
// 查询数据字典子表列表
const fetchDetailData = async (code: string, id: string) => {
  if (code && id) {
    selectCode.value = code
    selectGlobal.value = id
  }
  dataLoading.value = true;
  try {
    const params: Params = {
      pageIndex: pagination.current,
      pageSize: pagination.pageSize,
      filter: []
    }
    if (code || selectCode.value)
      params.filter.push({ op: 0, propertyName: 'code', value: code || selectCode.value })
    if (searchDetailValue.value)
      params.filter.push({ op: 5, propertyName: 'name', value: searchDetailValue.value })
    const { list, totalCount } = await getPageList(params);
    data = list
    pagination.total = parseInt(totalCount);
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
};

// 子表查询参数
const searchDetailGlobal = (value: InputValue) => {
  searchDetailValue.value = value
  fetchData();
}
// 获取dialogForm元素节点
const globalDataForm = ref<any>(null);;

// 新建Modal
const handleClickAdd = () => {
  globalDataForm.value.add()
};

// 编辑Modal
const handleClickUpdate = () => {
  if (!selectGlobal.value) {
    MessagePlugin.warning('请先选择主字典信息');
    return;
  }
  globalDataForm.value.update(selectGlobal.value)
};

// 获取dialogForm元素节点
const globalDataDetailForm = ref<any>(null);;

// 新建Modal
const handleClickDetailAdd = () => {
  if (!selectGlobal.value) {
    MessagePlugin.warning('请先选择主字典信息');
    return;
  }
  let code = globalData.value.filter((x) => x.id === selectGlobal.value)[0].code
  globalDataDetailForm.value.add(code)
};

// 编辑Modal
const handleClickDetailUpdate = ({ row }) => {
  globalDataDetailForm.value.update(row.id)
};

// 根据Id删除用户
const handleClickDelete = async ({ row }) => {
  try {
    await deleteById(row.id)
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
  fetchDetailData(null,null)
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