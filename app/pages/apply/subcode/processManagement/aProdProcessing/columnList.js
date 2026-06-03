const columnList = [
  {
    "label": "加工单号",
    "labelI18nCode": "",
    "prop": "F_ProcessNo",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ProcessNo",
    "fullName": "加工单号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "加工单号",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem58c1b4",
      "renderKey": 1768965791541
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
    "__vModel__": "F_ProcessNo"
  },
  {
    "label": "商品",
    "labelI18nCode": "",
    "prop": "F_GoodsId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
    "id": "F_GoodsId",
    "fullName": "商品",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "商品",
      "tipLabel": "",
      "showLabel": true,
      "required": true,
      "tag": "JnpfPopupSelect",
      "tagIcon": "icon-ym icon-ym-generator-popup",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem1c701c",
      "renderKey": 1768965800513
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
        "relationField": "",
        "sourceType": 3
      }
    ],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_GoodsName",
        "label": "商品名"
      },
      {
        "value": "F_GoodsNo",
        "label": "商品编号"
      },
      {
        "value": "F_Specification",
        "label": "商品规格"
      },
      {
        "value": "F_CategoryId",
        "label": "商品类别"
      },
      {
        "value": "F_Unit",
        "label": "单位"
      },
      {
        "value": "F_InspectRule",
        "label": "检验规范"
      },
      {
        "value": "F_Remark",
        "label": "备注"
      }
    ],
    "propsValue": "id",
    "relationField": "F_GoodsName",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "1000px",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_GoodsId",
    "interfaceHasPage": 0
  },
  {
    "label": "计划数",
    "labelI18nCode": "",
    "prop": "F_PlanQty",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_PlanQty",
    "fullName": "计划数",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "计划数",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem0684e1",
      "renderKey": 1768965820882
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
    "__vModel__": "F_PlanQty"
  },
  {
    "label": "状态",
    "labelI18nCode": "",
    "prop": "F_State",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_State",
    "fullName": "状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "状态",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": true,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013819135649255424",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem19f927",
      "renderKey": 1768967117997
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
    "__vModel__": "F_State"
  },
  {
    "label": "交货日期",
    "labelI18nCode": "",
    "prop": "F_DeliveryDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_DeliveryDate",
    "fullName": "交货日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "交货日期",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": false,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "startTimeRule": false,
      "startTimeType": 1,
      "startTimeTarget": 1,
      "startTimeValue": null,
      "startRelationField": "",
      "endTimeRule": false,
      "endTimeType": 1,
      "endTimeTarget": 1,
      "endTimeValue": null,
      "endRelationField": "",
      "formId": "formItem36fe16",
      "renderKey": 1768965822279
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_DeliveryDate"
  },
  {
    "label": "优先级",
    "labelI18nCode": "",
    "prop": "F_Priority",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_Priority",
    "fullName": "优先级",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "优先级",
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
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013182853290004480",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem3492d8",
      "renderKey": 1768965825289
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
    "filterable": false,
    "multiple": false,
    "__vModel__": "F_Priority"
  },
  {
    "label": "计划开始",
    "labelI18nCode": "",
    "prop": "F_PlanStartDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_PlanStartDate",
    "fullName": "计划开始",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "计划开始",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": false,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "startTimeRule": false,
      "startTimeType": 1,
      "startTimeTarget": 1,
      "startTimeValue": null,
      "startRelationField": "",
      "endTimeRule": false,
      "endTimeType": 1,
      "endTimeTarget": 1,
      "endTimeValue": null,
      "endRelationField": "",
      "formId": "formItem390b0f",
      "renderKey": 1768965828597
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_PlanStartDate"
  },
  {
    "label": "计划结束",
    "labelI18nCode": "",
    "prop": "F_PlanEndDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_PlanEndDate",
    "fullName": "计划结束",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "计划结束",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": false,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "startTimeRule": false,
      "startTimeType": 1,
      "startTimeTarget": 1,
      "startTimeValue": null,
      "startRelationField": "",
      "endTimeRule": false,
      "endTimeType": 1,
      "endTimeTarget": 1,
      "endTimeValue": null,
      "endRelationField": "",
      "formId": "formItem6b9034",
      "renderKey": 1768965828821
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_PlanEndDate"
  },
  {
    "label": "图纸",
    "labelI18nCode": "",
    "prop": "F_DrawingFile",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "uploadFile",
    "sortable": false,
    "width": null,
    "id": "F_DrawingFile",
    "fullName": "图纸",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "uploadFile",
      "label": "图纸",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUploadFile",
      "tagIcon": "icon-ym icon-ym-generator-upload",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": [],
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItembeedf8",
      "renderKey": 1768965834509
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "disabled": false,
    "accept": "",
    "fileSize": 10,
    "sizeUnit": "MB",
    "buttonText": "点击上传",
    "limit": 9,
    "pathType": "selfPath",
    "sortRule": [
      "1",
      "2"
    ],
    "timeFormat": "YYYYMMDD",
    "folder": "",
    "tipText": "",
    "__vModel__": "F_DrawingFile"
  },
  {
    "label": "客户",
    "labelI18nCode": "",
    "prop": "F_CustomerName",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_CustomerName",
    "fullName": "客户",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "客户",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem77740c",
      "renderKey": 1768965847106
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
    "__vModel__": "F_CustomerName"
  },
  {
    "label": "类别",
    "labelI18nCode": "",
    "prop": "F_Type",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "cascader",
    "sortable": false,
    "width": null,
    "id": "F_Type",
    "fullName": "类别",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "cascader",
      "label": "类别",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfCascader",
      "tagIcon": "icon-ym icon-ym-generator-cascader",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": [],
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "propsUrl": "2008414575283802112",
      "propsName": "设备管理类别",
      "useCache": true,
      "templateJson": [],
      "dictionaryType": "",
      "formId": "formIteme77d77",
      "renderKey": 1768965854736
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "value": "F_Id",
      "label": "F_Name",
      "children": "children"
    },
    "placeholder": "请选择",
    "disabled": false,
    "clearable": true,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_Type"
  },
  {
    "label": "BOM图片",
    "labelI18nCode": "",
    "prop": "F_BOMImages",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "uploadImg",
    "sortable": false,
    "width": null,
    "id": "F_BOMImages",
    "fullName": "BOM图片",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "uploadImg",
      "label": "BOM图片",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUploadImg",
      "tagIcon": "icon-ym icon-ym-generator-upload",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": [],
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem4feab6",
      "renderKey": 1768965859851
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "disabled": false,
    "fileSize": 10,
    "sizeUnit": "MB",
    "buttonText": "点击上传",
    "limit": 9,
    "pathType": "selfPath",
    "sortRule": [
      "1",
      "2"
    ],
    "timeFormat": "YYYYMMDD",
    "folder": "",
    "tipText": "",
    "showType": "button",
    "__vModel__": "F_BOMImages"
  },
  {
    "label": "BOM",
    "labelI18nCode": "",
    "prop": "F_BOMId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_BOMId",
    "fullName": "BOM",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "BOM",
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
      "tableName": "a_prod_processing",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2013188646957617152",
      "propsName": "获取BOM列表（可传商品ID）",
      "useCache": true,
      "templateJson": [
        {
          "id": "b6217b",
          "field": "goodsId",
          "dataType": "varchar",
          "defaultValue": "",
          "required": 0,
          "fieldName": "合同ID",
          "relationField": "F_GoodsId",
          "sourceType": 1,
          "jnpfKey": "popupSelect"
        }
      ],
      "formId": "formItem2e0820",
      "renderKey": 1768965818305
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_BomName",
      "value": "id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_BOMId"
  }
]

export default columnList