const columnList = [
  {
    "label": "生产人员",
    "labelI18nCode": "",
    "prop": "F_ProdUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "userSelect",
    "sortable": false,
    "width": null,
    "id": "F_ProdUserId",
    "fullName": "生产人员",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "生产人员",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUserSelect",
      "tagIcon": "icon-ym icon-ym-generator-user",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": [],
      "defaultCurrent": false,
      "required": true,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem0425dc",
      "renderKey": 1769067564681
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
    "multiple": true,
    "clearable": true,
    "disabled": false,
    "__vModel__": "F_ProdUserId"
  },
  {
    "label": "良品数",
    "labelI18nCode": "",
    "prop": "F_GoodQty",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_GoodQty",
    "fullName": "良品数",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "良品数",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItemb8be59",
      "renderKey": 1769067589889
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "min": 0,
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_GoodQty"
  },
  {
    "label": "不良品项",
    "labelI18nCode": "",
    "prop": "F_CreatorUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_CreatorUserId",
    "fullName": "不良品项",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "不良品项",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2014256316289257472",
      "propsName": "根据工序获取不良品项",
      "useCache": true,
      "templateJson": [
        {
          "id": "a3e186",
          "field": "processId",
          "dataType": "varchar",
          "defaultValue": "",
          "required": 0,
          "fieldName": "",
          "relationField": "F_TaskId",
          "sourceType": 1,
          "jnpfKey": "select"
        }
      ],
      "formId": "formItem72f097",
      "renderKey": 1769067661883
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
      "value": "id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": true,
    "__vModel__": "F_CreatorUserId"
  },
  {
    "label": "开始时间",
    "labelI18nCode": "",
    "prop": "F_StartTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_StartTime",
    "fullName": "开始时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "开始时间",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": true,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
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
      "formId": "formItem3a7c82",
      "renderKey": 1769067665449
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd HH:mm",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_StartTime"
  },
  {
    "label": "结束时间",
    "labelI18nCode": "",
    "prop": "F_EndTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_EndTime",
    "fullName": "结束时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "结束时间",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfDatePicker",
      "tagIcon": "icon-ym icon-ym-generator-date",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": true,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
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
      "formId": "formItem63b0af",
      "renderKey": 1769067666132
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd HH:mm",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_EndTime"
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItemc8742f",
      "renderKey": 1769067672877
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemb0db50",
      "renderKey": 1769067675624
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
      "minRows": 3,
      "maxRows": 3
    },
    "clearable": true,
    "maxlength": 500,
    "showCount": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_Description"
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
      "tableName": "a_prod_report",
      "noShow": true,
      "formId": "formItemdfe45d",
      "renderKey": 1769067679119
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
    "label": "不良品数",
    "labelI18nCode": "",
    "prop": "F_GoodNotQty",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_GoodNotQty",
    "fullName": "不良品数",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "不良品数",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItemadaf25",
      "renderKey": 1769069478536
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "min": 0,
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_GoodNotQty"
  },
  {
    "label": "工资单价",
    "labelI18nCode": "",
    "prop": "F_WagePrice",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "inputNumber",
    "sortable": false,
    "width": null,
    "id": "F_WagePrice",
    "fullName": "工资单价",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "工资单价",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem95767a",
      "renderKey": 1769070110967
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请输入",
    "min": 0,
    "controls": false,
    "addonBefore": "",
    "addonAfter": "",
    "thousands": false,
    "isAmountChinese": false,
    "step": 1,
    "precision": 2,
    "disabled": false,
    "__vModel__": "F_WagePrice"
  },
  {
    "label": "结算状态",
    "labelI18nCode": "",
    "prop": "F_SettleStatus",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_SettleStatus",
    "fullName": "结算状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "结算状态",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "0",
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": true,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2014214471169478656",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem68b4d9",
      "renderKey": 1769069896379
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
    "__vModel__": "F_SettleStatus"
  },
  {
    "label": "结算时间",
    "labelI18nCode": "",
    "prop": "F_SettleTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_SettleTime",
    "fullName": "结算时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "结算时间",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": true,
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
      "formId": "formItem890f04",
      "renderKey": 1769069906194
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "format": "yyyy-MM-dd HH:mm:ss",
    "startTime": null,
    "endTime": null,
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_SettleTime"
  },
  {
    "label": "结算人",
    "labelI18nCode": "",
    "prop": "F_SettleUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "userSelect",
    "sortable": false,
    "width": null,
    "id": "F_SettleUserId",
    "fullName": "结算人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "结算人",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUserSelect",
      "tagIcon": "icon-ym icon-ym-generator-user",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": null,
      "defaultCurrent": false,
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": true,
      "regList": [],
      "trigger": "change",
      "formId": "formItem1abc2f",
      "renderKey": 1769069901896
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
    "__vModel__": "F_SettleUserId"
  },
  {
    "label": "报工类型",
    "labelI18nCode": "",
    "prop": "F_State",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_State",
    "fullName": "报工类型",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "报工类型",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "0",
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2014253420491444224",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItembc1a00",
      "renderKey": 1769071310779
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
    "label": "生产任务",
    "labelI18nCode": "",
    "prop": "F_TaskId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_TaskId",
    "fullName": "生产任务",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "生产任务",
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
      "tableName": "a_prod_report",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2014269102549504000",
      "propsName": "获取生产任务ID",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem97930f",
      "renderKey": 1769074021664
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_Id",
      "value": "F_Id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": false,
    "multiple": false,
    "__vModel__": "F_TaskId"
  }
]

export default columnList