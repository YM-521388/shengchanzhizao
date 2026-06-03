const columnList = [
  {
    "label": "工序名",
    "labelI18nCode": "",
    "prop": "F_ProcessName",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ProcessName",
    "fullName": "工序名",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "工序名",
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
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemc2f382",
      "renderKey": 1768447270617,
      "unique": true
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
    "__vModel__": "F_ProcessName"
  },
  {
    "label": "工序编号",
    "labelI18nCode": "",
    "prop": "F_ProcessCode",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
    "id": "F_ProcessCode",
    "fullName": "工序编号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "工序编号",
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
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItema7c229",
      "renderKey": 1768447275768
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
    "__vModel__": "F_ProcessCode"
  },
  {
    "label": "计价方式",
    "labelI18nCode": "",
    "prop": "F_PriceType",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_PriceType",
    "fullName": "计价方式",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "计价方式",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSelect",
      "tagIcon": "icon-ym icon-ym-generator-select",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": "0",
      "required": true,
      "layout": "colFormItem",
      "span": 6,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2011642610875240448",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemde1b70",
      "renderKey": 1768447355356
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
    "__vModel__": "F_PriceType"
  },
  {
    "label": "启用状态",
    "labelI18nCode": "",
    "prop": "F_State",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "switch",
    "sortable": false,
    "width": null,
    "id": "F_State",
    "fullName": "启用状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "switch",
      "label": "启用状态",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfSwitch",
      "tagIcon": "icon-ym icon-ym-generator-switch",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": 1,
      "required": false,
      "layout": "colFormItem",
      "span": 6,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItemb7935f",
      "renderKey": 1768448746743
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "disabled": false,
    "activeTxt": "启用",
    "inactiveTxt": "禁用",
    "activeValue": 1,
    "inactiveValue": 0,
    "__vModel__": "F_State"
  },
  {
    "label": "生产人员",
    "labelI18nCode": "",
    "prop": "F_ProdUserIds",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "userSelect",
    "sortable": false,
    "width": null,
    "id": "F_ProdUserIds",
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
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem8ab29f",
      "renderKey": 1768448294270
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
    "__vModel__": "F_ProdUserIds"
  },
  {
    "label": "不良品项",
    "labelI18nCode": "",
    "prop": "F_DefectIds",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "select",
    "sortable": false,
    "width": null,
    "id": "F_DefectIds",
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
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "dictionaryType": "",
      "propsUrl": "2011648481097289728",
      "propsName": "不良品项",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem28744d",
      "renderKey": 1768448305185
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [],
    "props": {
      "label": "F_Name",
      "value": "F_Id"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": true,
    "__vModel__": "F_DefectIds"
  },
  {
    "label": "质检人员",
    "labelI18nCode": "",
    "prop": "F_QcUserIds",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "userSelect",
    "sortable": false,
    "width": null,
    "id": "F_QcUserIds",
    "fullName": "质检人员",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "质检人员",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfUserSelect",
      "tagIcon": "icon-ym icon-ym-generator-user",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "defaultValue": [],
      "defaultCurrent": false,
      "required": false,
      "layout": "colFormItem",
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_process",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItema16021",
      "renderKey": 1768448464775
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
    "__vModel__": "F_QcUserIds"
  }
]

export default columnList