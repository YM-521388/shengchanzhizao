using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuReturnPrd;

/// <summary>
/// a_pu_return_prd输入参数.
/// </summary>
public class APuReturnPrdListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 生产退料单号.
    /// </summary>
    public string? F_ReturnInNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string? F_ReturnInType { get; set; }

    /// <summary>
    /// 退料日期.
    /// </summary>
    public string F_ReturnInDate { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string? F_Type { get; set; }
    public string? F_TypeCode { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    public string? F_WorkOrderId { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string? F_Method { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}