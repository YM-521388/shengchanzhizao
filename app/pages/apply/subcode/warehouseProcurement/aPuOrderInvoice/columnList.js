const columnList = [
  {
    "label": "采购单",
    "labelI18nCode": "",
    "prop": "F_OrderId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
    "id": "F_OrderId",
    "fullName": "采购单",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "采购单",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem5a34cb",
      "renderKey": 1768530537250
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
        "label": "采购单编号"
      },
      {
        "value": "F_SupplierId",
        "label": "供应商"
      },
      {
        "value": "F_ProdPlanId",
        "label": "生成计划名"
      },
      {
        "value": "F_Money",
        "label": "金额"
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
      "required": true,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2008457397298925568",
      "propsName": "仓库供应商",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemcc4db2",
      "renderKey": 1768530764120
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
    "label": "发票金额",
    "labelI18nCode": "",
    "prop": "F_Money",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_Money",
    "fullName": "发票金额",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "发票金额",
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
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem7f405e",
      "renderKey": 1768530816807
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
    "label": "开票日期",
    "labelI18nCode": "",
    "prop": "F_InvoiceDate",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_InvoiceDate",
    "fullName": "开票日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "开票日期",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": false,
      "required": true,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_order_invoice",
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
      "formId": "formItemcf227d",
      "renderKey": 1768530830180
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
    "__vModel__": "F_InvoiceDate"
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
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItemefbc54",
      "renderKey": 1768530836285
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
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem4acde0",
      "renderKey": 1768530839589
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
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "formId": "formItemcbd979",
      "renderKey": 1768530852231
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
      "tableName": "a_pu_order_invoice",
      "noShow": false,
      "formId": "formItem543ef4",
      "renderKey": 1768530852729
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