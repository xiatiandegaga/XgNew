<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="width" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" layout="inline" :label-width="100" @submit="onSubmit">
        <t-form-item label="编号" name="imgRuleCode">
          <t-input v-model="formData.imgRuleCode" :style="{ width: '480px' }" placeholder="请输入编号" />
        </t-form-item>
        <t-form-item label="名称" name="imgRuleName">
          <t-input v-model="formData.imgRuleName" :style="{ width: '480px' }" placeholder="请输入名称" />
        </t-form-item>
        <t-form-item label="排序" name="sortNo">
          <t-input-number v-model="formData.sortNo" theme="column" :style="{ width: '480px' }" placeholder="请输入排序" />
        </t-form-item>
        <t-form-item label="说明" name="remark">
          <t-textarea v-model="formData.remark" placeholder="请输入说明" />
        </t-form-item>
        <!-- <t-form-item label="说明" name="remark">
          <ul>
            <li v-for="item in list" :key="item.id">
              <upload-img :imgValue="item.id" :limit="1" :ref="setRefs(item.id)" style="width: 100%"></upload-img>
            </li>
          </ul>
        </t-form-item> -->
        <t-row style="margin:0 auto">
          <t-col style="max-width: 1400px;">
            <t-space size="24px">
              <t-button shape="rectangle" variant="outline" @click="toAdd()">新增</t-button>
              <t-button shape="rectangle" variant="outline" @click="batchDelete()">删除</t-button>
            </t-space>
            <t-table :columns="COLUMNS_FORM" :data="formData.imgRuleDetailDtos" row-key="id" table-layout="fixed"
              :horizontalScrollAffixedBottom="true" @select-change="onSelectChange" :selected-row-keys="selectedRowKeys"
              :table-content-width="'2000px'">
              <template #mainImg="{ row }">
                <upload-img v-model="row.mainImg" :limit="1" :ref="setRefs(row.id)"
                  style="width: 100%;height:117px"></upload-img>
              </template>
              <template #linkType="{ row }">
                <t-select v-model="row.linkType" :clearable="true" style="width: 100%" placeholder="请选择">
                  <t-option v-for="item in linkTypeList" :key="item.constKey" :value="item.constKey"
                    :label="item.name">{{
                      item.name
                    }}</t-option>
                </t-select>
              </template>
              <template #linkKey="{ row }">
                <t-select v-model="row.linkKey" :clearable="true" style="width: 100%" placeholder="请选择"
                  v-if="row.linkType == '4' || row.linkType == '5'">
                </t-select>
              </template>
              <template #linkAddress="{ row }">
                <t-input v-model="row.linkAddress" style="width: 100%" />
              </template>
              <template #detailImage="{ row }">
                <t-button href="#" v-if="row.linkType == '1' || row.linkType == '6'" style="width: 100%">链接图片</t-button>
              </template>
              <template #startTime="{ row }">
                <t-date-picker v-model="row.startTime" enable-time-picker clearable style="width: 100%" />
              </template>
              <template #endTime="{ row }">
                <t-date-picker v-model="row.endTime" enable-time-picker clearable style="width: 100%" />
              </template>
              <template #sortNo="{ row }">
                <t-input-number theme="column" v-model="row.sortNo" style="width: 100px" />
              </template>
              <template #remark="{ row }">
                <t-input v-model="row.remark" style="width: 100%" />
              </template>
            </t-table>
          </t-col>

        </t-row>
        <t-row style="width:100%">
          <t-col style="display: flex;justify-content: flex-end;">
            <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
            <t-button theme="primary" type="submit">确定</t-button>
          </t-col>
        </t-row>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { ref, reactive, Ref } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { COLUMNS_FORM } from '../index.d';
import UploadImg from '@/components/upload/uploadImg.vue';
import { findSingleById, addOrUpdate } from '@/api/system/imgRule';
import { getAllList } from '@/api/system/globalDataDetail';
import { Params } from '@/api/model/common';
import { ImgRuleDetail } from '@/api/model/system/imgRuleModel'
import dayjs from 'dayjs';

const width = ref('80%')
const title = ref(); //dialog标题
const formVisible = ref(false); //控制是否展示dialog
let formData: any = ref({});
let linkTypeList = reactive([])
let count: Ref<number> = ref(0)
const selectedRowKeys = ref([]);
const emit = defineEmits(['parentFetchData']);

// 新建
const add = async () => {
  title.value = '新建自定义图片';
  formData.value = {}
  formData.value.imgRuleDetailDtos = []
  formVisible.value = true;
  init();
}
// 编辑
const update = async (id: string) => {
  title.value = '编辑自定义图片';
  init();
  if (id) {
    const result = await findSingleById(id);
    formData.value = result
    formData.value.imgRuleDetailDtos = result.imgRuleDetailDtos
    formVisible.value = true;
    if (formData.value.imgRuleDetailDtos && formData.value.imgRuleDetailDtos.length > 0) {
      setTimeout(() => {
        formData.value.imgRuleDetailDtos.forEach((item) => {
          uploadMainImgs.value[`uploadMainImg${item.id}`].init()
        })
      })
    }
  }
}
const init = () => {
  getLinkType();
}
// 链接类型
const getLinkType = async () => {
  const params: Params = {
    filter: [{ op: 0, propertyName: 'code', value: 'LinkType' }]
  }
  linkTypeList = await getAllList(params)
}
// 新增
const toAdd = () => {
  let nowTime = new Date()
  let afterTime = new Date().setFullYear(new Date().getFullYear() + 100)
  const currentTime = dayjs(nowTime).format('YYYY-MM-DD HH:mm:ss')
  const afterDate = dayjs(afterTime).format('YYYY-MM-DD HH:mm:ss')
  if (!formData.value.imgRuleDetailDtos) formData.value.imgRuleDetailDtos = []

  formData.value.imgRuleDetailDtos.push({ id: --count.value, startTime: currentTime, endTime: afterDate, })
  formData.value.imgRuleDetailDtos = [...formData.value.imgRuleDetailDtos]
}
// 删除
const batchDelete = () => {
  if (selectedRowKeys.value.length > 0) {
    let imgRuleDetailDtos = formData.value.imgRuleDetailDtos.filter(
      (item: ImgRuleDetail) => selectedRowKeys.value.indexOf(item.id) === -1
    )
    formData.value.imgRuleDetailDtos = [...imgRuleDetailDtos]
  }
}
// 选择
const onSelectChange = (value: Array<number>,) => {
  selectedRowKeys.value = value;
  console.log(value);
};
// 图片ref
const uploadMainImgs = ref([])

const setRefs = (index: string) => (el: HTMLElement) => {
  uploadMainImgs.value[`uploadMainImg${index}`] = el;
};


// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (formData.value.imgRuleDetailDtos && formData.value.imgRuleDetailDtos.length > 0) {
    let count = 1
    for (let item of formData.value.imgRuleDetailDtos) {
      // 获取dialogForm元素节点
      if (uploadMainImgs.value[`uploadMainImg${item.id}`]) {
        await uploadMainImgs.value[`uploadMainImg${item.id}`].uploadImg()
        item.mainImg = uploadMainImgs.value[`uploadMainImg${item.id}`].getFilesUrl()
        // item.mainImg = imgs.join(',')
      }
      if (!item.mainImg && item.linkType != 7) {
        MessagePlugin.warning(`第${count}行，主图不能为空！`);
        return
      }
      if ((item.linkType == 4 || item.linkType == 5) && !item.linkKey) {
        MessagePlugin.warning(`第${count}行，链接对象不能为空！`);
        return
      }

      if (item.linkType == 1 && !item.detailImage) {
        MessagePlugin.warning(`第${count}行，链接图片不能为空！`);
        return
      }
      if (!item.sortNo) {
        MessagePlugin.warning(`第${count}行，排序不能为空！`);
        return
      }
      count++
    }
  }
  console.log(formData.value)
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
  imgRuleCode: [{ required: true, message: '请输入编号', type: 'error' }],
  imgRuleName: [{ required: true, message: '请输入名称', type: 'error' }],
};

defineExpose({
  add,
  update
})
</script>
<style lang="less" scoped>
.t-textarea {
  width: 480px !important;
}
</style>
