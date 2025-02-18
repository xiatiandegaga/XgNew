<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="商品属性" name="attrKeyValueName">
          <t-input v-model="formData.attrKeyValueName" :style="{ width: '480px' }" :disabled="true"/>
        </t-form-item>
         <t-form-item label="当前库存" name="currentNum">
          <t-input v-model="formData.currentNum" :style="{ width: '480px' }" :disabled="true"/>
        </t-form-item>
        <t-form-item :label="typeName" name="num">
          <t-input v-model="formData.num" :style="{ width: '480px' }" placeholder="请输入数量" />
        </t-form-item>
        <t-form-item style="float: right">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit">确定</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { reactive, ref, onMounted,defineComponent } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { findSingleById, stockRel,stockInv} from '@/api/mall/mallProduct';

const title = ref(); // dialog标题
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});

const emit = defineEmits(['parentFetchData']);
let typeName=ref('入库数量')
let type=ref(1)
onMounted(() => {
 
});

// 入库
const inv = async (data) => {
  title.value = '库存入库';
  formVisible.value = true;
  typeName.value='入库数量'
  type.value=1
  formData.value=data
};
const rel = async (data) => {
  title.value = '库存出库';
  formVisible.value = true;
  typeName.value='出库数量'
  type.value=2
  formData.value=data
};


// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (!firstError) {
    if(type.value==1)
    {
        await stockInv(formData.value);
    }
    else if(type.value==2)
    {
         await stockRel(formData.value);
    }
    
    MessagePlugin.success('操作成功');
    formVisible.value = false

    emit('parentFetchData',{skuId:formData.value.skuId,type:type.value,num:formData.value.num});
  } else {
    MessagePlugin.warning(firstError);
  }
};

// 取消
const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};
// 验证规则
const rules: Record<string, FormRule[]> = {
  num: [{ required: true, message: '请输入出入库数量', type: 'error' }],

};

defineExpose({
  inv,
  rel,
});
</script>
