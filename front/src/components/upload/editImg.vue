<template>
  <t-dialog v-model:visible="formVisible" :width="'50%'" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="handleCancel" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="vmData">
        <t-form-item label="详细图">
          <upload-img ref="uploadImg" :limit="limit" v-model="vmData.fileUrl"></upload-img>
        </t-form-item>
        <t-form-item style="float: right">
          <t-button variant="outline" @click="handleCancel">取消</t-button>
          <t-button theme="primary" type="submit" :loading="confirmLoading" @click="saveUploadImgs"
            v-if="showSubmit">立即上传</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { ref, toRefs, Ref, nextTick } from 'vue'
import UploadImg from '@/components/upload/uploadImg.vue';

const emit = defineEmits(['parentFetchData']);

const vmData: any = ref({})
const formVisible = ref(false)
const title = ref('上传图片'); // dialog标题
const confirmLoading = ref(false)
const showSubmit = ref(true)
const key = ref(0)
const limit = ref(10)
const uploadImg = ref<any>(null)
const edit = (keyId, fileUrl, limitNum, isShowSubmit = true) => {
  formVisible.value = true
  vmData.value.fileUrl = ''
  vmData.value.fileUrl = fileUrl
  key.value = keyId
  showSubmit.value = isShowSubmit
  if (limitNum) limit.value = limitNum
  vmData.value = { ...vmData.value }
  nextTick(() => {
    uploadImg.value.init()
  })
}

const handleCancel = () => {
  formVisible.value = false
}
const saveUploadImgs = async () => {
  await uploadImg.value.uploadImg().catch((err) => {
    return
  })

  let data = {
    key: key.value,
    fileUrl: ''
  }
  data.fileUrl = uploadImg.value.getFilesUrl()
  emit('parentFetchData', data);
  formVisible.value = false
}
defineExpose({ edit })
</script>