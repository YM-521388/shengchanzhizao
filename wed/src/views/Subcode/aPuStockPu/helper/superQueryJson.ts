const superQueryJson = [
  {
    "id": "F_StockInNo",
    "fullName": "采购入库单号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "采购入库单号",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemccf20c",
      "renderKey": 1768550855231
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
    "__vModel__": "F_StockInNo"
  },
  // {
  //   "id": "F_WarehouseId",
  //   "fullName": "入库仓库",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "select",
  //     "label": "入库仓库",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfSelect",
  //     "tagIcon": "icon-ym icon-ym-generator-select",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "defaultValue": "",
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 8,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_pu_stock_pu",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "dynamic",
  //     "dictionaryType": "",
  //     "propsUrl": "2012073572784279552",
  //     "propsName": "入库仓库",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem083212",
  //     "renderKey": 1768550870159
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [],
  //   "props": {
  //     "label": "F_Name",
  //     "value": "F_Id"
  //   },
  //   "placeholder": "请选择",
  //   "clearable": true,
  //   "disabled": false,
  //   "filterable": false,
  //   "multiple": false,
  //   "__vModel__": "F_WarehouseId"
  // },
  {
    "id": "F_StockInType",
    "fullName": "入库类型",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "入库类型",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2012074650393251840",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItembe82c3",
      "renderKey": 1768550902973
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
    "__vModel__": "F_StockInType"
  },
  {
    "id": "F_StockInDate",
    "fullName": "入库日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "入库日期",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": true,
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
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
      "formId": "formItemef5d0a",
      "renderKey": 1768550998521
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
    "__vModel__": "F_StockInDate"
  },
  {
    "id": "F_StockInUserId",
    "fullName": "收货人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "收货人",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUserSelect",
      "tagIcon": "icon-ym icon-ym-generator-user",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "",
      "defaultCurrent": true,
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem6e284d",
      "renderKey": 1768551989729
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "selectType": "all",
    "ableIds": [],
    "ableRelationIds": [],
    "relationField": "",
    "multiple": false,
    "clearable": true,
    "disabled": false,
    "__vModel__": "F_StockInUserId"
  },
  {
    "id": "F_Description",
    "fullName": "备注",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "textarea",
      "label": "备注",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfTextarea",
      "tagIcon": "icon-ym icon-ym-generator-textarea",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem856379",
      "renderKey": 1768552011631
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "autoSize": {
      "minRows": 2,
      "maxRows": 4
    },
    "clearable": true,
    "maxlength": null,
    "showCount": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_Description"
  },
  {
    "id": "tableField4bd139-F_CustomerId",
    "fullName": "采购入库商品-商品",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "商品",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem95b2ca",
      "renderKey": 1768552081956,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2008721559174385664",
    "interfaceName": "商品",
    "templateJson": [],
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
      },
      {
        "value": "F_Unit",
        "label": "单位"
      },
      {
        "value": "F_Specification",
        "label": "规格"
      },
      {
        "value": "F_SalePrice",
        "label": "销售单价"
      },
      {
        "value": "F_CostPrice",
        "label": "成本单价"
      },
      {
        "value": "F_Image",
        "label": "图片"
      },
      {
        "value": "F_Source",
        "label": "商品来源"
      },
      {
        "value": "F_Remark",
        "label": "备注"
      }
    ],
    "propsValue": "F_Id",
    "relationField": "F_GoodsNo",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "70%",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_CustomerId",
    "interfaceHasPage": 0
  },
  {
    "id": "tableField4bd139-F_Num",
    "fullName": "采购入库商品-数量",
    "fullNameI18nCode": [
      "",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem861a9e",
      "renderKey": 1768552368110,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_Num"
  },
  {
    "id": "tableField4bd139-F_Price",
    "fullName": "采购入库商品-成本单价(元)",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "成本单价(元)",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formIteme505e3",
      "renderKey": 1768552385152,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": null,
    "disabled": false,
    "__vModel__": "F_Price"
  },
  {
    "id": "tableField4bd139-F_Description",
    "fullName": "采购入库商品-备注",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "textarea",
      "label": "备注",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfTextarea",
      "tagIcon": "icon-ym icon-ym-generator-textarea",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem466010",
      "renderKey": 1768552404657,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "autoSize": {
      "minRows": 1,
      "maxRows": 4
    },
    "clearable": true,
    "maxlength": null,
    "showCount": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_Description"
  },
  {
    "id": "tableField4bd139-F_CreatorUserId",
    "fullName": "采购入库商品-创建人员",
    "fullNameI18nCode": [
      "",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item",
      "formId": "formItem7e0aee",
      "renderKey": 1768552425928
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
    "id": "tableField4bd139-F_CreatorTime",
    "fullName": "采购入库商品-创建时间",
    "fullNameI18nCode": [
      "",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableField4bd139",
      "relationTable": "a_pu_stock_pu_item",
      "formId": "formItem7c688a",
      "renderKey": 1768552430234
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
    "id": "tableFieldca527d-F_Type",
    "fullName": "采购入库日志-类型",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "类型",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem0390eb",
      "renderKey": 1768552469814,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log"
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
    "__vModel__": "F_Type"
  },
  {
    "id": "tableFieldca527d-F_Title",
    "fullName": "采购入库日志-标题",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "标题",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem529ba9",
      "renderKey": 1768552170848,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log"
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
    "__vModel__": "F_Title"
  },
  {
    "id": "tableFieldca527d-F_Content",
    "fullName": "采购入库日志-内容",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "内容",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemb8c2d2",
      "renderKey": 1768552497501,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log"
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
    "__vModel__": "F_Content"
  },
  {
    "id": "tableFieldca527d-F_Reason",
    "fullName": "采购入库日志-修改原因",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "textarea",
      "label": "修改原因",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfTextarea",
      "tagIcon": "icon-ym icon-ym-generator-textarea",
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
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem5f756d",
      "renderKey": 1768552501888,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "autoSize": {
      "minRows": 1,
      "maxRows": 4
    },
    "clearable": true,
    "maxlength": null,
    "showCount": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_Reason"
  },
  {
    "id": "tableFieldca527d-F_CreatorUserId",
    "fullName": "采购入库日志-创建人员",
    "fullNameI18nCode": [
      "",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log",
      "formId": "formItemfe82b3",
      "renderKey": 1768552516105
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
    "id": "tableFieldca527d-F_CreatorTime",
    "fullName": "采购入库日志-创建时间",
    "fullNameI18nCode": [
      "",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_stock_pu",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableFieldca527d",
      "relationTable": "a_pu_stock_pu_log",
      "formId": "formItem05eb25",
      "renderKey": 1768552519713
    },
    "style": {
      "width": "100%"
    },
    "type": "currTime",
    "readonly": true,
    "placeholder": "",
    "__vModel__": "F_CreatorTime"
  }
]
export default superQueryJson