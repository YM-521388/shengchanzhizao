const columnList = [
  {
    "label": "供应商",
    "labelI18nCode": "",
    "prop": "F_SupplierId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_SupplierId",
    "fullName": "供应商",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "供应商",
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
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2008457397298925568",
      "propsName": "仓库供应商",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem188e53",
      "renderKey": 1768528732985
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_SupplierName",
      "value": "F_Id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": false,
    "multiple": false,
    "__vModel__": "F_SupplierId"
  },
  {
    "label": "采购单",
    "labelI18nCode": "",
    "prop": "F_OrderId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_OrderId",
    "fullName": "采购单",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "采购单",
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
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem81003c",
      "renderKey": 1768528740441
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
    "__vModel__": "F_OrderId"
  },
  
  {
    "label": "付款金额",
    "labelI18nCode": "",
    "prop": "F_Money",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_Money",
    "fullName": "付款金额",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "付款金额",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": true,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItemf67535",
      "renderKey": 1768528777688
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
    "disabled": false,
    "__vModel__": "F_Money"
  },
  {
    "label": "付款日期",
    "labelI18nCode": "",
    "prop": "F_PayDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_PayDate",
    "fullName": "付款日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "付款日期",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_pay",
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
      "formId": "formItem919f07",
      "renderKey": 1768528800389
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
    "__vModel__": "F_PayDate"
  },
  {
    "label": "附件",
    "labelI18nCode": "",
    "prop": "F_Files",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "uploadFile",
    "sortable": false,
    "width": null,
    "id": "F_Files",
    "fullName": "附件",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "uploadFile",
      "label": "附件",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem71dd6f",
      "renderKey": 1768528812000
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
    "pathType": "defaultPath",
    "sortRule": [],
    "timeFormat": "YYYY",
    "folder": "",
    "tipText": "",
    "__vModel__": "F_Files"
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
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemdac5cc",
      "renderKey": 1768528815768
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "formId": "formItema16b8c",
      "renderKey": 1768528826611
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_pay",
      "noShow": false,
      "formId": "formItem45cef9",
      "renderKey": 1768528827471
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