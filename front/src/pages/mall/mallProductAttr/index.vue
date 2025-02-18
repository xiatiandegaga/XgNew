<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="属性名" name="attrKeyName">
                <t-input v-model="formData.attrKeyName" class="form-item-content" type="search" placeholder="请输入属性名"
                  clearable :style="{ minWidth: '134px' }" />
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
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <!-- <template #mallProductCategoryId="slotProps">
          <span>
            <span>{{ getProductTypeName(slotProps.row) }}</span>
          </span>
        </template> -->

        <template #op="slotProps">
           <a v-auth="'mallBrand:edit'" class="t-button-link" @click="handleClickUpdate(slotProps,false)">查看</a>
          <a v-auth="'mallBrand:edit'" class="t-button-link" @click="handleClickUpdate(slotProps,true)">编辑</a>
          <t-popconfirm content="确认删除吗" @confirm="handleClickDelete(slotProps)">
            <a v-auth="'mallBrand:delete'" class="t-button-link">删除</a>
          </t-popconfirm>
        </template>
      </t-table>
    </div>
    <mall-product-attr-form ref="mallProductAttrForm" @parent-fetch-data="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getPageList, logicDelete } from '@/api/mall/mallProductAttrKey';
import { getTypeAllList } from '@/api/mall/mallProductType';
import { getCategoryAllList } from '@/api/mall/mallProductCategory';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import MallProductAttrForm from './components/MallProductAttrForm.vue';

let data = reactive([]); // 列表数据
let productTypeList = reactive([]);
let productCategoryList = reactive([]);
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开

onMounted(() => {
  getProductTypeList();
  getProductCategoryList();
  fetchData();
});

// 查询品牌列表
const fetchData = async () => {
  dataLoading.value = true;
  const filterInfo = filter();
  try {
    const params: Params = {
      pageIndex: pagination.current,
      pageSize: pagination.pageSize,
      filter: filterInfo,
    };
    const { list, totalCount } = await getPageList(params);
    data = list;
    pagination.total = parseInt(totalCount);
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
  getProductTypeName();
  
};

const getProductTypeName = () => {
  data.forEach((x) => {
    productTypeList.forEach((item) => {
      if (item.id === x.mallProductTypeId) {
        x.typeName = item.typeName;
      }
    });
    productCategoryList.forEach((item) => {
      if (item.id === x.mallProductCategoryId) {
        x.categoryName = item.categoryName;
      }
    });
  });
};



// 所有商品类型列表
const getProductTypeList = async () => {
  productTypeList = await getTypeAllList();
};

// 所有商品目录列表
const getProductCategoryList = async () => {
  productCategoryList = await getCategoryAllList();
};

// 查询参数
const filter = () => {
  const filterInfo: Array<ParamsFilter> = [];
  console.log('formData', formData);
  if (formData.attrKeyName) filterInfo.push({ op: 5, propertyName: 'attrKeyName', value: formData.attrKeyName });
  return filterInfo;
};

// 获取dialogForm元素节点
const mallProductAttrForm = ref<any>(null);

// 新建Modal
const handleClickAdd = () => {
  mallProductAttrForm.value.add();
};
// 编辑Modal
const handleClickUpdate = ({ row },edit:boolean) => {
  mallProductAttrForm.value.update(row.id,edit);
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

// 点击下一页
const onPageChange = (pageInfo: PageInfo) => {
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
