<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="width" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" layout="inline" :label-width="160">
        <t-form-item label="订单号" name="orderNo">
          <t-input v-model="formData.orderNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="用户名称" name="userName">
          <t-input v-model="formData.userName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="用户手机" name="userMobile">
          <t-input v-model="formData.userMobile" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="支付总金额（元）" name="payAmount">
          <t-input v-model="formData.payAmount" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="订单总金额（元）" name="totalAmount">
          <t-input v-model="formData.totalAmount" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="下单时间" name="createTime">
          <t-input v-model="formData.createTime" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="支付时间" name="paymentTime">
          <t-input v-model="formData.paymentTime" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="发货时间" name="deliveryTime">
          <t-input v-model="formData.deliveryTime" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="签收时间" name="receiveTime">
          <t-input v-model="formData.receiveTime" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="物流公司" name="logisticsCompanyName">
          <t-input v-model="formData.logisticsCompanyName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="物流单号" name="logisticsNo">
          <t-input v-model="formData.logisticsNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="分期" name="numberOfInstallments">
          <t-input v-model="formData.numberOfInstallments" :style="{ width: inputWidth }" suffix="期" />
        </t-form-item>
        <t-form-item label="订单状态" name="statusName">
          <t-input v-model="formData.statusName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="收获地址" name="receiveInfo">
          <t-input :default-value="getReceiveInfo(formData.receiveInfo)" :style="{ width: maxWidth }" />
        </t-form-item>
        <t-form-item label="备注" name="receiveInfo">
          <t-input v-model="formData.remark" :style="{ width: maxWidth }" />
        </t-form-item>
        <t-row style="margin: 0 auto">
          <t-col>
            <t-table :columns="COLUMNS" :data="formData.mallOrderDetails" row-key="id" table-layout="fixed"
              :horizontal-scroll-affixed-bottom="true">
              <template #productSkuAttrs="{ row }">
                <span>{{ getAttrKeyName(row.productSkuAttrs) }}</span>
              </template>
            </t-table>
          </t-col>
        </t-row>

      </t-form>
      <div style="float: right; margin-top: 20px">
        <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
        <!-- <t-button theme="primary" type="submit" v-if="isEdit">确定</t-button> -->
      </div>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
const width = ref('70%');
const inputWidth = ref('400px');
const maxWidth = ref('985px');
import { ref, reactive, onMounted, Ref } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { COLUMNS } from './orderForm.d';
import { findSingleById } from '@/api/mall/mallOrder';


const title = ref(); // dialog标题

const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});

let isEdit = ref(false)
onMounted(() => {


});


// 编辑
const show = async (id: string) => {
  title.value = '查看';
  if (id) {
    const res = await findSingleById(id);
    formData.value = res;
    formVisible.value = true;
  }
};


// 取消
const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};
// 验证规则
const rules: Record<string, FormRule[]> = {
  attrKeyName: [{ required: true, message: '请输入属性名称', type: 'error' }],
  mallProductTypeId: [{ required: true, message: '请选择商品类型', type: 'error' }],
  mallProductCategoryId: [{ required: true, message: '请选择商品目录', type: 'error' }],
};

const getReceiveInfo = (value: string) => {
  var receiveInfo = ""
  if (value && value != 'null') {
    var jsonValue = JSON.parse(value)
    receiveInfo = `${jsonValue.receiverProvinceName} ${jsonValue.receiverCityName} ${jsonValue.receiverCountyName} ${jsonValue.receiverDetailInfo} ${jsonValue.receiverName} ${jsonValue.receiverMobile}`
  }
  return receiveInfo
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
defineExpose({
  show
});
</script>
