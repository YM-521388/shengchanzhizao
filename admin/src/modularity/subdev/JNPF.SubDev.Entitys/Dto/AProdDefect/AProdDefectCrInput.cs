using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdDefect;
 
/// <summary>
/// 不良品项修改输入参数.
/// </summary>
public class AProdDefectCrInput
{
    /// <summary>
    /// 不良品项编号.
    /// </summary>
    public string? F_DefectCode { get; set; }

    /// <summary>
    /// 不良品项名称.
    /// </summary>
    public string? F_DefectName { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}