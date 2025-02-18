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
                  <t-option v-for="item in OptionSelect.ReturnOrderStatusList" :key="item.value" :value="item.value"
                    :label="item.label"></t-option>
                </t-select>
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="详细状态" name="status">
                <t-select v-model="formData.detailStatus" clearable>
                  <t-option v-for="item in detailStatusList" :key="item.constKey" :value="item.constKey"
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
        <template #productSkuAttrs="{ row }">
          <span>{{ getAttrKeyName(row.productSkuAttrs) }}</span>
        </template>
        <template #status="{ row }">
          <span>{{ getStatusName(row.status) }}</span>
        </template>
        <template #receiveInfo="{ row }">
          <span>{{ getReceiveInfo(row.receiveInfo) }}</span>
        </template>
        <template #op="{ row }">
          <a class="t-button-link" @click="handleClickShow(row)">查看</a>
          <a v-auth="'mallReturnOrder:check'" v-if='row.status == 0' class="t-button-link"
            @click="handleClickCheck(row)">审核</a>
          <!-- <t-popconfirm content="确定要退货入库吗" @confirm="handleReturnInv(row)">
            <a v-auth="'mallReturnOrder:returnInv'" v-if="row.status==1&&row.detailStatus==8" class="t-button-link">退货入库</a>
          </t-popconfirm> -->
          <t-popconfirm :content="`确定要退款吗? 退款金额是￥${row.productAmount}`" theme="warning" @confirm="handleRefund(row)"
            v-if="row.status == 1 && row.detailStatus == 9 || row.status == 1 && row.detailStatus == 13">
            <a v-auth="'mallReturnOrder:refund'" class="t-button-link">退款</a>
          </t-popconfirm>
          <t-popconfirm content="确定完成退货吗" @confirm="handleReturned(row)">
            <a v-auth="'mallReturnOrder:refund'" v-if="row.status == 1 && row.detailStatus == 8"
              class="t-button-link">退货完成</a>
          </t-popconfirm>
        </template>
      </t-table>
    </div>
    <ReturnOrderForm ref="returnOrderForm" @parent-fetch-data="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getReturnOrderPageList, orderReturnInv, orderRefund, orderReturned } from '@/api/mall/mallReturnOrder';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import type { Params, ParamsFilter } from '@/api/model/common';
import ReturnOrderForm from './components/ReturnOrderForm.vue'
import { getListByCodes } from '@/api/system/globalDataDetail';
import { GlobalDataCode, OptionSelect } from '@/utils/commonConst';
import { getRemoteSerch } from '@/api/system/user';
let data = reactive([]); // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开
let detailStatusList = ref([])
let userList = ref([])
onMounted(() => {
  fetchData();
  initData();
});
const initData = async () => {
  detailStatusList.value = await getListByCodes(GlobalDataCode.MallOrderDetailStatus)
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
    const { list, totalCount } = await getReturnOrderPageList(params);
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
  if (formData.detailStatus || formData.detailStatus === 0) filterInfo.push({ op: 0, propertyName: 'detailStatus', value: formData.detailStatus });
  if (formData.userId) filterInfo.push({ op: 0, propertyName: 'userId', value: formData.userId });
  if (formData.createTime && formData.createTime.length > 0) {
    filterInfo.push({ op: 3, propertyName: 'createDate', value: formData.createTime[0] });
    filterInfo.push({ op: 4, propertyName: 'createDate', value: `${formData.createTime[1]} 23:59:59` });
  }
  return filterInfo;
};
// 获取dialogForm元素节点
const returnOrderForm = ref<any>(null);

// 新建Modal
const handleClickShow = (row) => {
  returnOrderForm.value.show(row.id);
};
const handleClickCheck = (row) => {
  returnOrderForm.value.check(row.id);
}

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
const getStatusName = (value) => {
  if (value || value === 0) {
    var item = OptionSelect.ReturnOrderStatusList.filter(x => x.value == value)
    if (item && item[0]) return item[0].label
  }
  return ''
}
const getAttrKeyName = (value) => {
  let name = ''
  if (value) {
    JSON.parse(value).forEach((item) => {
      name += `${item.attrKeyName}:${item.attrValueName},`
    })
  }
  return !name ? '' : name.substr(0, name.length - 1)
}
const remoteMethod = async (search) => {
  if (search) {
    userList.value = []
    userList.value = await getRemoteSerch(search)
  }
}
const handleReturnInv = async (row) => {
  await orderReturnInv(row.id)
  MessagePlugin.success("退货入库成功！")
  fetchData()
}
const handleRefund = async (row) => {
  await orderRefund(row.id)
  MessagePlugin.success("退款成功！")
  fetchData()
}
const handleReturned = async (row) => {
  await orderReturned(row.mallOrderDetailId)
  MessagePlugin.success("退货成功！")
  fetchData()
}
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
