<template>
 <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
  :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
  <template #body>
   <!-- 表单内容 -->
   <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
    <t-form-item label="编码" name="code">
     <t-input :disabled="true" v-model="formData.code" :style="{ width: '480px' }" placeholder="请输入编码" />
    </t-form-item>
    <t-form-item label="key值" name="constKey">
     <t-input v-model="formData.constKey" :style="{ width: '480px' }" placeholder="请输入key值" />
    </t-form-item>
    <t-form-item label="名称" name="name">
     <t-input v-model="formData.name" :style="{ width: '480px' }" placeholder="请输入名称" />
    </t-form-item>
    <t-form-item label="排序" name="sortNo">
     <t-input-number v-model="formData.sortNo" theme="column" :style="{ width: '480px' }" placeholder="请输入排序" />
    </t-form-item>
    <t-form-item label="备注" name="remark">
     <t-textarea v-model="formData.remark" :style="{ width: '480px' }" placeholder="请输入备注" />
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
import { ref, } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { findSingleById, addOrUpdate } from '@/api/system/globalDataDetail';

const title = ref(); //dialog标题
const formVisible = ref(false); //控制是否展示dialog
let formData: any = ref({});
const emit = defineEmits(['parentFetchData']);

// 新建
const add = async (code: string) => {
 title.value = '新建数据字典信息';
 formVisible.value = true;
 formData.value.code = code
}

// 编辑
const update = async (id: string) => {
 title.value = '编辑数据字典信息';
 if (id) {
  formData.value = await findSingleById(id);
  formVisible.value = true;
 }
}
// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
 if (!firstError) {
  await addOrUpdate(formData.value);
  MessagePlugin.success('操作成功');
  formVisible.value = false;
  emit('parentFetchData');
 } else {
  MessagePlugin.warning(firstError);
 }
};

// 取消
const onClickCloseBtn = () => {
 formData.value = {};
 formVisible.value = false;
};
//验证规则
const rules: Record<string, FormRule[]> = {
 code: [{ required: true, message: '请输入编码', type: 'error' }],
 name: [{ required: true, message: '请输入名称', type: 'error' }],
};

defineExpose({
 add,
 update
})
</script>
