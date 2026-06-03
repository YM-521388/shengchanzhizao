const searchList = [
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
      "tableName": "a_prod_plan_item",
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
      "formId": "formItem54e5c3",
      "renderKey": 1768911502150
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
    "filterable": false,
    "multiple": false,
    "__vModel__": "F_GoodsId"
  }
]
export default searchList