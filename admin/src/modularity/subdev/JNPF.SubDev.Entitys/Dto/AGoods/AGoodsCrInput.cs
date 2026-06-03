using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AGoods;
 
/// <summary>
/// 商品管理修改输入参数.
/// </summary>
public class AGoodsCrInput
{
    /// <summary>
    /// 商品编号.
    /// </summary>
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 商品分类ID.
    /// </summary>
    public List<string> F_CategoryId { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public List<string> F_Unit { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }


    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 材质.
    /// </summary>
    public string? F_Material { get; set; }

    /// <summary>
    /// 销售单价.
    /// </summary>
    public decimal? F_SalePrice { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 售后佣金.
    /// </summary>
    public decimal? F_CommissionFixed { get; set; }

    /// <summary>
    /// 商品来源.
    /// </summary>
    public string F_Source { get; set; }

    /// <summary>
    /// 外协单价.
    /// </summary>
    public decimal? F_OutsourcePrice { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string? F_Specification { get; set; }

    /// <summary>
    /// 商品类型.
    /// </summary>
    public string F_Type { get; set; }


    /// <summary>
    /// 检验规范.
    /// </summary>
    public string? F_InspectRule { get; set; }

    /// <summary>
    /// 库存上限警告值.
    /// </summary>
    public int? F_StockUpperLimit { get; set; }

    /// <summary>
    /// 库存下限警告值.
    /// </summary>
    public int? F_StockLowerLimit { get; set; }

    /// <summary>
    /// 商品图片.
    /// </summary>
    public List<FileControlsModel> F_Image { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_AttachmentUrl { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Remark { get; set; }

    /// <summary>
    /// 商品简介.
    /// </summary>
    public string? F_Intro { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

}