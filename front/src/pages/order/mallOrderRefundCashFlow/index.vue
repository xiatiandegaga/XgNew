<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="订单号" name="orderNo">
                <t-input v-model="formData.orderNo" class="form-item-content" type="search" placeholder="请输入订单号"
                  clearable :style="{ minWidth: '134px' }" />
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
        <template #status="{ row }">
          <span>支付成功</span>
        </template>
      </t-table>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getOrderCashFlowPageList } from '@/api/mall/mallOrderCashFlow';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
let data = reactive([]); // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开
onMounted(() => {
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
    const { list, totalCount } = await getOrderCashFlowPageList(params);
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
  filterInfo.push({ op: 0, propertyName: 'status', value: 'TRADE_REFUND' })
  if (formData.orderNo) filterInfo.push({ op: 5, propertyName: 'orderNo', value: formData.orderNo });
  return filterInfo;
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
