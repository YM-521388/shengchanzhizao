const superQueryJson = [
  {
    "id": "F_ProdPlanId",
    "fullName": "生产计划",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "生产计划",
      "tipLabel": "",
      "showLabel": true,
      "required": false,
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem378825",
      "renderKey": 1769132129843
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2014512148058869760",
    "interfaceName": "生产计划",
    "templateJson": [],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_PlanNo",
        "label": "计划编号"
      },
      {
        "value": "F_PlanName",
        "label": "计划名称"
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
    "relationField": "F_PlanNo",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "50%",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_ProdPlanId",
    "interfaceHasPage": 0
  },
  {
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
      "span": 16,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem239980",
      "renderKey": 1769131613277
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
    "id": "F_ConsiderMainStock",
    "fullName": "是否考虑主商品库存",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑主商品库存",
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem73814e",
      "renderKey": 1769132363141
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_ConsiderMainStock"
  },
  {
    "id": "F_ConsiderMaterialStock",
    "fullName": "是否考虑物料库存",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑物料库存",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem0dc424",
      "renderKey": 1769132526734
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_ConsiderMaterialStock"
  },
  {
    "id": "F_ConsiderMaterialOccupy",
    "fullName": "是否考虑物料占用",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑物料占用",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem9b1787",
      "renderKey": 1769132413330
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_ConsiderMaterialOccupy"
  },
  {
    "id": "F_RoundUpQty",
    "fullName": "是否考虑数量向上取整",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑数量向上取整",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem03e0b3",
      "renderKey": 1769132547287
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_RoundUpQty"
  },
  {
    "id": "F_ConsiderWipGoods",
    "fullName": "是否考虑在制商品",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑在制商品",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItem7c14e0",
      "renderKey": 1769132493192
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_ConsiderWipGoods"
  },
  {
    "id": "F_ConsiderTransitGoods",
    "fullName": "是否考虑在途商品",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "是否考虑在途商品",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemeda625",
      "renderKey": 1769132563931
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_ConsiderTransitGoods"
  },
  {
    "id": "F_OccupyAllowOtherOut",
    "fullName": "占用物料能否被其他单据出库",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "radio",
      "label": "占用物料能否被其他单据出库",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfRadio",
      "tagIcon": "icon-ym icon-ym-generator-radio",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "static",
      "dictionaryType": "",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formIteme68bec",
      "renderKey": 1769132510606
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
        "id": "2"
      }
    ],
    "props": {
      "label": "fullName",
      "value": "id"
    },
    "direction": "horizontal",
    "optionType": "default",
    "buttonStyle": "solid",
    "size": "default",
    "disabled": false,
    "__vModel__": "F_OccupyAllowOtherOut"
  },
  {
    "id": "F_AnalysisUserId",
    "fullName": "分析人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "userSelect",
      "label": "分析人",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItema3a77f",
      "renderKey": 1769132597360
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
    "__vModel__": "F_AnalysisUserId"
  },
  {
    "id": "F_AnalysisTime",
    "fullName": "分析时间",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "datePicker",
      "label": "分析时间",
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
      "span": 12,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_prod_analysis",
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
      "formId": "formItem2d1c15",
      "renderKey": 1769132613270
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
    "__vModel__": "F_AnalysisTime"
  },
  {
    "id": "F_State",
    "fullName": "状态",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "状态",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemdbd99e",
      "renderKey": 1769132625546
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
    "__vModel__": "F_State"
  },
  {
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "formId": "formItem74c93e",
      "renderKey": 1769132659073
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
    "id": "F_CreatorUserId",
    "fullName": "创建用户",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "createUser",
      "label": "创建用户",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "formId": "formItemff7089",
      "renderKey": 1769132689646
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
    "id": "tableField4d57ab-F_ParentId",
    "fullName": "物料分析商品列表信息-商品上级",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "商品上级",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem8d2e26",
      "renderKey": 1769133579757,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "__vModel__": "F_ParentId"
  },
  {
    "id": "tableField4d57ab-F_GoodsId",
    "fullName": "物料分析商品列表信息-商品",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "popupSelect",
      "label": "商品",
      "tipLabel": "",
      "showLabel": true,
      "required": false,
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "transferList": [],
      "useCache": true,
      "formId": "formItem3bebdf",
      "renderKey": 1769133373929,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "interfaceId": "2008721559174385664",
    "interfaceName": "商品",
    "templateJson": [],
    "hasPage": true,
    "pageSize": 20,
    "extraOptions": [],
    "columnOptions": [
      {
        "value": "F_GoodsName",
        "label": "商品名称"
      },
      {
        "value": "F_GoodsNo",
        "label": "商品编号"
      },
      {
        "value": "F_Unit",
        "label": "单位"
      },
      {
        "value": "F_SalePrice",
        "label": "销售单价"
      },
      {
        "value": "F_CostPrice",
        "label": "成本单价"
      },
      {
        "value": "F_Source",
        "label": "商品来源"
      },
      {
        "value": "F_Remark",
        "label": "备注"
      }
    ],
    "propsValue": "F_Id",
    "relationField": "F_GoodsName",
    "popupType": "dialog",
    "popupTitle": "选择数据",
    "popupWidth": "60%",
    "disabled": false,
    "clearable": true,
    "__vModel__": "F_GoodsId",
    "interfaceHasPage": 0
  },
  {
    "id": "tableField4d57ab-F_Unit",
    "fullName": "物料分析商品列表信息-单位用量",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "单位用量",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem1cdf22",
      "renderKey": 1769133600550,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_Unit"
  },
  {
    "id": "tableField4d57ab-F_DemandQty",
    "fullName": "物料分析商品列表信息-需求",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "需求",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem7ded42",
      "renderKey": 1769133611190,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_DemandQty"
  },
  {
    "id": "tableField4d57ab-F_AvailableStock",
    "fullName": "物料分析商品列表信息-可用库存",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "可用库存",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem349229",
      "renderKey": 1769133615741,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_AvailableStock"
  },
  {
    "id": "tableField4d57ab-F_WipStock",
    "fullName": "物料分析商品列表信息-在制库存",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "在制库存",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem5ece7f",
      "renderKey": 1769133621112,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_WipStock"
  },
  {
    "id": "tableField4d57ab-F_TransitStock",
    "fullName": "物料分析商品列表信息-在途库存",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "在途库存",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItembd7a7b",
      "renderKey": 1769133624742,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_TransitStock"
  },
  {
    "id": "tableField4d57ab-F_ShouldSelfMake",
    "fullName": "物料分析商品列表信息-应自产",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "应自产",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem88cd10",
      "renderKey": 1769133636487,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_ShouldSelfMake"
  },
  {
    "id": "tableField4d57ab-F_ShouldOutsource",
    "fullName": "物料分析商品列表信息-应外协",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "应外协",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formIteme4b37d",
      "renderKey": 1769133645378,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_ShouldOutsource"
  },
  {
    "id": "tableField4d57ab-F_ShouldPurchase",
    "fullName": "物料分析商品列表信息-应采购",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "inputNumber",
      "label": "应采购",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInputNumber",
      "tagIcon": "icon-ym icon-ym-generator-number",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "regList": [],
      "trigger": [
        "blur",
        "change"
      ],
      "formId": "formItem8f9c68",
      "renderKey": 1769133660608,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item"
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
    "precision": 0,
    "disabled": false,
    "__vModel__": "F_ShouldPurchase"
  },
  {
    "id": "tableField4d57ab-F_CreatorUserId",
    "fullName": "物料分析商品列表信息-创建用户",
    "fullNameI18nCode": [
      "",
      ""
    ],
    "__config__": {
      "jnpfKey": "createUser",
      "label": "创建用户",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item",
      "formId": "formItem02ac7c",
      "renderKey": 1769133679954
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
    "id": "tableField4d57ab-F_CreatorTime",
    "fullName": "物料分析商品列表信息-创建时间",
    "fullNameI18nCode": [
      "",
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
      "tableName": "a_prod_analysis",
      "noShow": false,
      "isSubTable": true,
      "parentVModel": "tableField4d57ab",
      "relationTable": "a_prod_analysis_item",
      "formId": "formItem868eaa",
      "renderKey": 1769133685062
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
export default superQueryJson