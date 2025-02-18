<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="目录名称" name="categoryName">
                <t-input v-model="formData.categoryName" class="form-item-content" type="search" placeholder="请输入目录名称"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="商品类型" name="typeName">
             
                    <t-select v-model="formData.mallProductTypeId" :style="{ minWidth: '134px' }" placeholder="请选择商品类型">
            <t-option v-for="item in typeList" :key="item.id" :value="item.id" :label="item.typeName">
              {{ item.typeName }}
            </t-option>
          </t-select>
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" :style="{ marginLeft: 'var(--td-comp-margin-s)' }" @click="fetchData">
            查询
          </t-button>
          <t-button v-auth="'mallBrand:add'" @click="handleClickAdd()"> 新建 </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>
        </t-col>
      </t-row>
    </t-form>

    <div class="table-container">
      <t-enhanced-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true"
        :tree="treeConfig" :loading="dataLoading" :expand-on-row-click="true">
        <template #op="slotProps">
          <a v-auth="'mallBrand:edit'" class="t-button-link" @click="handleClickUpdate(slotProps)">编辑</a>
          <t-popconfirm content="确认删除吗" @confirm="handleClickDelete(slotProps)">
            <a v-auth="'mallBrand:delete'" class="t-button-link">删除</a>
          </t-popconfirm>
        </template>
      </t-enhanced-table>
    </div>
    <mall-product-category-form ref="mallProductCategoryForm" @parent-fetch-data="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { MessagePlugin } from 'tdesign-vue-next';
import { getCategoryAllList, logicDelete } from '@/api/mall/mallProductCategory';
import { getTypeAllList } from '@/api/mall/mallProductType';
import { COLUMNS, SEARCHFORM } from './index.d';
import { getTreeList } from '@/utils/tree';
import MallProductCategoryForm from './components/MallProductCategoryForm.vue';

let data = reactive([]); // 列表数据
let typeList = ref([]); // 列表数据
const formData = ref({ ...SEARCHFORM }); // 搜索数据
const dataLoading = ref(false); // 控制弹窗是否打开

onMounted(async() => {
  await getTypeProductList();
 fetchData();
});
 
// 查询目录列表
const fetchData = async () => {
  dataLoading.value = true;
  const filterInfo: any = [];
  if (formData.value.categoryName)
    filterInfo.push({ op: 5, propertyName: 'categoryName', value: formData.value.categoryName });
  if (formData.value.mallProductTypeId)
    filterInfo.push({ op: 0, propertyName: 'mallProductTypeId', value: formData.value.mallProductTypeId });
  try {
    const list = await getCategoryAllList({ filter: filterInfo });
    data = getTreeList(list);
  } catch (e) {
  } finally {
    dataLoading.value = false;
  }
  getProductTypeName();
};

const getTypeProductList = async () => {
  typeList.value = await getTypeAllList();
  console.log('typeList',typeList)
};

const getProductTypeName = () => {
  data.forEach((x) => {
    typeList.value.forEach((item) => {
      if (item.id === x.mallProductTypeId) {
        x.typeName = item.typeName;
      }
    });
  });
};

const treeConfig = reactive({ childrenKey: 'children', treeNodeColumnIndex: 0 });

// 获取dialogForm元素节点
const mallProductCategoryForm = ref<any>(null);

// 新建Modal
const handleClickAdd = () => {
  mallProductCategoryForm.value.add();
};
// 编辑Modal
const handleClickUpdate = ({ row }) => {
 
  mallProductCategoryForm.value.update(row.id);
};

// 根据Id删除品牌
const handleClickDelete = async ({ row }) => {
  try {
    await logicDelete(row.id);
    fetchData();
    MessagePlugin.success('删除成功');
  } catch (e) {
    MessagePlugin.warning('删除失败');
  }
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
