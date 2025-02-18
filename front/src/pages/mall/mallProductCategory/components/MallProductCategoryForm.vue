<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="目录名称" name="categoryName">
          <t-input v-model="formData.categoryName" :style="{ width: '480px' }" placeholder="请输入目录名称" />
        </t-form-item>
        <t-form-item label="目录编号" name="categoryCode">
          <t-input v-model="formData.categoryCode" :style="{ width: '480px' }" placeholder="请输入目录编号" />
        </t-form-item>
        <t-form-item label="上级目录" name="pid">
          <t-tree-select v-model="formData.pid" :data="treeData" placeholder="请选择" :style="{ width: '480px' }" />
        </t-form-item>
        <!-- <t-form-item label="商品品牌" name="brandId">
          <t-select v-model="formData.brandId" :clearable="true" :style="{ width: '480px' }" placeholder="请选择品牌">
            <t-option v-for="item in brandList" :key="item.id" :value="item.id" :label="item.brandName">{{
              item.brandName
            }}</t-option>
          </t-select>
        </t-form-item> -->
        <t-form-item label="商品类型" name="mallProductTypeId">
          <t-select v-model="formData.mallProductTypeId" :clearable="true" :style="{ width: '480px' }" placeholder="请选择类型">
            <t-option v-for="item in typeList"  :key="item.id" :value="item.id" :label="item.typeName">
              {{ item.typeName }}
            </t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="排序" name="sortNo">
          <t-input v-model="formData.sortNo" :style="{ width: '480px' }" placeholder="请输入排序码" />
        </t-form-item>
        <t-form-item label="目录图片" name="imageUrl">
          <upload-img ref="imageUrl" v-model="formData.imageUrl" :limit="1" style="width: 100%"></upload-img>
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
import { reactive, ref, onMounted,defineComponent } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import UploadImg from '@/components/upload/uploadImg.vue';
import { getTreeList } from '@/utils/tree';
import { findSingleById, addOrUpdate, getCategoryAllList } from '@/api/mall/mallProductCategory';
import { getTypeAllList } from '@/api/mall/mallProductType';

const title = ref(); // dialog标题
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});
const treeList = ref([]);
const data = ref([]); // 列表数据
const treeData = ref([]);
let typeList = reactive([]);
const emit = defineEmits(['parentFetchData']);

onMounted(() => {
  selectType();
});

// 上级菜单
const menuData = async () => {
  try {
    const list = await getCategoryAllList();
    treeList.value = list;
    const newList = list.map((x) => {
      console.log('x', x);
      const res = {
        label: x.categoryName,
        value: x.id,
        id: x.id,
        pid: x.pid,
        mallProductTypeId: x.mallProductTypeId,
      };
      return res;
    });
    const rootTree = [
      {
        value: '0',
        label: '根节点',
        children: [],
      },
    ];
    rootTree[0].children = getTreeList(newList);
    treeData.value = rootTree;
  } catch (e) {
    console.log(e);
  }
};

// 新建
const add = async () => {
  title.value = '新建类型信息';
  formVisible.value = true;
  menuData();
  selectType();
};
// 编辑
const update = async (id: string) => {
  title.value = '编辑类型信息';
  menuData();
  if (id) {
    formData.value = await findSingleById(id);

    formVisible.value = true;
    setTimeout(() => {
      if (formData.value.imageUrl) imageUrl.value.init();
    });
  }
};
// 图片ref
const imageUrl = ref<any>(null);

// // 商品品牌
// const getBrand = async () => {
//   brandList = await getBrandAllList()
// }

// 商品类型
const selectType = async () => {
  typeList = await getTypeAllList();
  // dataLoading.value = true;
  // const filterInfo: any = [];
  // if (formData.value.mallProductTypeId)
  //   filterInfo.push({ op: 5, propertyName: 'mallProductTypeId', value: formData.value.mallProductTypeId });
  // console.log('formData', formData);
  // try {
  //   const typeList = await getTypeAllList({ filter: filterInfo });
  //   console.log('商品类型目录', typeList);
  //   data.value = getTreeList(typeList);
  // } catch (e) {
  // } finally {
  // dataLoading.value = false;
  // }
};
// const fetchData = async () => {
//   dataLoading.value = true;
//   const filterInfo: any = [];
//   if (formData.value.ctgrName) filterInfo.push({ op: 5, propertyName: 'ctgrName', value: formData.value.ctgrName });
//   try {
//     const list = await getAllList({ filter: filterInfo });
//     console.log('商品目录', list);
//     data.value = getTreeList(list);
//   } catch (e) {
//   } finally {
//     dataLoading.value = false;
//   }
// };

// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (imageUrl.value) {
    await imageUrl.value.uploadImg();
    formData.value.imageUrl = imageUrl.value.getFilesUrl();
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
// 验证规则
const rules: Record<string, FormRule[]> = {
  categoryName: [{ required: true, message: '请输入目录名称', type: 'error' }],
  mallProductTypeId: [{ required: true, message: '商品类型不能为空', type: 'error' }],
};

defineExpose({
  add,
  update,
});
</script>
