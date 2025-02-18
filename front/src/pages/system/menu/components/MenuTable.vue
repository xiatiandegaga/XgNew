<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="菜单标题" name="metaTitle">
                <t-input v-model="formData.metaTitle" class="form-item-content" type="search" placeholder="请输入菜单名称"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" @click="fetchData" :style="{ marginLeft: 'var(--td-comp-margin-s)' }"> 查询
          </t-button>
          <t-button @click="handleClickAdd()" v-auth="'menu:add'"> 新建 </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>

        </t-col>
      </t-row>
    </t-form>
    <div class="table-container">
      <t-enhanced-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true"
        :tree="treeConfig" :loading="dataLoading" :expand-on-row-click="true" @page-change="onPageChange">
        <template #op="slotProps">
          <a class="t-button-link" @click="handleClickUpdate(slotProps)" v-auth="'menu:edit'">编辑</a>
          <a class="t-button-link" @click="handleClickDelete(slotProps)" v-auth="'menu:delete'">删除</a>
        </template>
      </t-enhanced-table>
    </div>
    <menu-form ref="menuForm" @parentFetchData="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';
import { logicDelete } from '@/api/menu';
import { getAllList } from '@/api/permission';
import MenuForm from './MenuForm.vue';
import { getTreeList } from '@/utils/tree';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '菜单标题',
    fixed: 'left',
    width: 100,
    ellipsis: true,
    align: 'left',
    colKey: 'metaTitle',
  },
  { title: '菜单图标', colKey: 'metaIcon', width: 100 },
  { title: '权限标识', colKey: 'permission', width: 100 },
  { title: '菜单路径', colKey: 'path', width: 100 },
  {
    title: '路由地址',
    width: 160,
    ellipsis: true,
    colKey: 'component',
  },
  {
    title: '菜单排序',
    width: 160,
    ellipsis: true,
    colKey: 'sortNo',
  },
  {
    align: 'left',
    fixed: 'right',
    width: 160,
    colKey: 'op',
    title: '操作',
  },
];

const searchForm = {
  metaTitle: '',
  status: 1,
};

const formData = ref({ ...searchForm });

const pagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0,
  showJumper: true,
});

const onPageChange = (pageInfo, context) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
};

const data = ref([]);

const dataLoading = ref(false);

const fetchData = async () => {
  dataLoading.value = true;
  const filterInfo: any = [];
  if (formData.value.metaTitle)
    filterInfo.push({ op: 5, propertyName: 'metaTitle', value: formData.value.metaTitle });
  try {
    const menuList = await getAllList({ filter: filterInfo });
    data.value = getTreeList(menuList)
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
};
const treeConfig = reactive({ childrenKey: 'children', treeNodeColumnIndex: 0, });


onMounted(() => {
  fetchData();
});

const handleClickDelete = async ({ row }) => {
  await logicDelete({ id: row.id })
  fetchData();
};

const menuForm = ref();

const handleClickAdd = () => {
  menuForm.value.add()
};

const handleClickUpdate = ({ row }) => {
  menuForm.value.update(row.id)
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
