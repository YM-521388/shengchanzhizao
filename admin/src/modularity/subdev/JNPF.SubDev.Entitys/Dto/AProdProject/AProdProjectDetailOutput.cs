using JNPF.example.Entitys.Dto.AProdProjectItem;

namespace JNPF.example.Entitys.Dto.AProdProject;

/// <summary>
/// a_prod_project详情输出参数.
/// </summary>
public class AProdProjectDetailOutput
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
    public string? F_ProjectTypeName { get; set; }

    /// <summary>
    /// 项目状态.
    /// </summary>
    public string? F_Status { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 修改人员.
    /// </summary>
    public string? F_UpdateUserId { get; set; }

    /// <summary>
    /// 修改时间.
    /// </summary>
    public string F_UpdateTime { get; set; }

    /// <summary>
    /// 项目商品列表.
    /// </summary>
    public List<AProdProjectItemDetailOutput> tableField751ecb { get; set; }

}