<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="width" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" layout="inline" :rules="rules" :label-width="160" @submit="onSubmit">
        <t-form-item label="订单号" name="orderNo">
          <t-input v-model="formData.orderNo" :style="{ width: inputWidth }" :disabled="true" />
        </t-form-item>
        <t-form-item label="收获地址" name="receiveInfo">
          <t-input :default-value="getReceiveInfo(formData.receiveInfo)" :style="{ width: inputWidth }"
            :disabled="true" />
        </t-form-item>
        <t-form-item label="物流公司" name="logisticsCompany">
          <t-select v-model="formData.logisticsCompany" :clearable="true" :style="{ width: inputWidth }"
            placeholder="请选择">
            <t-option v-for="item in logisticsCompanyList" :key="item.constKey" :value="item.constKey"
              :label="item.name"></t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="物流单号" name="logisticsNo">
          <t-input v-model="formData.logisticsNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item style="float: right; margin-top: 20px">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit">确定</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
const width = ref('80%');
const inputWidth = ref('400px');
const maxWidth = ref('1000px');
import { ref, reactive, onMounted, Ref } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { COLUMNS } from './orderForm.d';
import { findSingleById, sendOut } from '@/api/mall/mallOrder';
import { getListByCodes } from '@/api/system/globalDataDetail';
import { GlobalDataCode } from '@/utils/commonConst';
const title = ref(); // dialog标题

const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});
let logisticsCompanyList = reactive([]); // 商品类型列表
const emit = defineEmits(['parentFetchData']);
let isEdit = ref(false)
onMounted(() => {
  getLogisticsCompanyList()
});

// 商品类型
const getLogisticsCompanyList = async () => {
  logisticsCompanyList = await getListByCodes(GlobalDataCode.LogisticsType);
};
// 编辑
const send = async (id: string) => {
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

};
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (!firstError) {
    await sendOut(formData.value);
    MessagePlugin.success('操作成功');
    formVisible.value = false;
    emit('parentFetchData');
  } else {
    MessagePlugin.warning(firstError);
  }
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
  send
});
</script>
