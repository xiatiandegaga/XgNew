<template>
  <div class="myFileList">
    <div class="upload-list-picture">

      <div class="upload-list-item upload-list-item-done" v-for="(item, index) in filesList" :key="index"
        style="width: 110px; height: 110px">
        <div class="upload-list-item-info">
          <t-image-viewer :key="item.url" v-model:visible="visible" :default-index="index" :images="images">
            <template #trigger>
              <div class="tdesign-demo-image-viewer__ui-image">
                <img alt="test" :src="item.url" class="tdesign-demo-image-viewer__ui-image--img" />
                <div class="tdesign-demo-image-viewer__ui-image--hover" @click="onOpen">
                  <span>
                    <BrowseIcon size="1.4em" /> 预览
                  </span>
                </div>
              </div>
            </template>
          </t-image-viewer>
          <!-- <img :src="item.url" style="width: 86px; height: 86px" /> -->
          <i aria-label="图标:delete" tabindex="-1" class="icon-close" @click="removeCoupon(index)"><svg
              viewBox="64 64 896 896" data-icon="delete" width="1.2em" height="1.2em" fill="currentColor">
              <path
                d="M563.8 512l262.5-312.9c4.4-5.2.7-13.1-6.1-13.1h-79.8c-4.7 0-9.2 2.1-12.3 5.7L511.6 449.8 295.1 191.7c-3-3.6-7.5-5.7-12.3-5.7H203c-6.8 0-10.5 7.9-6.1 13.1L459.4 512 196.9 824.9A7.95 7.95 0 0 0 203 838h79.8c4.7 0 9.2-2.1 12.3-5.7l216.5-258.1 216.5 258.1c3 3.6 7.5 5.7 12.3 5.7h79.8c6.8 0 10.5-7.9 6.1-13.1L563.8 512z">
              </path>
            </svg></i>
        </div>
      </div>
      <t-upload v-if="filesList.length < limit" listType="picture" multiple class="upload-list-inline" theme="image"
        accept="image/png, image/gif, image/jpg, image/jpeg" :files="[]" :locale="{
          triggerUploadText: {
            image: '请选择图片',
          },
        }" size="small" :before-upload="handleBeforeUpload">
      </t-upload>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref, toRefs, Ref } from 'vue'
import { MessagePlugin } from 'tdesign-vue-next';
import { getBase64 } from './index';
import { uploadFile } from '@/api/system/imgRule';
import { BrowseIcon } from 'tdesign-icons-vue-next';
const showFileUrl = import.meta.env.VITE_FILE_URL;

interface Props {
  limit?: number,
  modelValue: string | Array<[]>
}

const props = withDefaults(defineProps<Props>(), {
  // 允许上传图片数量
  limit: 10,
  modelValue: ''
})
const { limit, modelValue } = toRefs(props)
const maxSize: Ref<number> = ref(2)
let filesList = ref([]) // 所有的图片
let files = ref([])  // 新上传的图片
let images = ref([]) // 预览图片
const visible = ref(false);
const onOpen = () => (visible.value = true);

const init = () => {
  let count = 1
  let list = []
  filesList.value = []
  if (typeof modelValue.value === 'string') {
    list = modelValue.value.split(',')
  } else if (Array.isArray(modelValue.value)) {
    list = modelValue.value
  }

  if (list && list.length > 0) {
    /** */
    list.forEach((item, i) => {
      if (item) {
        filesList.value.push({
          index: i,
          uid: (count++).toString(),
          name: (count++).toString(),
          url: showFileUrl + item,
        })
      }
    })
    images.value = filesList.value.map(x => x.url)
  }
}
const handleBeforeUpload = (file) => {
  if (!checkImg(file)) return false
  if (!file.url) {
    getBase64(file).then((res) => {
      if (filesList.value.length + 1 > limit.value) {
        MessagePlugin.error(`图片数量最多只能上传${limit.value}个`);
        return false
      }
      file.url = res
      filesList.value = [...filesList.value, file]
      files.value = [...files.value, file]
      images.value = filesList.value.map(x => x.url)
    })
  } else {
    filesList.value = [...filesList.value, file]
    files.value = [...files.value, file]
  }
  return false
}
// 图片是否超过最大值
const checkImg = (file) => {
  const size = file.size / 1024 / 1024
  if (size > maxSize.value) {
    MessagePlugin.error(`图片大小必须小于${maxSize.value}M`);
    return false
  }
  return true
}
// 上传图片
const uploadImg = async () => {
  if (files.value && files.value.length > 0) {
    const formData = new FormData();
    for (let i = 0; i < files.value.length; i++) {
      formData.append(`file${i}`, files.value[i].raw);
    }
    const res = await uploadFile(formData)
    for (var i = 0; i < res.length; i++) {
      filesList.value.forEach((item) => {
        item.url =
          item.uid == files.value[i].uid && item.name == files.value[i].name ? showFileUrl + res[i] : item.url
      })
    }
  }
};
const getFilesUrl = (separator) => {
  separator = separator || ','
  const re = new RegExp(showFileUrl, 'g')
  let imgUrl = ''
  filesList.value.forEach((item) => {
    imgUrl += item.url.toString().replace(re, '') + separator
  })
  return !imgUrl ? imgUrl : imgUrl.substr(0, imgUrl.length - 1)
}
// 删除图片
const removeCoupon = (file) => {
  filesList.value.forEach((val, key) => {
    if (file === key) {
      filesList.value.splice(key, 1)
    }
  })
};
defineExpose({ init, uploadImg, getFilesUrl })
</script>
<style lang="less" scoped>
.upload-list-item {
  margin-right: 10px;
}

.upload-list-item-info {
  width: 108px;
  height: 108px;
  border-radius: 4px;
  border: 1px solid #d9d9d9;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
}

.upload-list-picture {
  display: flex;
  flex-wrap: wrap;
  height: 100%;
  align-items: center;
}

.upload-list-picture .icon-close {
  position: absolute;
  top: -7px;
  right: -6px;
  line-height: 1;
  opacity: 1;
}

.tdesign-demo-image-viewer__ui-image {
  width: 100%;
  height: 100%;
  display: inline-flex;
  position: relative;
  justify-content: center;
  align-items: center;
  border-radius: var(--td-radius-small);
  overflow: hidden;
}

.tdesign-demo-image-viewer__ui-image--hover {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  position: absolute;
  left: 0;
  top: 0;
  opacity: 0;
  background-color: rgba(0, 0, 0, 0.6);
  color: var(--td-text-color-anti);
  line-height: 22px;
  transition: 0.2s;
}

.tdesign-demo-image-viewer__ui-image:hover .tdesign-demo-image-viewer__ui-image--hover {
  opacity: 1;
  cursor: pointer;
}

.tdesign-demo-image-viewer__ui-image--img {
  width: auto;
  height: auto;
  max-width: 100%;
  max-height: 100%;
  cursor: pointer;
  position: absolute;
}

.tdesign-demo-image-viewer__ui-image--footer {
  padding: 0 16px;
  height: 56px;
  width: 100%;
  line-height: 56px;
  font-size: 16px;
  position: absolute;
  bottom: 0;
  color: var(--td-text-color-anti);
  background-image: linear-gradient(0deg, rgba(0, 0, 0, 0.4) 0%, rgba(0, 0, 0, 0) 100%);
  display: flex;
  box-sizing: border-box;
}

.tdesign-demo-image-viewer__ui-image--title {
  flex: 1;
}

.tdesign-demo-popup__reference {
  margin-left: 16px;
}

.tdesign-demo-image-viewer__ui-image--icons .tdesign-demo-icon {
  cursor: pointer;
}

.tdesign-demo-image-viewer__base {
  width: 160px;
  height: 160px;
  margin: 10px;
  border: 4px solid var(--td-bg-color-secondarycontainer);
  border-radius: var(--td-radius-medium);
}
</style>