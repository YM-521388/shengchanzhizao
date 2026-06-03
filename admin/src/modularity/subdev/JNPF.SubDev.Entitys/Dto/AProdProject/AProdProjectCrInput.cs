using JNPF.example.Entitys.Dto.AProdProjectItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdProject;

/// <summary>
/// a_prod_project修改输入参数.
/// </summary>
public class AProdProjectCrInput
{
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
    public string F_ProjectType { get; set; }

    /// <summary>
    /// 项目状态.
    /// </summary>
    public string F_Status { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 修改人员.
    /// </summary>
    public string? F_UpdateUserId { get; set; }


    /// <summary>
    /// 项目商品列表.
    /// </summary>
    public List<AProdProjectItemCrInput> tableField751ecb { get; set; }

}