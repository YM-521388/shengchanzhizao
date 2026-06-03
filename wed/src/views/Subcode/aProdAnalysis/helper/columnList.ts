const columnList = [
  {
    "label": "生产计划",
    "labelI18nCode": "",
    "prop": "F_ProdPlanId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "popupSelect",
    "sortable": false,
    "width": null,
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
  // {
  //   "label": "是否考虑主商品库存",
  //   "labelI18nCode": "",
  //   "prop": "F_ConsiderMainStock",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_ConsiderMainStock",
  //   "fullName": "是否考虑主商品库存",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑主商品库存",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "defaultValue": "",
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem73814e",
  //     "renderKey": 1769132363141
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_ConsiderMainStock"
  // },
  // {
  //   "label": "是否考虑物料库存",
  //   "labelI18nCode": "",
  //   "prop": "F_ConsiderMaterialStock",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_ConsiderMaterialStock",
  //   "fullName": "是否考虑物料库存",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑物料库存",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem0dc424",
  //     "renderKey": 1769132526734
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_ConsiderMaterialStock"
  // },
  // {
  //   "label": "是否考虑物料占用",
  //   "labelI18nCode": "",
  //   "prop": "F_ConsiderMaterialOccupy",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_ConsiderMaterialOccupy",
  //   "fullName": "是否考虑物料占用",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑物料占用",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem9b1787",
  //     "renderKey": 1769132413330
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_ConsiderMaterialOccupy"
  // },
  // {
  //   "label": "是否考虑数量向上取整",
  //   "labelI18nCode": "",
  //   "prop": "F_RoundUpQty",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_RoundUpQty",
  //   "fullName": "是否考虑数量向上取整",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑数量向上取整",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem03e0b3",
  //     "renderKey": 1769132547287
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_RoundUpQty"
  // },
  // {
  //   "label": "是否考虑在制商品",
  //   "labelI18nCode": "",
  //   "prop": "F_ConsiderWipGoods",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_ConsiderWipGoods",
  //   "fullName": "是否考虑在制商品",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑在制商品",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItem7c14e0",
  //     "renderKey": 1769132493192
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_ConsiderWipGoods"
  // },
  // {
  //   "label": "是否考虑在途商品",
  //   "labelI18nCode": "",
  //   "prop": "F_ConsiderTransitGoods",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_ConsiderTransitGoods",
  //   "fullName": "是否考虑在途商品",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "是否考虑在途商品",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formItemeda625",
  //     "renderKey": 1769132563931
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_ConsiderTransitGoods"
  // },
  // {
  //   "label": "占用物料能否被其他单据出库",
  //   "labelI18nCode": "",
  //   "prop": "F_OccupyAllowOtherOut",
  //   "fixed": "none",
  //   "align": "left",
  //   "jnpfKey": "radio",
  //   "sortable": false,
  //   "width": null,
  //   "id": "F_OccupyAllowOtherOut",
  //   "fullName": "占用物料能否被其他单据出库",
  //   "fullNameI18nCode": [
  //     ""
  //   ],
  //   "__config__": {
  //     "jnpfKey": "radio",
  //     "label": "占用物料能否被其他单据出库",
  //     "tipLabel": "",
  //     "showLabel": true,
  //     "tag": "JnpfRadio",
  //     "tagIcon": "icon-ym icon-ym-generator-radio",
  //     "tableAlign": "left",
  //     "tableFixed": "none",
  //     "className": [],
  //     "required": false,
  //     "layout": "colFormItem",
  //     "span": 24,
  //     "dragDisabled": false,
  //     "visibility": [
  //       "pc",
  //       "app"
  //     ],
  //     "tableName": "a_prod_analysis",
  //     "noShow": false,
  //     "regList": [],
  //     "trigger": "change",
  //     "dataType": "static",
  //     "dictionaryType": "",
  //     "propsUrl": "",
  //     "propsName": "",
  //     "useCache": true,
  //     "templateJson": [],
  //     "formId": "formIteme68bec",
  //     "renderKey": 1769132510606
  //   },
  //   "on": {
  //     "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
  //   },
  //   "style": {
  //     "width": "100%"
  //   },
  //   "options": [
  //     {
  //       "fullName": "是",
  //       "id": "1"
  //     },
  //     {
  //       "fullName": "否",
  //       "id": "2"
  //     }
  //   ],
  //   "props": {
  //     "label": "fullName",
  //     "value": "id"
  //   },
  //   "direction": "horizontal",
  //   "optionType": "default",
  //   "buttonStyle": "solid",
  //   "size": "default",
  //   "disabled": false,
  //   "__vModel__": "F_OccupyAllowOtherOut"
  // },
  {
    "label": "分析人",
    "labelI18nCode": "",
    "prop": "F_AnalysisUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "userSelect",
    "sortable": false,
    "width": null,
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
    "label": "分析时间",
    "labelI18nCode": "",
    "prop": "F_AnalysisTime",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "datePicker",
    "sortable": false,
    "width": null,
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
    "label": "状态",
    "labelI18nCode": "",
    "prop": "F_State",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "input",
    "sortable": false,
    "width": null,
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
    "label": "创建用户",
    "labelI18nCode": "",
    "prop": "F_CreatorUserId",
    "fixed": "none",
    "align": "left",
    "jnpfKey": "createUser",
    "sortable": false,
    "width": null,
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
  }
]
export default columnList