using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdReportDefect;
 
/// <summary>
/// 不良品项修改输入参数.
/// </summary>
public class AProdReportDefectCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string F_DefectId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}