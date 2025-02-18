<template>
  <t-dialog v-model:visible="formVisible" :header="title" :width="width" :footer="false" :close-on-esc-keydown="false"
    :close-on-overlay-click="false" :on-close-btn-click="onClickCloseBtn" destroy-on-close>
    <template #body>
      <!-- 表单内容 -->
      <!-- <t-form ref="form" :data="formData" layout="inline" :rules="rules" :label-width="100" > -->
      <t-form ref="form" :data="formData" layout="inline" :rules="rules" :label-width="140" @submit="onSubmit">
        <t-form-item label="商品名称" name="productName">
          <t-input v-model="formData.productName" :style="{ width: inputWidth }" placeholder="请输入商品名称" />
        </t-form-item>
        <t-form-item label="商品简称" name="productShortName">
          <t-input v-model="formData.productShortName" :style="{ width: inputWidth }" placeholder="请输入商品简称" />
        </t-form-item>
        <!-- <t-form-item label="商品编号" name="productCode">
          <t-input v-model="formData.productCode" :style="{ width: '480px' }" placeholder="请输入商品编号" />
        </t-form-item> -->
        <!-- <t-form-item label="商品品牌" name="brandId">
          <t-select v-model="formData.brandId" :clearable="true" :style="{ width: '480px' }" placeholder="请选择品牌">
            <t-option v-for="item in brandList" :key="item.id" :value="item.id" :label="item.brandName">{{
              item.brandName
              }}</t-option>
          </t-select>
        </t-form-item> -->
        <t-form-item label="商品类型" name="mallProductTypeId">
          <t-select v-model="formData.mallProductTypeId" :clearable="true" :style="{ width: inputWidth }"
            placeholder="请选择类型" :disabled="formData.id > 0" @change="productTypeChange">
            <t-option v-for="item in typeList" :key="item.id" :value="item.id" :label="item.typeName">{{
              item.typeName
            }}</t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="商品目录" name="mallProductCategoryId">
          <t-select v-model="formData.mallProductCategoryId" :clearable="true" :style="{ width: inputWidth }"
            placeholder="请选择目录" @change="changeCategory" :disabled="formData.id > 0">
            <t-option v-for="item in ctgrList" :key="item.id" :value="item.id" :label="item.categoryName">{{
              item.categoryName
            }}</t-option>
          </t-select>
        </t-form-item>

        <t-form-item label="商品属性">
          <t-select v-model="formData.checkList" :clearable="true" :style="{ width: inputWidth }" placeholder="请选择"
            multiple @change="changeAttr" :disabled="formData.id > 0">
            <t-option v-for="item in attrList" :key="item.mallProductAttrKeyDto.id"
              :value="item.mallProductAttrKeyDto.id" :label="item.mallProductAttrKeyDto.attrKeyName">
              {{ item.mallProductAttrKeyDto.attrKeyName }}
            </t-option>
          </t-select>
        </t-form-item>
        <t-form-item label="单位名称" name="unitName">
          <t-input v-model="formData.unitName" :style="{ width: inputWidth }" placeholder="请输入单位名称" />
        </t-form-item>
        <t-form-item label="商品主图" name="productMainImg">
          <upload-img ref="productMainImg" v-model="formData.productMainImg" :limit="1"
            :style="{ width: inputWidth }"></upload-img>
        </t-form-item>
        <t-form-item label="商品详情图" name="productDetailImg" :style="{ width: inputWidth }">
          <upload-img ref="productDetailImg" v-model="formData.productDetailImg" :limit="6"
            :style="{ width: inputWidth }"></upload-img>
        </t-form-item>
        <t-form-item label="最低价格" name="minPrice">
          <t-input v-model="formData.minPriceAmount" :style="{ width: inputWidth }" placeholder="请输入最低价格"
            :disabled="true" />
        </t-form-item>
        <t-form-item label="最高价格" name="maxPrice">
          <t-input v-model="formData.maxPriceAmount" :style="{ width: inputWidth }" placeholder="请输入最高价格"
            :disabled="true" />
        </t-form-item>
        <t-form-item label="备注" name="remark">
          <t-input v-model="formData.remark" :style="{ width: inputWidth }" placeholder="请输入备注" />
        </t-form-item>
        <t-form-item label="卖点描述" name="desc">
          <t-input v-model="formData.desc" :style="{ width: inputWidth }" placeholder="请输入卖点描述" />
        </t-form-item>
        <t-form-item label="排序" name="sortNo">
          <t-input v-model="formData.sortNo" :style="{ width: inputWidth }" placeholder="请输入排序码" />
        </t-form-item>
        <t-form-item label="是否推荐" name="recommendStatus">
          <t-select v-model="formData.recommendStatus" clearable :style="{ width: inputWidth }">
            <t-option v-for="(item, index) in GOOD_RECOMMEND_STATUS_OPTIONS" :key="index" :value="item.value"
              :label="item.label">
              {{ item.label }}
            </t-option>
          </t-select>
        </t-form-item>
        <!-- <t-form-item label="是否上架" name="status">
          <t-select v-model="formData.status" clearable :style="{ width: inputWidth}">
            <t-option v-for="(item, index) in GOOD_PUBLIC_STATUS_OPTIONS" :key="index" :value="item.value"
              :label="item.label">
              {{ item.label }}
            </t-option>
          </t-select>
        </t-form-item> -->
        <t-form-item label="是否新品" name="newStatus">
          <t-select v-model="formData.newStatus" clearable :style="{ width: inputWidth }">
            <t-option v-for="(item, index) in GOOD_NEW_STATUS_OPTIONS" :key="index" :value="item.value"
              :label="item.label">
              {{ item.label }}
            </t-option>
          </t-select>
        </t-form-item>

        <div style="display: flex; flex-direction: column; width: 100%">
          <template v-for="item in attrKeyList" :key="item.mallProductAttrKeyDto.id">
            <t-form-item :label="item.mallProductAttrKeyDto.attrKeyName">
              <t-checkbox-group v-model="item.checkValue" :options="getAttrValuesOption(item.mallProductAttrValueDtos)"
                name="city" @change="onChangeAttr"></t-checkbox-group>
            </t-form-item>
          </template>
        </div>
        <t-row style="margin: 0 auto">
          <t-col style="max-width: 1400px">
            <t-space size="24px" v-if='isEdit'>
              <t-button shape="rectangle" variant="outline" @click="toAdd()">新增</t-button>
              <t-button shape="rectangle" variant="outline" @click="autoCreateSku()"
                v-if="!formData.id">自动生成sku</t-button>
            </t-space>
            <t-table :columns="COLUMNS_FORM" :data="formData.mallProductSkuDtos" row-key="id" table-layout="fixed"
              :horizontal-scroll-affixed-bottom="true" :selected-row-keys="selectedRowKeys"
              :table-content-width="'2000px'" @select-change="onSelectChange">
              <template #attrKeyValue="{ row }">
                <span v-if="row.attrKeyValue" style="width: 100%">
                  {{ getSkuName(row.attrKeyValue) }}</span>
                <t-button v-else @click="mallAttrFormUpdate(row.id)">选择属性</t-button>
              </template>
              <template #specParam="{ row }">
                <div v-if="row.specParam && row.specParam != '[]' && row.specParam != '{}'" class="specParam">
                  <span>{{ getSpecParamName(row.specParam) }}</span>
                  <t-button size="small" theme="success" @click="specParamUpdate(row)">{{ isEdit ? '更改' : '查看'
                    }}</t-button>
                </div>

                <t-button v-else @click="specParamUpdate(row)">选择参数</t-button>
              </template>
              <template #skuPriceAmount="{ row }">
                <t-input v-model="row.skuPriceAmount" style="width: 100%" @change="oriPriceChange(row)" />
              </template>
              <template #numberOfInstallments="{ row }">
                <t-select v-model="row.numberOfInstallments" style="width: 100%" placeholder="请选择" multiple
                  :min-collapsed-num="2">
                  <t-option v-for="item in numberOfInstallmentsList" :key="item.id" :value="item.constKey"
                    :label="item.name">{{ item.name }}</t-option>
                </t-select>
              </template>
              <template #skuStock="{ row }">
                <t-input v-model="row.skuStock" style="width: 100%" :disabled="formData.id > 0" />
              </template>
              <template #status="{ row }">
                <t-radio-group v-model="row.status" @change="oriPriceChange(row)">
                  <t-radio :value="1" :checked="true">启用</t-radio>
                  <t-radio :value="0">停用</t-radio>

                </t-radio-group>
              </template>
              <template #sortNo="{ row }">
                <t-input-number v-model="row.sortNo" min="0" theme="column" style="width: 100px" />
              </template>

              <template #op="{ row }">
                <t-popconfirm content="确认删除吗" @confirm="toDelete(row.id)" v-if="row.id <= 0">
                  <a class="t-button-link">删除</a>
                </t-popconfirm>
                <a class="t-button-link" @click="handleClickStockInv(row)" v-if="!isEdit">入库</a>
                <a class="t-button-link" @click="handleClickStockRel(row)" v-if="!isEdit">出库</a>
                <a class="t-button-link" @click="uploadSkuImg(row.id, row.skuImg)" v-if="isEdit">上传图片</a>
              </template>
            </t-table>
          </t-col>
        </t-row>
        <t-row>
          <t-col style="float: right">
            <t-button variant="outline" @click="onClickCloseBtn">取消</t-button>
            <t-button theme="primary" type="submit" v-if='isEdit'>确定</t-button>
          </t-col>
        </t-row>
      </t-form>

      <div style="position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); z-index: 10">
        <!-- <t-dialog v-model:visible="visibleNormalDrag" header="普通对话框" mode="normal" draggable
          :on-confirm="onConfirmAnother">
          <template v-for="(item, index) in attrKeyList">
            <t-form-item :label="item.mallProductAttrKeyDto.attrKeyName">
              <t-radio-group v-model="item.id" :options="getAttrValuesOption(item.mallProductAttrValueDtos)" name="city"
                @change="addAttr(item, index)"></t-radio-group>
            </t-form-item>
          </template>
        </t-dialog> -->
        <t-dialog v-model:visible="visibleNormalDrag1" placement="center" header="参数表格" mode="normal" draggable
          :on-confirm="paramSubmit">
          <table>
            <thead>
              <tr>
                <th>参数名</th>
                <th>参数值</th>
                <th>操作</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in tableData" :key="index">
                <td>
                  <t-input v-model="item.name" auto-width></t-input>
                </td>
                <td>
                  <t-input v-model="item.value" auto-width></t-input>
                </td>
                <td>
                  <t-button size="small" @click="addRow">增加</t-button>
                  <t-button size="small" @click="removeRow(index)">删除</t-button>
                </td>
              </tr>
            </tbody>
          </table>
        </t-dialog>
      </div>
    </template>
  </t-dialog>

  <mall-attr-form ref="mallAttrForm" @parentFetchData="selectAttr" />
  <SpecParamForm ref="specParamForm" @parentFetchData="selectSpecParam" />
  <MallProductInvOrRelForm ref="mallProductInvOrRelForm" @parentFetchData="selectProductInvOrRel" />
  <EditImg ref="editImg" @parentFetchData="uploadImgHandleOk"></EditImg>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { MessagePlugin, FormRule, SubmitContext, Data, TableProps, install } from 'tdesign-vue-next';
import dayjs from 'dayjs';
import { format, number } from 'echarts/core';
import UploadImg from '@/components/upload/uploadImg.vue';
import { GOOD_RECOMMEND_STATUS_OPTIONS, GOOD_NEW_STATUS_OPTIONS, GOOD_PUBLIC_STATUS_OPTIONS } from '@/constants';
import { findSingleById, addOrUpdate } from '@/api/mall/mallProduct';
import { COLUMNS_FORM } from '../index.d';
import { getCategoryAllList, getListByTypeId } from '@/api/mall/mallProductCategory';
import { getTypeAllList } from '@/api/mall/mallProductType';
import { getAllList } from '@/api/system/globalDataDetail';
import { getBrandAllList } from '@/api/mall/mallBrand';
import { getAllDetailsById } from '@/api/mall/mallProductAttrKey';
import MallAttrForm from './MallAttrForm.vue';
import SpecParamForm from './specParamForm.vue';
import MallProductInvOrRelForm from './mallProductInvOrRelForm.vue';
import EditImg from '@/components/upload/editImg.vue';

const width = ref('80%');
const inputWidth = ref('400px');
const title = ref(); // dialog标题
const formVisible = ref(false); // 控制是否展示dialog
const formData: any = ref({});
let brandList = reactive([]); // 商品品牌列表
let ctgrList = ref([]); // 商品目录列表
let typeList = ref([]); // 商品类型列表
const attrList = ref([]); // 商品属性列表
const attrKeyList = ref([]); // 商品属性值列表
const valueList = ref([]); // 商品属性值列表
const specParamNameList = ref([]); // 新增商品属性值列表
const skuNameList = ref(); // 新增商品属性值列表
const count = ref(0);
const selectedRowKeys = ref([]);
const numberOfInstallmentsList: any = ref([]);
const record: any = ref([]);
const checkSkuAttrList = ref([]); // 选中的属性列表
const results: any = ref([]);
const temp: any = ref([]);
const visibleNormalDrag = ref(false);
const visibleNormalDrag1 = ref(false);
const emit = defineEmits(['parentFetchData']);
const mallAttrForm = ref<any>(null);
const tableData = ref([{ name: '', value: '' }]);
const specParamForm = ref<any>(null);
const mallProductInvOrRelForm = ref<any>(null)
const editImg = ref<any>(null)

let isEdit = ref(false)
onMounted(() => {
  // getBrand();
  // getCtgr();
  getNumberOfInstallments();
  getAllProductList();
});

const getAllProductList = async () => {
  brandList = await getBrandAllList();
  typeList.value = await getTypeAllList();
};



const addRow = () => {
  tableData.value.push({ name: '', value: '' });
};
const removeRow = (index) => {
  tableData.value.splice(index, 1);
};

const getSkuName = (value) => {
  let name = '';

  if (value) {
    JSON.parse(value).forEach((item) => {
      const attr = attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item.attrKeyId)[0];
      if (attr) {
        // const attrKeyName = attr.name;
        const { attrValueName } = attr.mallProductAttrValueDtos.filter((x) => x.id == item.attrValueId)[0];
        name += `${attrValueName},`;
      }
    });
  }
  return !name ? '' : name.substr(0, name.length - 1);
};

const getSpecParamName = (value) => {
  let name = '';
  if (value) {
    JSON.parse(value).forEach((item) => {
      name += `${item.name}:${item.value} `;
    });
  }
  return name;
};

// 新建
const add = async () => {
  title.value = '新建';
  formVisible.value = true;
  isEdit.value = true
  formData.value.mallProductAttrDtos = {};
  formData.value.mallProductSkuDtos = [];
};
const show = async (id: string) => {
  title.value = '查看';

  isEdit.value = false
  await initData(id)
  formVisible.value = true;
};
// 编辑
const update = async (id: string) => {
  title.value = '编辑';

  isEdit.value = true
  await initData(id)
  formVisible.value = true;
};

const initData = async (id: string) => {
  if (id) {
    formData.value = await findSingleById(id);
    getNumberOfInstallments();

    ctgrList.value = await getListByTypeId(formData.value.mallProductTypeId)
    attrList.value = await getAllDetailsById(formData.value.mallProductCategoryId);
    const checkList = [];

    if (formData.value.mallProductAttrDtos.length > 0) {
      formData.value.mallProductAttrDtos.forEach((item) => {
        checkList.push(item.mallProductAttrKeyId);
      });
      formData.value.checkList = [...new Set(checkList)];
    }

    if (formData.value.mallProductSkuDtos.length > 0) {
      const attrValueList = [];

      let checkAttr = [];
      formData.value.mallProductSkuDtos.forEach((item) => {
        item.numberOfInstallments = item.numberOfInstallments.split(',');
        checkAttr = JSON.parse(item.attrKeyValue);
        checkAttr.forEach((item) => {
          attrValueList.push(item);
        });
      });
      const valueList = [];

      attrList.value.forEach((item) => {
        valueList.push(item.mallProductAttrKeyDto.id);
      });
      attrKeyList.value = [];
      valueList.forEach((item) => {
        if (attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item).length > 0) {
          const attr = attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item)[0];
          attrKeyList.value.push(attr);
        }
      });
      attrKeyList.value.forEach((item) => {
        item.checkValue = []
        if (formData.value.mallProductAttrDtos.length > 0) {
          formData.value.mallProductAttrDtos.forEach((attritem) => {
            if (attritem.mallProductAttrKeyId == item.mallProductAttrKeyDto.id && item.checkValue.indexOf(x => x == attritem.mallProductAttrValueId) == -1) {
              item.checkValue.push(attritem.mallProductAttrValueId)
            }
          });

        }
      });
    }


    setTimeout(() => {
      if (formData.value.productMainImg) productMainImg.value.init();
      if (formData.value.productDetailImg) productDetailImg.value.init();
    });
  }
}
// 复制
const copy = async (id: string) => {
  title.value = '复制';
  isEdit.value = true
  if (id) {
    formData.value = await findSingleById(id);
    getNumberOfInstallments();

    ctgrList.value = await getListByTypeId(formData.value.mallProductTypeId)
    attrList.value = await getAllDetailsById(formData.value.mallProductCategoryId);
    const checkList = [];

    if (formData.value.mallProductAttrDtos.length > 0) {
      formData.value.mallProductAttrDtos.forEach((item) => {
        checkList.push(item.mallProductAttrKeyId);
      });
      formData.value.checkList = [...new Set(checkList)];
    }

    if (formData.value.mallProductSkuDtos.length > 0) {
      const attrValueList = [];

      let checkAttr = [];
      formData.value.mallProductSkuDtos.forEach((item) => {
        item.numberOfInstallments = item.numberOfInstallments.split(',');
        checkAttr = JSON.parse(item.attrKeyValue);
        checkAttr.forEach((item) => {
          attrValueList.push(item);
        });
      });
      const valueList = [];

      attrList.value.forEach((item) => {
        valueList.push(item.mallProductAttrKeyDto.id);
      });
      attrKeyList.value = [];
      valueList.forEach((item) => {
        if (attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item).length > 0) {
          const attr = attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item)[0];
          attrKeyList.value.push(attr);
        }
      });
      attrKeyList.value.forEach((item) => {
        item.checkValue = []

      });
    }
    formData.value.id = 0
    formData.value.mallProductSkuDtos = [];
    formVisible.value = true;
    setTimeout(() => {
      if (formData.value.productMainImg) productMainImg.value.init();
      if (formData.value.productDetailImg) productDetailImg.value.init();
    });
  }
};

const selectParam = (row) => {
  visibleNormalDrag1.value = true;
  record.value = row;
};


const paramSubmit = () => {
  visibleNormalDrag1.value = false;
  tableData.value.forEach((item) => {
    specParamNameList.value.push({
      name: item.name,
      value: item.value,
    });
  });
  const index = formData.value.mallProductSkuDtos.findIndex((x) => x.id === record.value.id);
  if (index > -1) {
    formData.value.mallProductSkuDtos[index].specParam = JSON.stringify(specParamNameList.value);
    specParamNameList.value = [];
  }
};

const onConfirmAnother = () => {
  visibleNormalDrag.value = false;
};

// 获取分期数
const getNumberOfInstallments = async () => {
  const params = {
    filter: [{ op: 0, propertyName: 'code', value: 'NumberOfInstallmentsType' }],
  };
  numberOfInstallmentsList.value = await getAllList(params);
};

// 选择
const onSelectChange = (value: Array<number>) => {
  selectedRowKeys.value = value;
};

// 选择商品属性
const changeAttr = async (value) => {
  formData.value.mallProductSkuDtos = []
  if (value) {
    const checkList = [];
    attrKeyList.value = attrList.value.filter((x) => value.indexOf(x.id) > -1);
    value.forEach((item) => {
      if (attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item).length > 0) {
        const attr = attrList.value.filter((x) => x.mallProductAttrKeyDto.id == item)[0];
        attr.checkValue = [];
        attrKeyList.value.push(attr);
      }
    });
    attrKeyList.value.forEach((item) => {
      checkList.push(item.mallProductAttrKeyDto.id);
    });
    formData.value.checkList = checkList;
  }
};

// 复选框 option
const getAttrValuesOption = (value) => {
  if (value) {
    return value.map((attr) => ({
      label: attr.attrValueName,
      value: attr.id,
    }));
  }
  return [];
};

const getProductAttr = () => {
  if (!attrList.value || attrList.value.length == 0) {
    return [];
  }
  const jsonArr = [];
  attrList.value
    .filter((x) => x.checkValue && x.checkValue.length > 0)
    .forEach((item) => {
      item.checkValue.forEach((checkItem) => {
        jsonArr.push({ mallProductAttrKeyId: item.mallProductAttrKeyDto.id, mallProductAttrValueId: checkItem });
      });
    });
  return jsonArr;
};

// 自动生成sku
const onChangeAttr = () => {
  // 选中的属性

  attrKeyList.value.forEach((item) => {
    item.checkList = [];
    if (item.checkValue.length > 0) {
      item.checkValue.forEach((checkItem) => {
        const listItem = item.mallProductAttrValueDtos.filter((x) => x.id === checkItem);
        item.checkList.push(listItem[0]);
      });
    }
  });


};
// 自动生成sku
const autoCreateSku = () => {

  formData.value.mallProductSkuDtos = [];
  checkSkuAttrList.value = attrKeyList.value.map((item) => item.checkList);

  if (attrKeyList.value && attrKeyList.value.length > 0) {
    attrKeyList.value.forEach((item) => {
      if (!item.checkList || item.checkList.length == 0) {
        MessagePlugin.error(`属性${item.mallProductAttrKeyDto.attrKeyName}尚未勾选！`);
        return;
      }

    });


    var checkAttrList = attrKeyList.value.filter(x => x.checkList && x.checkList.length > 0);

    let attrKeyArr = []

    for (var i = 0; i < checkAttrList.length; i++) {
      if (i == 0) {
        checkAttrList[i].checkList.forEach(item => {
          attrKeyArr.push([{ 'attrKeyId': item.mallProductAttrKeyId, 'attrValueId': item.id }])
        });
      }
      else {
        var newAttrKeyArr = combin(JSON.parse(JSON.stringify(attrKeyArr)), checkAttrList[i])
        attrKeyArr = [...newAttrKeyArr]
      }

    }
    createSku(attrKeyArr);
  }


};
const combin = (attrKeyArr, checkAttrList) => {
  var newAttrKeyArr = []

  for (var i = 0; i < attrKeyArr.length; i++) {
    checkAttrList.checkList.forEach(checkItem => {
      var newItem = JSON.parse(JSON.stringify(attrKeyArr[i]))
      newItem.push({ 'attrKeyId': checkItem.mallProductAttrKeyId, 'attrValueId': checkItem.id })
      newAttrKeyArr.push(newItem)

    })
  }

  return newAttrKeyArr
}

// 创建sku
const createSku = (attrKeyArr) => {
  if (attrKeyArr && attrKeyArr.length > 0) {
    attrKeyArr.forEach(item => {
      formData.value.mallProductSkuDtos.push({
        id: --count.value,
        skuName: item.map((x) => x.name).join(';'),
        skuPriceAmount: 0,
        skuStock: 0,
        freezeStock: 0,
        status: 1,
        // attrKeyValue: JSON.stringify(attrJson),
        attrKeyValue: JSON.stringify(item),
      });
    })
  }
};

// 形成sku
const sku = () => {
  results.value.forEach((item, index) => {
    formData.value.mallProductSkuDtos.push({
      id: --count.value,
      skuName: item.map((x) => x.name).join(';'),
      skuPriceAmount: 0,
      skuStock: 0,
      freezeStock: 0,
      status: 1,
      // attrKeyValue: JSON.stringify(attrJson),
      attrKeyValue: JSON.stringify(item),
    });
  });
};
const addAttr = (item, index) => {
  const checkList = [];
  const name = item.mallProductAttrValueDtos.filter((x) => x.id === item.id)[0];
  valueList.value[index] = name;
  skuNameList.value = '';
  valueList.value.forEach((x) => {
    if (x) {
      skuNameList.value += `${x.attrValueName},`;
      checkList.push({ attrKeyId: x.attrKeyId, attrValueId: x.id });
    }
  });
  const current = formData.value.mallProductSkuDtos.findIndex((x) => x.id === record.value.id);
  if (index > -1) {
    formData.value.mallProductSkuDtos[current].skuName = skuNameList.value;
    formData.value.mallProductSkuDtos[current].attrKeyValue = JSON.stringify(checkList);
  }
};
// 新增
const toAdd = () => {
  if (!formData.value.mallProductSkuDtos) formData.value.mallProductSkuDtos = [];
  formData.value.mallProductSkuDtos.push({
    id: --count.value,
    specParam: '',
    skuPriceAmount: 0,
    skuStock: 0,
    freezeStock: 0,
    status: 1
  });
  formData.value.mallProductSkuDtos = [...formData.value.mallProductSkuDtos];
};
// 删除
const toDelete = (id) => {
  const mallProductSkuDtos = formData.value.mallProductSkuDtos.filter(
    x => x.id != id
  );
  formData.value.mallProductSkuDtos = [...mallProductSkuDtos];
};
const batchDelete = () => {
  if (selectedRowKeys.value.length > 0) {
    const mallProductSkuDtos = formData.value.mallProductSkuDtos.filter(
      (item) => selectedRowKeys.value.indexOf(item.id) === -1,
    );
    formData.value.mallProductSkuDtos = [...mallProductSkuDtos];
  }
};
// 上传sku图片
const uploadSkuImg = (key, fileUrl) => {
  editImg.value.edit(key, fileUrl, 1)
}
const uploadImgHandleOk = (data) => {
  console.log(data)
  formData.value.mallProductSkuDtos.filter((x) => x.id == data.key)[0].skuImg = data.fileUrl
  // editImg.value.skuTable.loadData()
}
// 图片ref
const productMainImg = ref<any>(null);
const productDetailImg = ref<any>(null);

// 确定
const onSubmit = async ({ firstError }: SubmitContext<Data>) => {
  if (productMainImg.value) {
    await productMainImg.value.uploadImg();
    formData.value.productMainImg = productMainImg.value.getFilesUrl();
  }
  if (productDetailImg.value) {
    await productDetailImg.value.uploadImg();
    formData.value.productDetailImg = productDetailImg.value.getFilesUrl();
  }
  if (!formData.value.productMainImg) {
    MessagePlugin.warning('商品主图不能为空！');
    return;
  }
  formData.value.mallProductAttrDtos = getProductAttr();
  formData.value.mallProductSkuDtos.forEach((item) => {
    item.numberOfInstallments = item.numberOfInstallments.join(',');
  });
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
  productName: [{ required: true, message: '请输入商品名称', type: 'error' }],
  mallProductTypeId: [{ required: true, message: '请选择商品类型', type: 'error' }],
  mallProductCategoryId: [{ required: true, message: '请选择商品目录', type: 'error' }],
  status: [{ required: true, message: '请选择商品状态', type: 'error' }],
};
const productTypeChange = async (typeId) => {
  formData.value.mallProductCategoryId = undefined
  formData.value.checkList = []

  ctgrList.value = await getListByTypeId(typeId)
  attrList.value = []
  formData.value.mallProductSkuDtos = []
  attrKeyList.value.forEach((item) => {
    item.checkValue = []
    item.checkList = []
  });
}

// 根据目录id查找属性相关值
const changeCategory = async (value) => {
  attrList.value = await getAllDetailsById(value);
  formData.value.checkList = []
  formData.value.mallProductSkuDtos = []
  attrKeyList.value.forEach((item) => {
    item.checkValue = []
    item.checkList = []
  });
};

const oriPriceChange = (row) => {
  if (row.status == 1) {
    var maxPrice = row.skuPriceAmount
    var minPrice = row.skuPriceAmount
    formData.value.mallProductSkuDtos.forEach(item => {
      if (item.status == 1) {
        if (item.skuPriceAmount > maxPrice) {
          maxPrice = item.skuPriceAmount
        }
        if (item.skuPriceAmount < minPrice) {
          minPrice = item.skuPriceAmount
        }
      }
    })
    formData.value.maxPriceAmount = maxPrice
    formData.value.minPriceAmount = minPrice
  }
}

const mallAttrFormUpdate = (id) => {
  var checkAttrList = attrKeyList.value.filter(x => x.checkValue && x.checkValue.length > 0);
  mallAttrForm.value.update(id, JSON.parse(JSON.stringify(checkAttrList)))
};
const specParamUpdate = (row) => {
  if (!row.specParam) {
    row.specParam = '[]'
  }
  specParamForm.value.update(row.id, JSON.parse(row.specParam))
};

const selectAttr = (selectAttrData) => {

  var item = formData.value.mallProductSkuDtos.filter(x => x.attrKeyValue == selectAttrData.attr)
  if (item && item[0]) {
    MessagePlugin.error('选择的属性已存在！');
  }
  else {
    var row = formData.value.mallProductSkuDtos.filter(x => x.id == selectAttrData.key)
    if (row && row[0]) {
      row[0].attrKeyValue = selectAttrData.attr
    }
  }
}

const selectSpecParam = (selectData) => {
  var row = formData.value.mallProductSkuDtos.filter(x => x.id == selectData.key)
  if (row && row[0]) {
    row[0].specParam = selectData.attr
  }

}

const handleClickStockInv = (row) => {
  var data: any = {}
  data.skuId = row.id
  data.currentNum = row.skuStock
  data.attrKeyValueName = getSkuName(row.attrKeyValue)
  mallProductInvOrRelForm.value.inv(data)
}

const handleClickStockRel = (row) => {
  var data: any = {}
  data.skuId = row.id
  data.currentNum = row.skuStock
  data.attrKeyValueName = getSkuName(row.attrKeyValue)
  mallProductInvOrRelForm.value.rel(data)
}
const selectProductInvOrRel = (data) => {
  var row = formData.value.mallProductSkuDtos.filter(x => x.id == data.skuId)
  if (row && row[0]) {
    if (data.type == 1) {
      row[0].skuStock = Number(row[0].skuStock) + Number(data.num)
    }
    else {
      row[0].skuStock = Number(row[0].skuStock) - Number(data.num)
    }
  }
}
defineExpose({
  add,
  update,
  copy,
  show
});
</script>

<style scoped>
.specParam {
  display: flex;
  flex-flow: row nowrap;
  justify-content: space-between;
  align-items: center;
}
</style>
