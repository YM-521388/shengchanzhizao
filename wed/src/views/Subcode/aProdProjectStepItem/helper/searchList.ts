const searchList = [
  {
    "label": "项目商品工序",
    "labelI18nCode": "",
    "prop": "F_ProjectStepId",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_ProjectStepId",
    "fullName": "项目商品工序",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "项目商品工序",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project_step_item",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2012092092830060544",
      "propsName": "弹窗获取工序列表",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem5dc3e0",
      "renderKey": 1768890891682
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_ProcessName",
      "value": "id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_ProjectStepId"
  },
  {
    "label": "商品",
    "labelI18nCode": "",
    "prop": "F_GoodsId",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_GoodsId",
    "fullName": "商品",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "商品",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project_step_item",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2012085692393459712",
      "propsName": "弹窗获取商品列表（可传合同ID）",
      "useCache": true,
      "templateJson": [
        {
          "id": "b6217b",
          "field": "contractId",
          "dataType": "varchar",
          "defaultValue": "",
          "required": 0,
          "fieldName": "合同ID",
          "relationField": "",
          "sourceType": 3
        }
      ],
      "formId": "formItem316782",
      "renderKey": 1768890895568
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_GoodsName",
      "value": "id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_GoodsId"
  },
  {
    "label": "数量",
    "labelI18nCode": "",
    "prop": "F_Num",
    "jnpfKey": "inputNumber",
    "value": [],
    "searchType": 3,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_Num",
    "fullName": "数量",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "数量",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project_step_item",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItembe865f",
      "renderKey": 1768890897008
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "min": 1,
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_Num"
  }
]
export default searchList