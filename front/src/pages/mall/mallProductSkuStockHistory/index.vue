<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="单号" name="stockNo">
                <t-input v-model="formData.stockNo" class="form-item-content" type="search" placeholder="请输入出入库单号"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
             <t-col :span="4">
              <t-form-item label="类型" name="stockType">
                <t-select v-model="formData.stockType" clearable   >
                  <t-option  key="1" value="1" label="入库"></t-option>
                    <t-option  key="2" value="2" label="出库"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="产品名称" name="productId">
                <t-select v-model="formData.productId" clearable filterable>
                  <t-option v-for="item in productList" :key="item.id" :value="item.id" :label="item.productName"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
             <t-col :span="4">
              <t-form-item label="细分类型" name="status">
                <t-select v-model="formData.stockDetailType" clearable>
                  <t-option v-for="item in stockDetailTypeList" :key="item.constKey" :value="item.constKey" :label="item.name"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="单据日期" name="createDate">
                <t-space width="100%">
                  <t-date-range-picker v-model="formData.createDate" clearable />
                </t-space>
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>

        <t-col :span="2" class="operation-container">
          <t-button theme="primary" :style="{ marginLeft: 'var(--td-comp-margin-s)' }" @click="fetchData">
            查询
          </t-button>
        </t-col>
      </t-row>
    </t-form>

    <div class="table-container">
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <template #stockType="{row}">
          <span>{{row.stockType==1?'入库':'出库'}}</span>
        </template>
        <template #goodsAttrs="{row}">
          <span>{{getSkuName(row.goodsAttrs)}}</span>
        </template>
      </t-table>
    </div>

  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getPageList} from '@/api/mall/mallProductSkuStockHistory';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';

import {getListByCodes} from '@/api/system/globalDataDetail';
import { GlobalDataCode,OptionSelect} from '@/utils/commonConst';
import { getAllList} from '@/api/mall/mallProduct';
import { getList as getAttrList} from '@/api/mall/mallProductAttrKey';
import { getList as getAttrValueList} from '@/api/mall/mallProductAttrValue';
let data = reactive([]); // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开
let stockDetailTypeList=ref([])
let productList=ref([])
let attrList=ref([])
let attrValueList=ref([])
onMounted(() => {
  fetchData();
  initData();
});
const initData= async ()=>{
  stockDetailTypeList.value= await  getListByCodes(GlobalDataCode.ProductInvOrRelType)
  productList.value=await getAllList()
  attrList.value= await getAttrList()
  attrValueList.value=await getAttrValueList()
}
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
};
// 查询参数
const filter = () => {
  const filterInfo: Array<ParamsFilter> = [];
  if (formData.stockNo) filterInfo.push({ op: 5, propertyName: 'stockNo', value: formData.stockNo });
  if (formData.stockType) filterInfo.push({ op: 0, propertyName: 'stockType', value: formData.stockType });
   if (formData.productId) filterInfo.push({ op: 0, propertyName: 'productId', value: formData.productId });
  if (formData.stockDetailType) filterInfo.push({ op: 0, propertyName: 'stockDetailType', value: formData.stockDetailType });
  if (formData.createDate && formData.createDate.length > 0) {
    filterInfo.push({ op: 3, propertyName: 'createDate', value: formData.createDate[0] });
    filterInfo.push({ op: 4, propertyName: 'createDate', value: `${formData.createDate[1]} 23:59:59` });
  }
  return filterInfo;
};


// 点击下一页
const onPageChange = (pageInfo: PageInfo) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
  fetchData()
};
const getSkuName = (value) => {
  let name = '';
  
  if (value) {
    JSON.parse(value).forEach((item) => {
      const attr = attrList.value.filter((x) => x.id == item.attrKeyId)
      const attrValue=attrValueList.value.filter(x=>x.id==item.attrValueId)
      if(attr&&attr[0]&&attrValue&&attrValue[0])
      {
        name+= `${attr[0].attrKeyName}:${attrValue[0].attrValueName} `
      }
    });
  }
  return !name ? '' : name.substr(0, name.length - 1);
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
