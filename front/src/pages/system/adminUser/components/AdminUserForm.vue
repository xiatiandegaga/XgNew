<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="用户账号" name="account">
          <t-input v-model="formData.account" :style="{ width: '480px' }" placeholder="请输入用户账号" />
        </t-form-item>
        <t-form-item label="用户角色" name="userRoleIds">
          <t-select v-model="formData.userRoleIds" clearable :multiple="true" :style="{ width: '480px' }">
            <t-option v-for="item in roleList" :key="item.id" :value="item.id" :label="item.name">
              {{ item.name }}
            </t-option>
          </t-select>
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
import { ref, reactive, onMounted } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { findSingleById, addOrUpdate } from '@/api/system/adminUser';
import { getAllList } from '@/api/system/role';

const title = ref(); //dialog标题
const formVisible = ref(false); //控制是否展示dialog
let formData: any = ref({});
let roleList = reactive([])
const emit = defineEmits(['parentFetchData']);

onMounted(() => {
  role();
})

// 新建
const add = async () => {
  title.value = '新建用户信息';
  formVisible.value = true;
}
// 编辑
const update = async (id: string) => {
  title.value = '编辑用户信息';
  if (id) {
    formData.value = await findSingleById(id);
    formVisible.value = true;
  }
}
// 用户角色
const role = async () => {
  roleList = await getAllList()
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
  account: [{ required: true, message: '请输入用户账号', type: 'error' }],
};

defineExpose({
  add,
  update
})
</script>
