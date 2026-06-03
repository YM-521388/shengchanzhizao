const columnList = [
  {
    "label": "采购退货单号",
    "labelI18nCode": "",
    "prop": "F_ReturnOutNo",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ReturnOutNo",
    "fullName": "采购退货单号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "采购退货单号",
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
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemb4e5cc",
      "renderKey": 1768914565217
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
    "__vModel__": "F_ReturnOutNo"
  },
  {
    "label": "出库类型",
    "labelI18nCode": "",
    "prop": "F_ReturnOutType",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_ReturnOutType",
    "fullName": "出库类型",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "出库类型",
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
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013096194263355392",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem76e1f2",
      "renderKey": 1768914788009
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
    "__vModel__": "F_ReturnOutType"
  },
  {
    "label": "采购单号",
    "labelI18nCode": "",
    "prop": "F_OrderId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
    "id": "F_OrderId",
    "fullName": "采购单号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "采购单号",
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
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItemdc4ec6",
      "renderKey": 1768915130582
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2011984543933927424",
    "interfaceName": "采购单",
    "templateJson": [],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_OrderNo",
        "label": "采购编号"
      },
      {
        "value": "F_SupplierId",
        "label": "供应商"
      },
      {
        "value": "F_ProdPlanId",
        "label": "生产计划"
      },
      {
        "value": "F_Money",
        "label": "商品金额"
      },
      {
        "value": "F_PurchaseNum",
        "label": "采购数量"
      },
      {
        "value": "F_DeliveryDate",
        "label": "交货日期"
      },
      {
        "value": "F_Description",
        "label": "备注"
      }
    ],
    "propsValue": "F_Id",
    "relationField": "F_OrderNo",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "70%",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_OrderId",
    "interfaceHasPage": 0
  },
  {
    "label": "退货日期",
    "labelI18nCode": "",
    "prop": "F_ReturnOutDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_ReturnOutDate",
    "fullName": "退货日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "退货日期",
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
      "tableName": "a_pu_out_return_pu",
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
      "formId": "formItembe2f0a",
      "renderKey": 1768915221540
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
    "__vModel__": "F_ReturnOutDate"
  },
  {
    "label": "备注",
    "labelI18nCode": "",
    "prop": "F_Description",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "textarea",
    "sortable": false,
    "width": null,
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
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemfa00eb",
      "renderKey": 1768915241725
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
    "showCount": true,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_Description"
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "formId": "formItem7f64d9",
      "renderKey": 1768915261705
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_out_return_pu",
      "noShow": false,
      "formId": "formItemc2f186",
      "renderKey": 1768915262687
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

export default columnList