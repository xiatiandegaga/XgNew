<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData.mallProductAttrKeyDto" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="属性名" name="attrKeyName">
          <t-input v-model="formData.mallProductAttrKeyDto.attrKeyName" :style="{ width: '480px' }"
            placeholder="请输入属性名" />
        </t-form-item>
        <t-form-item label="商品类型" name="mallProductTypeId">
          <t-select v-model="formData.mallProductAttrKeyDto.mallProductTypeId" :clearable="true"
            :style="{ width: '480px' }" placeholder="请选择" @change="productTypeChange">
            <t-option v-for="item in productTypeList" :key="item.id" :value="item.id" :label="item.typeName">{{
              item.typeName
              }}</t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="商品目录" name="mallProductCategoryId">
          <t-select v-model="formData.mallProductAttrKeyDto.mallProductCategoryId" :clearable="true"
            :style="{ width: '480px' }" placeholder="请选择">
            <t-option v-for="item in ctgrList" :key="item.id" :value="item.id" :label="item.categoryName">{{
              item.categoryName
              }}</t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="排序" name="sortNo">
          <t-input v-model="formData.mallProductAttrKeyDto.sortNo" :style="{ width: '480px' }" placeholder="请输入排序码" />
        </t-form-item>
        <t-row style="margin: 0 auto">
          <t-col>
            <t-space size="24px">
              <t-button shape="rectangle" variant="outline" @click="toAdd()">新增</t-button>
              <!-- <t-button shape="rectangle" variant="outline" @click="batchDelete()">删除</t-button> -->
            </t-space>
            <t-table :columns="COLUMNS_FORM" :data="formData.mallProductAttrValueDtos" row-key="id" table-layout="fixed"
              :horizontal-scroll-affixed-bottom="true" 
             >
              <template #attrValueName="{ row }">
                <t-input v-model="row.attrValueName" style="width: 100%" />
              </template>
              <!-- <template #attrValueName="{ row }">
                <t-input v-model="row.mallProductCategoryId" style="width: 100%" />
              </template> -->
              <template #sortNo="{ row }">
                <t-input-number v-model="row.sortNo" theme="column" style="width: 100px" />
              </template>
                    <template #op="{row}">
          <t-popconfirm content="确认删除吗" @confirm="toDelete(row.id)" v-if="row.id<=0||!isEdit">
            <a  class="t-button-link">删除</a>
          </t-popconfirm>
        </template>
            </t-table>
          </t-col>
        </t-row>
        <t-form-item style="float: right; margin-top: 20px">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit" v-if="isEdit">确定</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, Ref } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { COLUMNS_FORM } from '../index.d';
import { AttrDetail } from '@/api/model/mall/mallProductCategoryModel';
import { findSingleById, addOrUpdate } from '@/api/mall/mallProductAttrKey';
import { logicDelete} from '@/api/mall/mallProductAttrValue';
import { getTypeAllList } from '@/api/mall/mallProductType';
import { getCategoryAllList ,getListByTypeId} from '@/api/mall/mallProductCategory';

const title = ref(); // dialog标题
let productTypeList = reactive([]); // 商品类型列表
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});
const count: Ref<number> = ref(0);
const selectedRowKeys = ref([]);
const emit = defineEmits(['parentFetchData']);
let ctgrList = ref([]); // 商品目录列表
let isEdit=ref(false)
onMounted(() => {
  getProductType();
  
});

// 商品类型
const getProductType = async () => {
  productTypeList = await getTypeAllList();
};

// 新建
const add = async () => {
  isEdit.value=true
  title.value = '新建属性信息';
  formData.value.mallProductAttrKeyDto = {};
  formData.value.mallProductAttrValueDtos = [];
  formVisible.value = true;
};
// 编辑
const update = async (id: string,edit:boolean) => {
  isEdit.value=edit
  title.value = '编辑属性信息';
  if (id) {
    const { mallProductAttrKeyDto, mallProductAttrValueDtos } = await findSingleById(id);
    formData.value.mallProductAttrKeyDto = mallProductAttrKeyDto;
    formData.value.mallProductAttrValueDtos = mallProductAttrValueDtos;
 
    ctgrList.value= await getListByTypeId(formData.value.mallProductAttrKeyDto.mallProductTypeId)
    formVisible.value = true;
  }
};

// 新增
const toAdd = () => {
  if (!formData.value.mallProductAttrValueDtos) formData.value.mallProductAttrValueDtos = [];

  formData.value.mallProductAttrValueDtos.push({ id: --count.value });
  formData.value.mallProductAttrValueDtos = [...formData.value.mallProductAttrValueDtos];
};
// 删除
const batchDelete = () => {
  if (selectedRowKeys.value.length > 0) {
    const mallProductAttrValueDtos = formData.value.mallProductAttrValueDtos.filter(
      (item: AttrDetail) => selectedRowKeys.value.indexOf(item.id) === -1,
    );
    formData.value.mallProductAttrValueDtos = [...mallProductAttrValueDtos];
  }
};
const toDelete =async (id) => {
  if(Number(id)>0)
  {
  await logicDelete(id)
  }

      const mallProductAttrValueDtos = formData.value.mallProductAttrValueDtos.filter(
      x=>x.id!=id
    );
    formData.value.mallProductAttrValueDtos = [...mallProductAttrValueDtos];
  
  
};
// 选择
const onSelectChange = (value: Array<number>) => {
  selectedRowKeys.value = value;
  console.log(value);
};

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
// 验证规则
const rules: Record<string, FormRule[]> = {
  attrKeyName: [{ required: true, message: '请输入属性名称', type: 'error' }],
   mallProductTypeId: [{ required: true, message: '请选择商品类型', type: 'error' }],
     mallProductCategoryId: [{ required: true, message: '请选择商品目录', type: 'error' }],
};
const productTypeChange=async (typeId)=>{
  formData.value.mallProductAttrKeyDto.mallProductCategoryId=undefined
  ctgrList.value= await getListByTypeId(typeId)

}
defineExpose({
  add,
  update,
});
</script>
