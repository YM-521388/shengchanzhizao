using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 商品管理实体.
/// </summary>
[SugarTable("a_goods")]
public class AGoodsEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 商品编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsNo")]
    [CodeGenUpload("F_GoodsNo", "F_GoodsNo", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"商品编号\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsName")]
    [CodeGenUpload("F_GoodsName", "F_GoodsName", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": true,\r\n  \"unique\": false,\r\n  \"label\": \"商品名称\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemc10586\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 商品分类ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CategoryId")]
    public string? F_CategoryId { get; set; }

    /// <summary>
    /// 单位(“/”分割，例：箱/盒).
    /// </summary>
    [SugarColumn(ColumnName = "F_Unit")]
    [CodeGenUpload("F_Unit", "F_Unit", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"单位（“/”分割，例：箱/盒）\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_Unit { get; set; }


    /// <summary>
    /// 数量(填写二级单位数量，一级单位数量为1).
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [CodeGenUpload("F_Num", "F_Num", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"数量（填写二级单位数量，一级单位数量默认为1）\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public int? F_Num { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    [SugarColumn(ColumnName = "F_Unit_Ratio")]
    public string? F_Unit_Ratio { get; set; }



    /// <summary>
    /// 商品编码.
    /// </summary>
    [SugarColumn(ColumnName = "F_Encoding")]
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 销售单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_SalePrice")]
    [CodeGenUpload("F_SalePrice", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"销售单价\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemf981b1\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public decimal? F_SalePrice { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_CostPrice")]
    [CodeGenUpload("F_CostPrice", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"成本单价\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItema77dec\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 售后佣金.
    /// </summary>
    [SugarColumn(ColumnName = "F_CommissionFixed")]
    [CodeGenUpload("F_CommissionFixed", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"售后佣金\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem0daec6\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public decimal? F_CommissionFixed { get; set; }

    /// <summary>
    /// 商品来源.
    /// </summary>
    [SugarColumn(ColumnName = "F_Source")]
    [CodeGenUpload("F_Source", false, "{\r\n  \"label\": \"fullName\",\r\n  \"value\": \"enCode\",\r\n  \"children\": null\r\n}", "[\r\n  {\r\n    \"id\": \"2008449032669761536\",\r\n    \"fullName\": \"自产\",\r\n    \"enCode\": \"自产\",\r\n    \"sortCode\": 0\r\n  },\r\n  {\r\n    \"id\": \"2008449101255020544\",\r\n    \"fullName\": \"外协加工\",\r\n    \"enCode\": \"外协加工\",\r\n    \"sortCode\": 1\r\n  },\r\n  {\r\n    \"id\": \"2008449174701477888\",\r\n    \"fullName\": \"外购\",\r\n    \"enCode\": \"外购\",\r\n    \"sortCode\": 2\r\n  }\r\n]", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"select\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": \"2008448951216377856\",\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"商品来源\",\r\n  \"dataType\": \"dictionary\",\r\n  \"propsUrl\": \"\",\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": [],\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem6ce206\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfSelect\"\r\n}")]
    public string? F_Source { get; set; }

    /// <summary>
    /// 外协单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_OutsourcePrice")]
    [CodeGenUpload("F_OutsourcePrice", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"外协单价\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem899814\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public decimal? F_OutsourcePrice { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_SupplierId")]
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    [SugarColumn(ColumnName = "F_Specification")]
    [CodeGenUpload("F_Specification", "F_Specification", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"规格\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemc812df\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_Specification { get; set; }

    /// <summary>
    /// 材质.
    /// </summary>
    [SugarColumn(ColumnName = "F_Material")]
    [CodeGenUpload("F_Material", "F_Material", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"材质\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemc85ee1\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_Material { get; set; }

    /// <summary>
    /// 商品类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_Type")]
    [CodeGenUpload("F_Type", false, "{\n  \"label\": \"fullName\",\n  \"value\": \"enCode\",\n  \"children\": null\n}", "[\n  {\n    \"id\": \"2029481905342255104\",\n    \"fullName\": \"原材料\",\n    \"enCode\": \"0\",\n    \"sortCode\": 0\n  },\n  {\n    \"id\": \"2029481938942824448\",\n    \"fullName\": \"零部件\",\n    \"enCode\": \"1\",\n    \"sortCode\": 1\n  },\n  {\n    \"id\": \"2029481973482917888\",\n    \"fullName\": \"成品\",\n    \"enCode\": \"2\",\n    \"sortCode\": 2\n  }\n]", "{\n  \"tableName\": \"a_goods\",\n  \"regList\": [],\n  \"jnpfKey\": \"select\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": \"2029481827470807040\",\n  \"required\": true,\n  \"unique\": false,\n  \"label\": \"商品类型\",\n  \"dataType\": \"dictionary\",\n  \"propsUrl\": \"\",\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": [],\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem8c88e6\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfSelect\"\n}")]
    public string? F_Type { get; set; }

    /// <summary>
    /// 检验规范.
    /// </summary>
    [SugarColumn(ColumnName = "F_InspectRule")]
    [CodeGenUpload("F_InspectRule", "F_InspectRule", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"检验规范\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItembb8f48\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_InspectRule { get; set; }

    /// <summary>
    /// 库存上限警告值.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockUpperLimit")]
    [CodeGenUpload("F_StockUpperLimit", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"库存上限警告值\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": 1,\r\n  \"formId\": \"formItem16780b\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public int? F_StockUpperLimit { get; set; }

    /// <summary>
    /// 库存下限警告值.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockLowerLimit")]
    [CodeGenUpload("F_StockLowerLimit", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"inputNumber\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"库存下限警告值\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": 1,\r\n  \"formId\": \"formItemd6ec58\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInputNumber\"\r\n}", 95279527, 95279527)]
    public int? F_StockLowerLimit { get; set; }

    /// <summary>
    /// 商品图片.
    /// </summary>
    [SugarColumn(ColumnName = "F_Image")]
    public string? F_Image { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_AttachmentUrl")]
    public string? F_AttachmentUrl { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Remark")]
    [CodeGenUpload("F_Remark", "F_Remark", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"textarea\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"备注\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem36d84d\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfTextarea\"\r\n}", 9999999L)]
    public string? F_Remark { get; set; }

    /// <summary>
    /// 商品简介.
    /// </summary>
    [SugarColumn(ColumnName = "F_Intro")]
    public string? F_Intro { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemb2c1f3\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建用户\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem6d9dfe\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    [CodeGenUpload("F_CreatorOrganizeId", false, "all", "[]", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"depSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建组织\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formIteme1d4f3\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfDepSelect\"\r\n}")]
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 逻辑删除.
    /// </summary>
    [SugarColumn(ColumnName = "F_Delete_Mark")]
    public int? DeleteMark { get; set; }

    /// <summary>
    /// 获取或设置 删除时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_TIME", ColumnDescription = "删除时间")]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 获取或设置 删除用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_USER_ID", ColumnDescription = "删除用户")]
    public string DeleteUserId { get; set; }

    /// <summary>
    /// 流程引擎ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_Flow_Id")]
    public string FlowId { get; set; }

    /// <summary>
    /// 获取或设置 租户id.
    /// </summary>
    [SugarColumn(ColumnName = "F_TENANT_ID", ColumnDescription = "租户id", IsPrimaryKey = true)]
    public string TenantId { get; set; }
}

public class AGoodsUnitEntity
{
    /// <summary>
    /// 商品编号.
    /// </summary>
    [CodeGenUpload("F_GoodsNo", "F_GoodsNo", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"商品编号\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 单位(“/”分割，例：箱/盒).
    /// </summary>
    [CodeGenUpload("F_Unit", "F_Unit", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"单位（“/”分割，例：箱/盒）\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public string? F_Unit { get; set; }


    /// <summary>
    /// 数量(二级单位数量，一级单位数量为1).
    /// </summary>
    [CodeGenUpload("F_Num", "F_Num", "{\r\n  \"tableName\": \"a_goods\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"input\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"数量（填写二级单位数量，一级单位数量默认为1）\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem034b17\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfInput\"\r\n}", 9999999L)]
    public int? F_Num { get; set; }

}