const searchList = [
  {
    "label": "外协类型",
    "labelI18nCode": "",
    "prop": "F_OutsourceType",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_OutsourceType",
    "fullName": "外协类型",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "外协类型",
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
      "tableName": "a_prod_outsource_accept",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem1b988f",
      "renderKey": 1769052103016
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
    "__vModel__": "F_OutsourceType"
  },
  {
    "label": "结算状态",
    "labelI18nCode": "",
    "prop": "F_SettleStatus",
    "jnpfKey": "radio",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_SettleStatus",
    "fullName": "结算状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "结算状态",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_outsource_accept",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2014214471169478656",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemd41808",
      "renderKey": 1769061076327
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [
      {
        "id": "2014214738782851072",
        "fullName": "未结算",
        "enCode": "0",
        "sortCode": 0
      },
      {
        "id": "2014214806168539136",
        "fullName": "已结算",
        "enCode": "1",
        "sortCode": 1
      }
    ],
    "props": {
      "label": "fullName",
      "value": "enCode"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_SettleStatus"
  },
  {
    "label": "结算时间",
    "labelI18nCode": "",
    "prop": "F_SettleTime",
    "jnpfKey": "datePicker",
    "value": [],
    "searchType": 3,
    "searchMultiple": false,
    "isKeyword": false,
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
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_outsource_accept",
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
      "formId": "formItem4c7c4d",
      "renderKey": 1769052141513
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
    "__vModel__": "F_SettleTime"
  },
  {
    "label": "结算人",
    "labelI18nCode": "",
    "prop": "F_SettleUserId",
    "jnpfKey": "userSelect",
    "searchType": 1,
    "searchMultiple": true,
    "isKeyword": false,
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
      "defaultValue": "",
      "defaultCurrent": true,
      "required": false,
      "layout": "colFormItem",
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_outsource_accept",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItemcc7faf",
      "renderKey": 1769052135125
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
  }
]
export default searchList