const columnList = [
  {
    "label": "项目编号",
    "labelI18nCode": "",
    "prop": "F_ProjectNo",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ProjectNo",
    "fullName": "项目编号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "项目编号",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": false,
      "layout": "colFormItem",
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem996c8b",
      "renderKey": 1768812525863
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请填写，忽略将自动生成",
    "useScan": false,
    "useMask": false,
    "maskConfig": {
      "filler": "*",
      "maskType": 1,
      "prefixType": 1,
      "prefixLimit": 0,
      "prefixSpecifyChar": "",
      "suffixType": 1,
      "suffixLimit": 0,
      "suffixSpecifyChar": "",
      "ignoreChar": "",
      "useUnrealMask": false,
      "unrealMaskLength": 1
    },
    "clearable": true,
    "addonBefore": "",
    "addonAfter": "",
    "prefixIcon": "",
    "suffixIcon": "",
    "maxlength": null,
    "showCount": false,
    "showPassword": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_ProjectNo"
  },
  {
    "label": "项目名称",
    "labelI18nCode": "",
    "prop": "F_ProjectName",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ProjectName",
    "fullName": "项目名称",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "项目名称",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": true,
      "layout": "colFormItem",
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem9af91a",
      "renderKey": 1768812526450
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "useScan": false,
    "useMask": false,
    "maskConfig": {
      "filler": "*",
      "maskType": 1,
      "prefixType": 1,
      "prefixLimit": 0,
      "prefixSpecifyChar": "",
      "suffixType": 1,
      "suffixLimit": 0,
      "suffixSpecifyChar": "",
      "ignoreChar": "",
      "useUnrealMask": false,
      "unrealMaskLength": 1
    },
    "clearable": true,
    "addonBefore": "",
    "addonAfter": "",
    "prefixIcon": "",
    "suffixIcon": "",
    "maxlength": null,
    "showCount": false,
    "showPassword": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_ProjectName"
  },
  {
    "label": "项目类别",
    "labelI18nCode": "",
    "prop": "F_ProjectType",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_ProjectType",
    "fullName": "项目类别",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "项目类别",
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
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013172950349516800",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem22bb6a",
      "renderKey": 1768812562386
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "fullName",
      "value": "enCode"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_ProjectType"
  },
  {
    "label": "项目状态",
    "labelI18nCode": "",
    "prop": "F_Status",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_Status",
    "fullName": "项目状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "项目状态",
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
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013173013452820480",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemda3810",
      "renderKey": 1768812565426
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "fullName",
      "value": "enCode"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_Status"
  },
  {
    "label": "创建人员",
    "labelI18nCode": "",
    "prop": "F_CreatorUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "createUser",
    "sortable": false,
    "width": null,
    "id": "F_CreatorUserId",
    "fullName": "创建人员",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "createUser",
      "label": "创建人员",
      "showLabel": true,
      "tag": "JnpfOpenData",
      "tagIcon": "icon-ym icon-ym-generator-founder",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "required": false,
      "layout": "colFormItem",
      "span": 6,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": true,
      "formId": "formItem54e7dd",
      "renderKey": 1768812568076
    },
    "style": {
      "width": "100%"
    },
    "type": "currUser",
    "readonly": true,
    "placeholder": "",
    "__vModel__": "F_CreatorUserId"
  },
  {
    "label": "创建时间",
    "labelI18nCode": "",
    "prop": "F_CreatorTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "createTime",
    "sortable": false,
    "width": null,
    "id": "F_CreatorTime",
    "fullName": "创建时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "createTime",
      "label": "创建时间",
      "showLabel": true,
      "tag": "JnpfOpenData",
      "tagIcon": "icon-ym icon-ym-generator-createtime",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "layout": "colFormItem",
      "required": false,
      "span": 6,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": true,
      "formId": "formItem8a3a71",
      "renderKey": 1768812568450
    },
    "style": {
      "width": "100%"
    },
    "type": "currTime",
    "readonly": true,
    "placeholder": "",
    "__vModel__": "F_CreatorTime"
  },
  {
    "label": "修改时间",
    "labelI18nCode": "",
    "prop": "F_UpdateTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "modifyTime",
    "sortable": false,
    "width": null,
    "id": "F_UpdateTime",
    "fullName": "修改时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "modifyTime",
      "label": "修改时间",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-modifytime",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "required": false,
      "layout": "colFormItem",
      "span": 6,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": true,
      "formId": "formItemf171c3",
      "renderKey": 1768812569223
    },
    "style": {
      "width": "100%"
    },
    "readonly": true,
    "placeholder": "系统自动生成",
    "__vModel__": "F_UpdateTime"
  },
  {
    "label": "修改人员",
    "labelI18nCode": "",
    "prop": "F_UpdateUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
    "id": "F_UpdateUserId",
    "fullName": "修改人员",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "修改人员",
      "tipLabel": "",
      "showLabel": true,
      "required": false,
      "tag": "JnpfPopupSelect",
      "tagIcon": "icon-ym icon-ym-generator-popup",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_project",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem8e9d2f",
      "renderKey": 1769135571925
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2012085692393459712",
    "interfaceName": "弹窗获取商品列表（可传合同ID）",
    "templateJson": [
      {
        "id": "b6217b",
        "field": "contractId",
        "dataType": "varchar",
        "defaultValue": "",
        "required": 0,
        "fieldName": "合同ID",
        "relationField": "F_ContractId",
        "sourceType": 1,
        "jnpfKey": "select"
      }
    ],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_GoodsNo",
        "label": "商品编号"
      },
      {
        "value": "F_GoodsName",
        "label": "商品名称"
      }
    ],
    "propsValue": "id",
    "relationField": "F_GoodsName",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "800px",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_UpdateUserId",
    "interfaceHasPage": 0
  }
]

export default columnList