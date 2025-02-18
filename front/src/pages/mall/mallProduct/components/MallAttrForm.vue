<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="'50%'" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :label-width="100" @submit="onSubmit">
        <template v-for="(item, index) in formData.attrKeyList" :key="item.id">
            <t-form-item :label="item.mallProductAttrKeyDto.attrKeyName">
              <t-radio-group v-model="item.checkId" :options="getAttrValuesOption(item)" name="city"></t-radio-group>
            </t-form-item>
          </template>
        <t-form-item style="float: right">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit">确定</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref, Ref } from 'vue';
import { COLUMNS_FORM } from '..';

const title = ref(); // dialog标题
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({id:0,attrKeyList:[] });

const attrList = ref([]); // 商品属性列表
const temp: any = ref([]);
const emit = defineEmits(['parentFetchData']);



// 编辑
const update = (id,attrKeyList) => {

  formData.value.id=id
  formData.value.attrKeyList=attrKeyList
    formVisible.value = true;
      console.log(formData.value)
};
// 复选框 option
const getAttrValuesOption = (item) => {
  var ret=[]
console.log('item',item)
  item.checkValue.forEach(checkItem=>{
    var attrValueItem=item.mallProductAttrValueDtos.filter(x=>x.id==checkItem)
    if(attrValueItem&&attrValueItem[0]){
      ret.push({label:attrValueItem[0].attrValueName,value:checkItem})
    }
  })
  // if (value) {
  //   return value.map((attr) => ({
  //     label: attr.attrValueName,
  //     value: attr.id,
  //   }));
  // }
  return ret;
};
// 确定
const onSubmit = async () => {
var attr=[]

console.log('formData.value.attrKeyList',formData.value.attrKeyList)
formData.value.attrKeyList.forEach(item=>{
  if(item.checkId)
  {
    attr.push({attrKeyId:item.mallProductAttrKeyDto.id,attrValueId:item.checkId})
  }
})

    formVisible.value = false;
    emit('parentFetchData',{key:formData.value.id,attr:JSON.stringify(attr)});

};

// 取消
const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};

defineExpose({
  update,
});
</script>
