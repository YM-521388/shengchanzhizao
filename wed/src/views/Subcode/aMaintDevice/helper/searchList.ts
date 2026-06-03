const searchList = [
  {
    "label": "设备编号",
    "labelI18nCode": "",
    "prop": "F_DeviceNo",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_DeviceNo",
    "fullName": "设备编号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "设备编号",
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
      "tableName": "a_maint_device",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItema3d7da",
      "renderKey": 1769570420616
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
    "__vModel__": "F_DeviceNo"
  },
  {
    "label": "设备名称",
    "labelI18nCode": "",
    "prop": "F_DeviceName",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_DeviceName",
    "fullName": "设备名称",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "设备名称",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
      "tableAlign": "left",
      "tableFixed": "none",
      "className": [],
      "required": true,
      "layout": "colFormItem",
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_maint_device",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem751038",
      "renderKey": 1769570422757
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
    "__vModel__": "F_DeviceName"
  },
  {
    "label": "设备状态",
    "labelI18nCode": "",
    "prop": "F_DeviceStatus",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_DeviceStatus",
    "fullName": "设备状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "设备状态",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_maint_device",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2011620395194650624",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem250f93",
      "renderKey": 1769570430016
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [
      {
        "id": "2011624015319404544",
        "fullName": "正常",
        "enCode": "0",
        "sortCode": 0
      },
      {
        "id": "2011624046621495296",
        "fullName": "维修中",
        "enCode": "1",
        "sortCode": 1
      }
    ],
    "props": {
      "label": "fullName",
      "value": "enCode"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": true,
    "multiple": false,
    "__vModel__": "F_DeviceStatus"
  },
  {
    "label": "设备类别",
    "labelI18nCode": "",
    "prop": "F_DeviceType",
    "jnpfKey": "cascader",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_DeviceType",
    "fullName": "设备类别",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "cascader",
      "label": "设备类别",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_maint_device",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "propsUrl": "2011621811959238656",
      "propsName": "机台类别",
      "useCache": true,
      "templateJson": [],
      "dictionaryType": "",
      "formId": "formItem59fc76",
      "renderKey": 1769570435859
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
    "__vModel__": "F_DeviceType"
  }
]
export default searchList