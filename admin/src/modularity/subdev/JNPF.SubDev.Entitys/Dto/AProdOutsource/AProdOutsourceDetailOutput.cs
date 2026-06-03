using JNPF.Common.Models;
using Newtonsoft.Json;
using JNPF.example.Entitys.Dto.AProdOutsourceItem;

namespace JNPF.example.Entitys.Dto.AProdOutsource;

/// <summary>
/// a_prod_outsource详情输出参数.
/// </summary>
public class AProdOutsourceDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

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
    public string F_PlanNum { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

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
    public string? F_Region { get; set; }

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
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string? F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string F_PlanEndDate { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public object? F_Files { get; set; }

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
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string? F_PauseReason { get; set; }

    /// <summary>
    /// 取消原因.
    /// </summary>
    public string? F_CancelReason { get; set; }

    /// <summary>
    /// 外协工单用料清单.
    /// </summary>
    public List<AProdOutsourceItemDetailOutput> tableField7a8044 { get; set; }

}