<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="角色名称" name="name">
          <t-input v-model="formData.name" :style="{ width: '480px' }" placeholder="请输入角色名称" />
        </t-form-item>
        <t-form-item label="级别" name="level">
          <t-input v-model="formData.level" type="number" :style="{ width: '480px' }" placeholder="请输入级别" />
        </t-form-item>
        <t-form-item label="描述" name="description">
          <t-textarea placeholder="请输入描述" v-model="formData.description" :style="{ width: '480px' }" />
        </t-form-item>
        <t-form-item label="菜单分配">
          <t-tree v-model:value="formData.roleMenuIds" :data="treeData" hover :checkable="true" @change="onChange" />
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
import { MessagePlugin, FormRule, SubmitContext, TreeNodeValue, Data } from 'tdesign-vue-next';
import { findSingleById, addOrUpdate } from '@/api/system/role';
import { getTreeList } from '@/utils/tree';
import { getAllList } from '@/api/permission';

const title = ref(); //Modal标题
const form = ref<HTMLInputElement | null>(null);
const formVisible = ref(false);
let formData: any = ref({});
const emit = defineEmits(['parentFetchData']);
let treeList = reactive([])
let treeData = reactive([{
  value: '0',
  label: '根节点',
  children: []
},])

onMounted(() => {
  menuData();
})

// 新建
const add = async () => {
  title.value = '新建角色';
  formVisible.value = true;
}
// 编辑
const update = async (id: string) => {
  title.value = '编辑角色';
  if (id) {
    formData.value = await findSingleById(id);
    formVisible.value = true;
  }
}
// 获取菜单列表
const menuData = async () => {
  
  try {
    treeList = await getAllList();
    let newList = treeList.map(x => {
      const res = {
        label: x.metaTitle,
        value: x.id,
        id: x.id,
        pid: x.pid
      };
      return res
    })
    treeData[0].children = getTreeList(newList);
    treeData = [...treeData];
  } catch (e) {
    console.log(e);
  }
}
const onChange = (checked: Array<TreeNodeValue>) => {
  formData.value.roleMenuIds = checked
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
//验证规则
const rules: Record<string, FormRule[]> = {
  name: [{ required: true, message: '请输入角色名称', type: 'error' }],
};

defineExpose({
  add,
  update
})
</script>
<style>
.steps-content {
  margin-top: 30px;
}
</style>
