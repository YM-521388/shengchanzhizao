using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdOutsourceItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdOutsource;

/// <summary>
/// a_prod_outsource修改输入参数.
/// </summary>
public class AProdOutsourceCrInput
{
    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string? F_BOMId { get; set; }

    /// <summary>
    /// 外协工单号.
    /// </summary>
    public string? F_OutsourceNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanNum { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 供应商联系人.
    /// </summary>
    public string? F_ContactPerson { get; set; }

    /// <summary>
    /// 供应商联系人电话.
    /// </summary>
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 供应商地区.
    /// </summary>
    public List<string> F_Region { get; set; }

    /// <summary>
    /// 供应商详细地址.
    /// </summary>
    public string? F_DetailAddress { get; set; }

    /// <summary>
    /// 外协单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 暂停/取消原因.
    /// </summary>
    public string? F_Reason { get; set; }


    /// <summary>
    /// 外协工单用料清单.
    /// </summary>
    public List<AProdOutsourceItemCrInput> tableField7a8044 { get; set; }

}