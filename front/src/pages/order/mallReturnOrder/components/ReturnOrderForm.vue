<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="width" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" layout="inline" :rules="rules" :label-width="160" @submit="onSubmit">
        <t-form-item label="订单号" name="orderNo">
          <t-input v-model="formData.orderNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="关联订单号" name="refOrderNo">
          <t-input v-model="formData.refOrderNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="用户名称" name="userName">
          <t-input v-model="formData.userName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="用户手机" name="userMobile">
          <t-input v-model="formData.userMobile" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="产品名称" name="productName">
          <t-input v-model="formData.productName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="产品属性" name="productSkuAttrs">
          <t-input :default-value="getAttrKeyName(formData.productSkuAttrs)" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="产品数量" name="productQuantity">
          <t-input :default-value="formData.productQuantity" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="单价金额（元）" name="productAmount">
          <t-input v-model="formData.productAmount" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="总金额（元）" name="totalProductAmount">
          <t-input v-model="formData.totalProductAmount" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="订单状态" name="status">
          <t-input :default-value="getStatusName(formData.status)" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="详细状态" name="detailStatusName">
          <t-input v-model="formData.detailStatusName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="申请时间" name="createDate">
          <t-input v-model="formData.createDate" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="支付时间" name="paymentTime">
          <t-input v-model="formData.paymentTime" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="申请原因">
          <t-select v-model="formData.applicationReason" :style="{ width: inputWidth }" placeholder="请选择">
            <t-option v-for="item in applicationList" :key="item.constKey" :value="item.constKey" :label="item.name">
              {{ item.name }}
            </t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="申请说明" name="applicationDescription">
          <t-textarea v-model="formData.applicationDescription" :autosize="{ minRows: 2, maxRows: 4 }"
            :style="{ width: maxWidth }" />
        </t-form-item>
        <t-form-item label="申请图片" name="applicationImgs">
          <upload-img ref="applicationImg" v-model="formData.applicationImgs" :limit="imgNum"
            :style="{ width: maxWidth }"></upload-img>
        </t-form-item>
        <t-form-item label="退货物流单号" name="returnLogisticsNo" v-if="formData.returnLogisticsNo">
          <t-input v-model="formData.returnLogisticsNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="退货物流单号" name="returnLogisticsNo" v-if="formData.returnLogisticsNo">
          <t-input v-model="formData.returnLogisticsNo" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="审核人" name="checkUserName" v-if='!isEdit'>
          <t-input v-model="formData.checkUserName" :style="{ width: inputWidth }" />
        </t-form-item>
        <t-form-item label="审核时间" name="checkDate" v-if='!isEdit'>
          <t-input v-model="formData.checkDate" :style="{ width: inputWidth }" />
        </t-form-item>

        <t-form-item label="备注" name="remark">
          <t-textarea v-model="formData.remark" name="description" :autosize="{ minRows: 3, maxRows: 5 }"
            :style="{ width: maxWidth }" />
        </t-form-item>
        <t-form-item label="审核结果" name="checkStatus">
          <t-radio-group v-model="formData.checkStatus" @change="onChange">
            <t-radio value="1">通过</t-radio>
            <t-radio value="2">拒绝</t-radio>
          </t-radio-group>

        </t-form-item>
        <t-form-item label="审核意见" name="responseResult">
          <t-textarea v-model="formData.responseResult" :autosize="{ minRows: 3, maxRows: 5 }"
            :style="{ width: maxWidth }" />

        </t-form-item>
        <t-form-item style="float: right; margin-top: 20px">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit" v-if="isEdit">确定</t-button>
        </t-form-item>
      </t-form>
      <!-- <div style="float: right; margin-top: 20px">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary"  type="submit" v-if="isEdit">确定</t-button>
        </div> -->
    </template>
  </t-dialog>

  <t-dialog v-model:visible="confirmVisible" header="审核" :body="confirmBody" :on-cancel="onCancel"
    @confirm="onConfirmDelete" />
</template>

<script setup lang="ts">
const width = ref('70%');
const inputWidth = ref('400px');
const maxWidth = ref('985px');
import { ref, reactive, onMounted, Ref } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data, FormProps } from 'tdesign-vue-next';
import { findSingleById, checkOrder } from '@/api/mall/mallReturnOrder';
import { GlobalDataCode, OptionSelect } from '@/utils/commonConst';
import { getListByCodes } from '@/api/system/globalDataDetail';
import UploadImg from '@/components/upload/uploadImg.vue';

const title = ref(); // dialog标题
let imgNum = ref(0)
const formVisible = ref(false); // 控制是否展示dialog
let confirmVisible = ref(false)
let confirmBody = ref('确定要审核通过吗？')
const formData: any = ref({
  checkStatus: 1
});
let applicationList = ref([])

let isEdit = ref(false)
//let checkStatus=ref(1)
const emit = defineEmits(['parentFetchData']);


const initData = async () => {
  applicationList.value = await getListByCodes(GlobalDataCode.OrderReturnApplicationReason)
}
onMounted(() => {
  initData();
});

// 图片ref
const applicationImg = ref<any>(null);
// 编辑
const show = async (id: string) => {
  title.value = '查看';
  isEdit.value = false
  if (id) {
    const res = await findSingleById(id);
    formData.value = res;
    formVisible.value = true;
    setTimeout(() => {
      if (formData.value.applicationImgs) {
        imgNum.value = formData.value.applicationImgs.split(',').length
        applicationImg.value.init();
      }
    });
  }
};
const check = async (id: string) => {
  title.value = '审核';
  isEdit.value = true
  if (id) {
    const res = await findSingleById(id);
    formData.value = res;
    formVisible.value = true;
    setTimeout(() => {
      if (formData.value.applicationImgs) {
        imgNum.value = formData.value.applicationImgs.split(',').length
        applicationImg.value.init();
      }
    });
  }
};
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (!firstError) {
    confirmVisible.value = true
  } else {
    MessagePlugin.warning(firstError);
  }

}

// 取消
const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};
// 验证规则
let rules = ref<Record<string, FormRule[]>>({
  checkStatus: [{ required: true, message: '请选择审核结果', type: 'error' }],
  responseResult: []
});

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

const getStatusName = (value) => {
  if (value || value === 0) {
    var item = OptionSelect.ReturnOrderStatusList.filter(x => x.value == value)
    if (item && item[0]) return item[0].label
  }
  return ''
}

const onCancel = () => {
  confirmVisible.value = false
}

const onConfirmDelete = async () => {
  var data: any = {}
  data.id = formData.value.id
  data.message = formData.value.responseResult
  data.status = formData.value.checkStatus
  await checkOrder(data)
  MessagePlugin.success(`审核成功！`);
  formVisible.value = false;
  confirmVisible.value = false
  emit('parentFetchData');
}
const onChange = (value) => {
  //checkStatus.value=value
  console.log(formData.value.checkStatus)
  if (value == 1) {
    confirmBody.value = '确定要审核通过吗？'
    rules.value.responseResult = []
  }
  else {
    confirmBody.value = '确定要审核拒绝吗？'
    rules.value.responseResult = [{ required: true, message: '请填写审核意见', type: 'error' }]
  }

}
defineExpose({
  show,
  check
});
</script>
