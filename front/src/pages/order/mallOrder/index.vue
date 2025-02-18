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
            <t-col :span="4">
              <t-form-item label="用户" name="userId">
                <t-select v-model="formData.userId" clearable filterable @search="remoteMethod">
                  <t-option v-for="item in userList" :key="item.id" :value="item.id" :label="item.realName"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="单据状态" name="status">
                <t-select v-model="formData.status" clearable>
                  <t-option v-for="item in statusList" :key="item.constKey" :value="item.constKey"
                    :label="item.name"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="单据日期" name="createTime">
                <t-space width="100%">
                  <t-date-range-picker v-model="formData.createTime" clearable />
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
        <template #numberOfInstallments="{ row }">
          <span>{{ row.numberOfInstallments }}期</span>
        </template>
        <template #receiveInfo="{ row }">
          <span>{{ getReceiveInfo(row.receiveInfo) }}</span>
        </template>
        <template #op="{ row }">
          <a class="t-button-link" @click="handleClickShow(row)">查看</a>
          <a v-auth="'mallOrder:send'" v-if='row.status == 1' class="t-button-link" @click="handleClickSend(row)">发货</a>
        </template>
      </t-table>
    </div>
    <OrderForm ref="mallOrderForm" @parent-fetch-data="fetchData" />
    <OrderSendForm ref="mallOrderSendForm" @parent-fetch-data="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getOrderPageList } from '@/api/mall/mallOrder';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import OrderForm from './components/OrderForm.vue';
import OrderSendForm from './components/OrderSendForm.vue';
import { getListByCodes } from '@/api/system/globalDataDetail';
import { GlobalDataCode } from '@/utils/commonConst';
import { getRemoteSerch } from '@/api/system/user';
let data = reactive([]); // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开
let statusList = ref([])
let userList = ref([])
onMounted(() => {
  fetchData();
  initData();
});
const initData = async () => {
  statusList.value = await getListByCodes(GlobalDataCode.MallOrderStatus)
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
    const { list, totalCount } = await getOrderPageList(params);
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
  if (formData.orderNo) filterInfo.push({ op: 5, propertyName: 'orderNo', value: formData.orderNo });
  if (formData.status || formData.status === 0) filterInfo.push({ op: 0, propertyName: 'status', value: formData.status });
  if (formData.userId) filterInfo.push({ op: 0, propertyName: 'userId', value: formData.userId });
  if (formData.createTime && formData.createTime.length > 0) {
    filterInfo.push({ op: 3, propertyName: 'createTime', value: formData.createTime[0] });
    filterInfo.push({ op: 4, propertyName: 'createTime', value: `${formData.createTime[1]} 23:59:59` });
  }
  return filterInfo;
};
// 获取dialogForm元素节点
const mallOrderForm = ref<any>(null);
const mallOrderSendForm = ref<any>(null);
// 新建Modal
const handleClickShow = (row) => {
  mallOrderForm.value.show(row.id);
};
// 编辑Modal
const handleClickSend = (row) => {
  mallOrderSendForm.value.send(row.id);
};


// 点击下一页
const onPageChange = (pageInfo: PageInfo) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
  fetchData()
};
const getReceiveInfo = (value: string) => {
  var receiveInfo = ""
  if (value && value != 'null') {
    var jsonValue = JSON.parse(value)
    receiveInfo = `${jsonValue.receiverProvinceName} ${jsonValue.receiverCityName} ${jsonValue.receiverCountyName} ${jsonValue.receiverDetailInfo} ${jsonValue.receiverName} ${jsonValue.receiverMobile}`
  }
  return receiveInfo
}

const remoteMethod = async (search) => {
  if (search) {
    userList.value = []
    userList.value = await getRemoteSerch(search)
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
