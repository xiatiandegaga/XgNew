<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="'50%'" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :label-width="100" @submit="onSubmit">
        <t-row style="margin: 0 auto">
          <t-col>
                <t-space size="24px">
              <t-button shape="rectangle" variant="outline" @click="toAdd()">新增</t-button>
              <t-button shape="rectangle" variant="outline" @click="batchDelete()" >删除</t-button>
            </t-space>
            <t-table :columns="COLUMNS_FORM" :data="formData.dataList" row-key="id" table-layout="fixed"
              :horizontal-scroll-affixed-bottom="true"  :selected-row-keys="selectedRowKeys" @select-change="onSelectChange">
            
              <template #name="{ row }">
                <t-input v-model="row.name" style="width: 100%" />
              </template>
              <template #value="{ row }">
                <t-input v-model="row.value" style="width: 100%" />
              </template>
              <template #sortNo="{ row }">
                <t-input-number v-model="row.sortNo" theme="column" style="width: 100px" />
              </template>
            </t-table>
          </t-col>
        </t-row>
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
import { MessagePlugin, FormRule, SubmitContext, Data ,PrimaryTableCol, TableRowData} from 'tdesign-vue-next';
import { addOrUpdate } from '@/api/mall/mallProductSku';
import { getAllDetailsById } from '@/api/mall/mallProductAttrKey';
import { getTypeAllList } from '@/api/mall/mallProductType';
import { findSingleDetailById, findSingleById } from '@/api/mall/mallProduct';

const title = ref(); // dialog标题
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({ dataList: [] });
const results: any = ref([]);
const count: Ref<number> = ref(0);
const emit = defineEmits(['parentFetchData']);
const selectedRowKeys = ref([]);
const COLUMNS_FORM:PrimaryTableCol<TableRowData>[] = [
  {
    colKey: 'row-select',
    fixed: 'left',
    type: 'multiple',
    width: 20,
  },
  {
    title: '参数名称',
    ellipsis: true,
    colKey: 'name',
  },
  {
    title: '参数值',
    ellipsis: true,
    colKey: 'value',
  },
 {
    title: '排序码',
    ellipsis: true,
    colKey: 'sortNo',
  },
];


// 编辑
const update = (id,list) => {
  formData.value.id=id
  formData.value.dataList=[]
  if(list&&list.length>0)
  {
    formData.value.dataList=list.sort((a,b)=>a.sortNo>b.sortNo?1:-1);
  }
  formVisible.value = true;
};

// 确定
const onSubmit = async () => {
var attr=[]
if(!formData.value.dataList||formData.value.dataList.length==0)
{
  MessagePlugin.error(`请填写参数！`);
  return
}

formData.value.dataList.forEach(item=>{
    attr.push({name:item.name,value:item.value,sortNo:item.sortNo})
})
    emit('parentFetchData',{key:formData.value.id,attr:JSON.stringify(attr)});
 formVisible.value = false;
};

// 取消
const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};
const onSelectChange = (value: Array<number>) => {
  selectedRowKeys.value = value;
};
// 新增
const toAdd = () => {
  if (!formData.value.dataList) formData.value.dataList = [];

  formData.value.dataList.push({
    id: --count.value
  });
};

const batchDelete = () => {
  if (selectedRowKeys.value.length > 0) {
    const dataList = formData.value.dataList.filter(
      (item) => selectedRowKeys.value.indexOf(item.id) === -1,
    );
    formData.value.dataList = [...dataList];
  }
};
defineExpose({
  update,
});
</script>
