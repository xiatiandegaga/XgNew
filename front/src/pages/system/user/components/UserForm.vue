<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="用户账号" name="account">
          <t-input :disabled="true" v-model="formData.account" :style="{ width: '480px' }" placeholder="请输入用户账号" />
        </t-form-item>
        <t-form-item label="用户真实名" name="realName">
          <t-input v-model="formData.realName" :style="{ width: '480px' }" placeholder="请输入用户名" />
        </t-form-item>
        <t-form-item label="用户昵称" name="nickName">
          <t-input v-model="formData.nickName" :style="{ width: '480px' }" placeholder="请输入用户名" />
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
import { findSingleById, update } from '@/api/system/user';

const title = ref(); //dialog标题
const formVisible = ref(false); //控制是否展示dialog
let formData: any = ref({});
const emit = defineEmits(['parentFetchData']);

// 编辑
const updatePage = async (id: string) => {
  title.value = '编辑用户信息';
  if (id) {
    formData.value = await findSingleById(id);
    formVisible.value = true;
  }
}
// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (!firstError) {
    await update(formData.value);
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
  account: [{ required: true, message: '请输入用户账号', type: 'error' }],
  name: [{ required: true, message: '请输入用户名', type: 'error' }],
};

defineExpose({
  updatePage
})
</script>
