const columnList = [
  {
    "label": "客户",
    "labelI18nCode": "",
    "prop": "F_CustomerId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_CustomerId",
    "fullName": "客户",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "客户",
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
      "tableName": "a_ctc_follow",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2009458830353764352",
      "propsName": "客户",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemb9fad1",
      "renderKey": 1767928027689
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_CustomerName",
      "value": "F_Id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": false,
    "multiple": false,
    "__vModel__": "F_CustomerId"
  },
  {
    "label": "跟单类型",
    "labelI18nCode": "",
    "prop": "F_FollowType",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_FollowType",
    "fullName": "跟单类型",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "跟单类型",
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
      "tableName": "a_ctc_follow",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2009462155446980608",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem229495",
      "renderKey": 1767928134043
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
    "__vModel__": "F_FollowType"
  },
  {
    "label": "联系人",
    "labelI18nCode": "",
    "prop": "F_ContactId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
    "id": "F_ContactId",
    "fullName": "联系人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "联系人",
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
      "tableName": "a_ctc_follow",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem6a7e8e",
      "renderKey": 1767928149159
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2009459664370143232",
    "interfaceName": "联系人",
    "templateJson": [
      {
        "id": "781a4a",
        "field": "Id",
        "dataType": "varchar",
        "defaultValue": "",
        "required": 0,
        "fieldName": "",
        "relationField": "2009181616060108800",
        "sourceType": 2
      }
    ],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_ContactName",
        "label": "姓名"
      },
      {
        "value": "F_ContactPhone",
        "label": "电话"
      }
    ],
    "propsValue": "F_Id",
    "relationField": "F_ContactName",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "800px",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_ContactId",
    "interfaceHasPage": 0
  },
  {
    "label": "跟单内容",
    "labelI18nCode": "",
    "prop": "F_FollowContent",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "textarea",
    "sortable": false,
    "width": null,
    "id": "F_FollowContent",
    "fullName": "跟单内容",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "textarea",
      "label": "跟单内容",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfTextarea",
      "tagIcon": "icon-ym icon-ym-generator-textarea",
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
      "tableName": "a_ctc_follow",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem997659",
      "renderKey": 1767928309371
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
      "minRows": 4,
      "maxRows": 4
    },
    "clearable": true,
    "maxlength": null,
    "showCount": false,
    "readonly": false,
    "disabled": false,
    "__vModel__": "F_FollowContent"
  },
  {
    "label": "下次跟单时间",
    "labelI18nCode": "",
    "prop": "F_NextFollowTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
    "id": "F_NextFollowTime",
    "fullName": "下次跟单时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "下次跟单时间",
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
      "tableName": "a_ctc_follow",
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
      "formId": "formItem81a2a5",
      "renderKey": 1767928322829
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
    "__vModel__": "F_NextFollowTime"
  },
  {
    "label": "跟单状态",
    "labelI18nCode": "",
    "prop": "F_State",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_State",
    "fullName": "跟单状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "跟单状态",
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
      "tableName": "a_ctc_follow",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2015701595244859392",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem64ed3b",
      "renderKey": 1769415745915
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
  }
]

export default columnList