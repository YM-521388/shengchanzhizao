const searchList = [
  {
    "label": "仓库",
    "labelI18nCode": "",
    "prop": "F_WarehouseId",
    "jnpfKey": "cascader",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_WarehouseId",
    "fullName": "仓库",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "cascader",
      "label": "仓库",
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
      "span": 8,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dynamic",
      "propsUrl": "2021045201115680768",
      "propsName": "仓库管理（树状）",
      "useCache": true,
      "templateJson": [],
      "dictionaryType": "",
      "formId": "formItem23fe2d",
      "renderKey": 1772442673614
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}",
      "blur": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [
      {
        "F_Id": "2021048550619746304",
        "F_ParentId": null,
        "F_Name": "测试储存2级",
        "F_Address": "002",
        "_realParentId": null,
        "children": []
      },
      {
        "F_Id": "2014233789731049472",
        "F_ParentId": null,
        "F_Name": "金家林5G仓",
        "F_Address": "金家林",
        "_realParentId": null,
        "children": [
          {
            "F_Id": "2026473367191818240",
            "F_ParentId": "[\"2014233789731049472\"]",
            "F_Name": "5G仓库2级",
            "F_Address": "001",
            "_realParentId": "2014233789731049472",
            "children": [
              {
                "F_Id": "2026483124422184960",
                "F_ParentId": "[\"2014233789731049472\",\"2026473367191818240\"]",
                "F_Name": "5G仓库3级",
                "F_Address": "测试",
                "_realParentId": "2026473367191818240",
                "children": []
              }
            ]
          }
        ]
      }
    ],
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
    "__vModel__": "F_WarehouseId"
  },
  {
    "label": "其他出库单号",
    "labelI18nCode": "",
    "prop": "F_StockOutNo",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_StockOutNo",
    "fullName": "其他出库单号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "其他出库单号",
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
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItema419e0",
      "renderKey": 1768807922188
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
    "__vModel__": "F_StockOutNo"
  },
  {
    "label": "出库类型",
    "labelI18nCode": "",
    "prop": "F_StockOutType",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": true,
    "isKeyword": false,
    "id": "F_StockOutType",
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
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2013096194263355392",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem111202",
      "renderKey": 1768808049995
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
    "__vModel__": "F_StockOutType"
  },
  {
    "label": "出库日期",
    "labelI18nCode": "",
    "prop": "F_StockOutDate",
    "jnpfKey": "datePicker",
    "value": [],
    "searchType": 3,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_StockOutDate",
    "fullName": "出库日期",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "出库日期",
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
      "tableName": "a_pu_out_stock_other",
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
      "formId": "formItem7ad041",
      "renderKey": 1768808075266
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
    "__vModel__": "F_StockOutDate"
  },
  {
    "label": "创建人员",
    "labelI18nCode": "",
    "prop": "F_CreatorUserId",
    "jnpfKey": "createUser",
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
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
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "formId": "formItem001163",
      "renderKey": 1768808150497
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
    "jnpfKey": "createTime",
    "searchType": 3,
    "searchMultiple": false,
    "isKeyword": false,
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
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "formId": "formItemee6610",
      "renderKey": 1768808154533
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
    "label": "发货人",
    "labelI18nCode": "",
    "prop": "F_StockOutUserId",
    "jnpfKey": "userSelect",
    "searchType": 1,
    "searchMultiple": true,
    "isKeyword": false,
    "id": "F_StockOutUserId",
    "fullName": "发货人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "发货人",
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
      "tableName": "a_pu_out_stock_other",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem5a578b",
      "renderKey": 1768808100154
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
    "__vModel__": "F_StockOutUserId"
  }
]
export default searchList