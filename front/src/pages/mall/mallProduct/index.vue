<template>
  <div class="list-common-table">
    <t-form ref="form" :data="formData" :label-width="80" colon @keyup.enter="fetchData">
      <t-row>
        <t-col :span="10">
          <t-row :gutter="[24, 24]">
            <t-col :span="4">
              <t-form-item label="商品名称" name="productName">
                <t-input v-model="formData.productName" class="form-item-content" type="search" placeholder="请输入商品名称"
                  clearable :style="{ minWidth: '134px' }" />
              </t-form-item>
            </t-col>
            <t-col :span="4">
              <t-form-item label="状态" name="status">
                  <t-select v-model="formData.status" :clearable="true" :style="{ minWidth: '134px' }"
            placeholder="请选择状态"  >
            <t-option key="1" label="上架" value="1" />
            <t-option key="0" label="下架" value="0" />
          </t-select>
              </t-form-item>
            </t-col>
          </t-row>
        </t-col>
        <t-col :span="2" class="operation-container">
          <t-button theme="primary" :style="{ marginLeft: 'var(--td-comp-margin-s)' }" @click="fetchData">
            查询
          </t-button>
          <t-button v-auth="'mallProduct:add'" @click="handleClickAdd()"> 新建 </t-button>
          <t-button type="reset" variant="base" theme="default"> 重置 </t-button>
        </t-col>
      </t-row>
    </t-form>
    
    <div class="table-container">
      <t-table :data="data" :columns="COLUMNS" row-key="id" vertical-align="top" :hover="true" :pagination="pagination"
        :loading="dataLoading" @page-change="onPageChange">
        <template #status="slotProps">
          <t-tag theme="success" v-if="slotProps.row.status==1">上架</t-tag> 
          <t-tag theme="danger" v-else>下架</t-tag>
        </template>
        <template #mallProductCategoryId="slotProps">
          <p>{{ getCategoryName(slotProps.row) }}</p>
        </template>
        <template #mallProductTypeId="slotProps">
          <p>{{ getTypeName(slotProps.row) }}</p>
        </template>
        <!-- <template #config="slotProps">
          <t-button theme="default" variant="outline" @click="handleEditAttr(slotProps)">编辑属性</t-button>
        </template> -->
        <template #op="slotProps">
           <a class="t-button-link" @click="handleClickShow(slotProps)">查看</a>
          <a v-auth="'mallProduct:edit'" class="t-button-link" @click="handleClickUpdate(slotProps)">编辑</a>
          <a v-auth="'mallProduct:copy'" class="t-button-link" @click="handleClickCopy(slotProps)">复制</a>
          <t-popconfirm content="确认删除吗" @confirm="handleClickDelete(slotProps)" >
            <a v-auth="'mallProduct:delete'" class="t-button-link">删除</a>
          </t-popconfirm>
           <t-popconfirm content="确定要上架吗" @confirm="handleTakeOn(slotProps.row)">
            <a v-auth="'mallProduct:takeOn'" v-if="slotProps.row.status==0" class="t-button-link">上架</a>
          </t-popconfirm>
           <t-popconfirm content="确定要下架吗" @confirm="handleTakeOff(slotProps.row)" >
            <a v-auth="'mallProduct:takeOff'" v-if="slotProps.row.status==1"  class="t-button-link">下架</a>
          </t-popconfirm>
        </template>
      </t-table>
    </div>

    <mall-product-form ref="mallProductForm" @parent-fetch-data="fetchData" />
  </div>
</template>
<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { PageInfo, MessagePlugin } from 'tdesign-vue-next';
import { getPageList, logicDelete, takeOn,takeOff } from '@/api/mall/mallProduct';
import { COLUMNS, PAGINATION, SEARCHFORM } from './index.d';
import { getCategoryAllList } from '@/api/mall/mallProductCategory';
import { getTypeAllList } from '@/api/mall/mallProductType';
import type { Params, ParamsFilter } from '@/api/model/common';
import MallProductForm from './components/MallProductForm.vue';


let data = reactive([]); // 列表数据
const formData = reactive({ ...SEARCHFORM }); // 搜索数据
const pagination = reactive(PAGINATION); // 分页数据
const dataLoading = ref(false); // 控制弹窗是否打开
let ctgrList = reactive([]);
let typeList = reactive([]);

onMounted(() => {
  fetchData();
  getAllProductList();
});

const getAllProductList = async () => {
  // brandList = await getBrandAllList();
  ctgrList = await getCategoryAllList();
  typeList = await getTypeAllList();
};

// 查询品牌列表
const fetchData = async () => {
  dataLoading.value = true;
  const filterInfo = filter();
  try {
    const params: Params = {
      pageIndex: pagination.current,
      pageSize: pagination.pageSize,
      filter: filterInfo,
    };
    const { list, totalCount } = await getPageList(params);
    data = list;
    pagination.total = parseInt(totalCount);
  } catch (e) {
    console.log(e);
  } finally {
    dataLoading.value = false;
  }
};
// 查询参数
const filter = () => {
  const filterInfo: Array<ParamsFilter> = [];
  if (formData.productName) 
  {
filterInfo.push({ op: 5, propertyName: 'productName', value: formData.productName });
  }
  if (formData.status||formData.status===0) 
  {
filterInfo.push({ op: 0, propertyName: 'status', value: formData.status });
  }
  return filterInfo;
};
// 获取dialogForm元素节点
const mallProductForm = ref<any>(null);


// 设置属性
const handleEditAttr = ({ row }) => {
 // mallAttrForm.value.update(row.id);
};
// 新建Modal
const handleClickAdd = () => {
  mallProductForm.value.add();
};
// 编辑Modal
const handleClickUpdate = ({ row }) => {
  mallProductForm.value.update(row.id);
};
const handleClickShow=({row})=>{
  mallProductForm.value.show(row.id);
}
const handleClickCopy = ({ row }) => {
  mallProductForm.value.copy(row.id);
};

const getCategoryName = (value) => {
  for (let i = 0; i < ctgrList.length; i++) {
    if (ctgrList[i].id == value.mallProductCategoryId) {
      return ctgrList[i].categoryName;
    }
  }
};
const getTypeName = (value) => {
  for (let i = 0; i < typeList.length; i++) {
    if (typeList[i].id == value.mallProductTypeId) {
      return typeList[i].typeName;
    }
  }
};

// 根据Id删除品牌
const handleClickDelete = async ({ row }) => {
  try {
    await logicDelete(row.id);
    fetchData();
    MessagePlugin.success('删除成功');
  } catch (e) {
    MessagePlugin.warning('删除失败');
  }
};
const handleTakeOn=async (row)=>
{
    await takeOn(row.id);
    fetchData();
    MessagePlugin.success('上架成功');
}
const handleTakeOff=async (row)=>
{
   try {
    await takeOff(row.id);
    fetchData();
    MessagePlugin.success('下架成功');
  } catch (e) {
    MessagePlugin.error('下架失败');
  }
}
// 点击下一页
const onPageChange = (pageInfo: PageInfo) => {
  pagination.current = pageInfo.current;
  pagination.pageSize = pageInfo.pageSize;
    fetchData()
};
</script>

<style lang="less" scoped>
.list-common-table {
  background-color: var(--td-bg-color-container);
  padding: var(--td-comp-paddingTB-xxl) var(--td-comp-paddingLR-xxl);
  border-radius: var(--td-radius-medium);

  .table-container {
    margin-top: var(--td-comp-margin-xxl);
  }
}

.form-item-content {
  width: 100%;
}

.operation-container {
  display: flex;
  justify-content: flex-end;
  align-items: center;

  .expand {
    .t-button__text {
      display: flex;
      align-items: center;
    }
  }
}

.payment-col {
  display: flex;

  .trend-container {
    display: flex;
    align-items: center;
    margin-left: var(--td-comp-margin-s);
  }
}
</style>
