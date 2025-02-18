<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="680" :footer="false" :closeOnEscKeydown="false"
    :closeOnOverlayClick="false" :onCloseBtnClick="onClickCloseBtn" destroyOnClose>
    <template #body>
      <!-- 表单内容 -->
      <t-form ref="form" :data="formData" :rules="rules" :label-width="100" @submit="onSubmit">
        <t-form-item label="菜单类型" name="category">
          <t-radio-group variant="primary-filled" v-model="formData.category" @change="changBtnPermission">
            <t-radio-button v-for="item in OPTIONS" :key="item.value" :value="item.value">{{
              item.label
            }}</t-radio-button>
          </t-radio-group>
        </t-form-item>
        <t-form-item label="上级菜单" name="pid">
          <t-tree-select v-model="formData.pid" :data="treeData" placeholder="请选择" :style="{ width: '480px' }"
            @change="changePermission(formData.pid)" />
        </t-form-item>
        <t-form-item label="菜单标题" name="metaTitle">
          <t-input v-model="formData.metaTitle" :style="{ width: '480px' }" placeholder="请输入菜单标题" />
        </t-form-item>
        <t-form-item label="菜单名称" name="name" v-if="formData.category === 1">
          <t-input v-model="formData.name" :style="{ width: '480px' }" placeholder="请输入菜单名称" />
        </t-form-item>
        <t-form-item label="菜单路径" name="path" v-if="formData.category === 1">
          <t-input v-model="formData.path" :style="{ width: '480px' }" placeholder="请输入路由地址" />
        </t-form-item>
        <t-form-item label="路由地址" name="component" v-if="formData.category === 1">
          <t-input v-model="formData.component" :style="{ width: '480px' }" placeholder="请输入路由地址" />
        </t-form-item>
        <t-form-item label="权限标识" name="permission">
          <t-input v-model="formData.permission" :style="{ width: '480px' }" placeholder="请输入权限标识" />
        </t-form-item>
        <t-form-item label="排序" name="sortNo">
          <t-input v-model="formData.sortNo" :style="{ width: '480px' }" placeholder="请输入排序码" />
        </t-form-item>
        <t-form-item label="图标" name="metaIcon">
          <t-input v-model="formData.metaIcon" :style="{ width: '480px' }" placeholder="请输入图标" />
        </t-form-item>
        <!-- <t-form-item label="备注" name="mark">
          <t-textarea v-model="textareaValue" :style="{ width: '480px' }" placeholder="请输入内容" name="description" />
        </t-form-item> -->
        <t-form-item style="float: right">
          <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
          <t-button theme="primary" type="submit">确定</t-button>
        </t-form-item>
      </t-form>
    </template>
  </t-dialog>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data } from 'tdesign-vue-next';
import { OPTIONS } from '../index.d';
import { findSingleById, addOrUpdate, getPageList } from '@/api/menu';
import { getTreeList } from '@/utils/tree';
import { getAllList } from '@/api/permission';

const title = ref();
const treeList = ref([]);
const treeData = ref([]);
const formVisible = ref(false);
const formData: any = ref({});
const emit = defineEmits(['parentFetchData']);

// 上级菜单
const menuData = async () => {
  try {
    const list = await getAllList();
    treeList.value = list;
    let newList = list.map(x => {
      const res = {
        label: x.metaTitle,
        value: x.id,
        id: x.id,
        pid: x.pid
      };
      return res
    })
    let rootTree = [{
      value: '0',
      label: '根节点',
      children: []
    }];
    rootTree[0].children = getTreeList(newList);
    treeData.value = rootTree
  } catch (e) {
    console.log(e);
  }
}
onMounted(() => {
  menuData();
})
const add = async () => {
  title.value = '新建菜单';
  formVisible.value = true;
  formData.value.category = 1
}

const update = async (id: number) => {
  title.value = '编辑菜单';
  if (id) {
    formData.value = await findSingleById({ id: id });
    formVisible.value = true;
  }
}

const onClickCloseBtn = () => {
  formData.value = {};
  formVisible.value = false;
};
// 选择上级菜单时带出权限
const changePermission = (e: string) => {
  if (formData.value.category === 2 && formData.value.pid !== '0') {
    const tree = treeList.value.filter(x => x.id === e)
    formData.value.permission = `${tree[0].permission}:`
  }
};
// 切换菜单类型时带出权限
const changBtnPermission = (checkedValues: number) => {
  if (checkedValues === 2) {
    if (formData.value.pid) {
      const tree = treeList.value.filter(x => x.id === formData.value.pid)
      formData.value.permission = `${tree[0].permission}:`
    } else {
      return
    }
  } else {
    return
  }
};

const onSubmit = async ({ validateResult, firstError }: SubmitContext<Data>) => {
  if (!firstError) {
    await addOrUpdate(formData.value);
    MessagePlugin.success('操作成功');
    formVisible.value = false;
    emit('parentFetchData');
  } else {
    MessagePlugin.warning(firstError);
  }
};

const rules: Record<string, FormRule[]> = {
  metaTitle: [{ required: true, message: '请输入菜单名称', type: 'error' }],
  path: [{ required: true, message: '请输入菜单路径', type: 'error' }],
  component: [{ required: true, message: '请输入路由地址', type: 'error' }],
  permission: [{ required: true, message: '请输入权限标识', type: 'error' }],
};

defineExpose({
  add,
  update
})
</script>
