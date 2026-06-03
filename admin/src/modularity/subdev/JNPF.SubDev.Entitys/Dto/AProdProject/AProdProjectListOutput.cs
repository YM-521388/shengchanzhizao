namespace JNPF.example.Entitys.Dto.AProdProject;

/// <summary>
/// a_prod_project输入参数.
/// </summary>
public class AProdProjectListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 项目编号.
    /// </summary>
    public string? F_ProjectNo { get; set; }

    /// <summary>
    /// 项目名称.
    /// </summary>
    public string? F_ProjectName { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 项目类别.
    /// </summary>
    public string? F_ProjectType { get; set; }
    public string? F_ProjectTypeCode { get; set; }

    /// <summary>
    /// 项目状态.
    /// </summary>
    public string? F_Status { get; set; }

    /// <summary>
    /// 项目进度.
    /// </summary>
    public string? F_ProjectProgress { get; set; }

    /// <summary>
    /// 工单数.
    /// </summary>
    public int? F_WorkOrderQty { get; set; }

    /// <summary>
    /// 已完成工单.
    /// </summary>
    public int? F_CompletedQtyStatus { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanQty { get; set; }

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
    public string F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 修改人员.
    /// </summary>
    public string? F_UpdateUserId { get; set; }

    /// <summary>
    /// 修改时间.
    /// </summary>
    public string F_UpdateTime { get; set; }

}