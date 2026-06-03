const searchList = [
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_supplier",
      "noShow": true,
      "formId": "formItem9071d0",
      "renderKey": 1767687344591
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
      "span": 24,
      "dragDisabled": false,
      "visibility": [
        "pc",
        "app"
      ],
      "tableName": "a_supplier",
      "noShow": true,
      "formId": "formItem3c7164",
      "renderKey": 1767687345104
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
    "label": "供应商编号",
    "labelI18nCode": "",
    "prop": "F_SupplierNo",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_SupplierNo",
    "fullName": "供应商编号",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "供应商编号",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem9be867",
      "renderKey": 1767687258471
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
    "__vModel__": "F_SupplierNo"
  },
  {
    "label": "供应商名称",
    "labelI18nCode": "",
    "prop": "F_SupplierName",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_SupplierName",
    "fullName": "供应商名称",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "供应商名称",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfInput",
      "tagIcon": "icon-ym icon-ym-generator-input",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItemc5ddd3",
      "renderKey": 1767687264071
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
    "__vModel__": "F_SupplierName"
  },
  {
    "label": "联系人",
    "labelI18nCode": "",
    "prop": "F_ContactPerson",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_ContactPerson",
    "fullName": "联系人",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "联系人",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [],
      "trigger": "blur",
      "formId": "formItem109e19",
      "renderKey": 1767687264742
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
    "__vModel__": "F_ContactPerson"
  },
  {
    "label": "联系人电话",
    "labelI18nCode": "",
    "prop": "F_ContactPhone",
    "jnpfKey": "input",
    "searchType": 2,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_ContactPhone",
    "fullName": "联系人电话",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "input",
      "label": "联系人电话",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [
        {
          "pattern": "/^1[3456789]\\d{9}$|^0\\d{2,3}-?\\d{7,8}$/",
          "message": "请输入正确的联系方式",
          "messageI18nCode": "sys.validate.phone"
        }
      ],
      "trigger": "blur",
      "formId": "formItem768ea2",
      "renderKey": 1767687269073,
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
    "__vModel__": "F_ContactPhone"
  },
  {
    "label": "地区",
    "labelI18nCode": "",
    "prop": "F_Region",
    "jnpfKey": "areaSelect",
    "value": [],
    "searchType": 1,
    "searchMultiple": false,
    "isKeyword": false,
    "id": "F_Region",
    "fullName": "地区",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "areaSelect",
      "label": "地区",
      "tipLabel": "",
      "showLabel": true,
      "tag": "JnpfAreaSelect",
      "tagIcon": "icon-ym icon-ym-generator-Provinces",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "formId": "formItem911f85",
      "renderKey": 1767687289602
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "placeholder": "请选择",
    "disabled": false,
    "clearable": true,
    "filterable": false,
    "multiple": false,
    "level": 2,
    "__vModel__": "F_Region"
  },
  {
    "label": "供应商标签",
    "labelI18nCode": "",
    "prop": "F_SupplierTags",
    "jnpfKey": "select",
    "searchType": 1,
    "searchMultiple": true,
    "isKeyword": false,
    "id": "F_SupplierTags",
    "fullName": "供应商标签",
    "fullNameI18nCode": [
      ""
    ],
    "__config__": {
      "jnpfKey": "select",
      "label": "供应商标签",
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
      "tableName": "a_supplier",
      "noShow": false,
      "regList": [],
      "trigger": "change",
      "dataType": "dictionary",
      "dictionaryType": "2008453808677588992",
      "propsUrl": "",
      "propsName": "",
      "useCache": true,
      "templateJson": [],
      "formId": "formItemcc29df",
      "renderKey": 1767687312654
    },
    "on": {
      "change": "({ data, rowIndex, formData, setFormData, setShowOrHide, setRequired, setDisabled, onlineUtils }) => {\n    // 在此编写代码\n    \n}"
    },
    "style": {
      "width": "100%"
    },
    "options": [
      {
        "id": "2008453887782162432",
        "fullName": "意向客户",
        "enCode": "意向客户",
        "sortCode": 0
      },
      {
        "id": "2008453944921165824",
        "fullName": "普通客户",
        "enCode": "普通客户",
        "sortCode": 1
      },
      {
        "id": "2008454012558512128",
        "fullName": "板材供货商",
        "enCode": "板材供货商",
        "sortCode": 2
      },
      {
        "id": "2008454098076176384",
        "fullName": "锁具配件",
        "enCode": "锁具配件",
        "sortCode": 3
      }
    ],
    "props": {
      "label": "fullName",
      "value": "enCode"
    },
    "placeholder": "请选择",
    "clearable": true,
    "disabled": false,
    "filterable": false,
    "multiple": true,
    "__vModel__": "F_SupplierTags"
  }
]
export default searchList