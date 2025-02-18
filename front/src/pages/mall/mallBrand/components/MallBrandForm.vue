<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="品牌名称" name="brandName">
          <t-input v-model="formData.brandName" :style="{ width: '480px' }" placeholder="请输入品牌名称" />
        </t-form-item>
        <t-form-item label="品牌编号" name="brandCode">
          <t-input v-model="formData.brandCode" :style="{ width: '480px' }" placeholder="请输入品牌编号" />
        </t-form-item>
        <t-form-item label="品牌logo" name="brandLogo">
          <upload-img v-model="formData.brandLogo" :limit="1" ref="brandLogo" style="width: 100%"></upload-img>
        </t-form-item>
        <t-form-item label="品牌详情图" name="brandImg">
          <upload-img v-model="formData.brandImg" :limit="6" ref="brandImg" style="width: 100%"></upload-img>
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
import UploadImg from '@/components/upload/uploadImg.vue';
import { findSingleById, addOrUpdate } from '@/api/mall/mallBrand';

const title = ref(); //dialog标题
const formVisible = ref(false); //控制是否展示dialog
let formData: any = ref({});
const emit = defineEmits(['parentFetchData']);

// 新建
const add = async () => {
  title.value = '新建品牌信息';
  formVisible.value = true;
}
// 编辑
const update = async (id: string) => {
  title.value = '编辑品牌信息';
  if (id) {
    formData.value = await findSingleById(id);
    formVisible.value = true;
    setTimeout(() => {
      if (formData.value.brandLogo) brandLogo.value.init()
      if (formData.value.brandImg) brandImg.value.init()
    })
  }
}
// 图片ref
const brandLogo = ref<any>(null)
const brandImg = ref<any>(null)

// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (brandLogo) {
    await brandLogo.value.uploadImg()
    formData.value.brandLogo = brandLogo.value.getFilesUrl()
  }
  if (brandImg) {
    await brandImg.value.uploadImg()
    formData.value.brandImg = brandImg.value.getFilesUrl()
  }
  if (!formData.value.brandLogo) {
    MessagePlugin.warning('品牌Logo不能为空！');
    return;
  }
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
  brandName: [{ required: true, message: '请输入品牌名称', type: 'error' }],
};

defineExpose({
  add,
  update
})
</script>
