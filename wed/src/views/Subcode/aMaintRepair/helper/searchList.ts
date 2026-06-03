const searchList = [
  {
    "label": "状态",
    "labelI18nCode": "",
    "prop": "F_State",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_maint_repair",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2016473505696190464",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItema1eb1d",
      "renderKey": 1769599517047
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
    "label": "工单编号",
    "labelI18nCode": "",
    "prop": "F_RepairNo",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_RepairNo",
    "fullName": "工单编号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "工单编号",
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
      "tableName": "a_maint_repair",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem1ac98b",
      "renderKey": 1769599344390
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
    "__vModel__": "F_RepairNo"
  },
  {
    "label": "处理人",
    "labelI18nCode": "",
    "prop": "F_HandlerUserId",
    "jnpfKey": "userSelect",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_HandlerUserId",
    "fullName": "处理人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "处理人",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_maint_repair",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem229d8c",
      "renderKey": 1769599373990
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
    "__vModel__": "F_HandlerUserId"
  },
  {
    "label": "报修人",
    "labelI18nCode": "",
    "prop": "F_CreatorUserId",
    "jnpfKey": "createUser",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_CreatorUserId",
    "fullName": "报修人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "createUser",
      "label": "报修人",
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
      "tableName": "a_maint_repair",
      "noShow": true,
      "formId": "formItemff9448",
      "renderKey": 1769599571564
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
    "label": "审核人",
    "labelI18nCode": "",
    "prop": "F_AuditorUserId",
    "jnpfKey": "userSelect",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_AuditorUserId",
    "fullName": "审核人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "审核人",
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
      "tableName": "a_maint_repair",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItemb1489f",
      "renderKey": 1769599544756
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
    "__vModel__": "F_AuditorUserId"
  },
  {
    "label": "是否解决问题",
    "labelI18nCode": "",
    "prop": "F_IsSolved",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_IsSolved",
    "fullName": "是否解决问题",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "是否解决问题",
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
      "tableName": "a_maint_repair",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemdf5c7d",
      "renderKey": 1769599567660
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [
      {
        "fullName": "是",
        "id": "1"
      },
      {
        "fullName": "否",
        "id": "0"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_IsSolved"
  }
]
export default searchList