using JNPF.Common.Models;

namespace JNPF.example.Entitys.Dto.AProdMachine;
 
/// <summary>
/// 机台管理输出参数.
/// </summary>
public class AProdMachineInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 机台编号.
    /// </summary>
    public string? F_MachineCode { get; set; }

    /// <summary>
    /// 机台名称.
    /// </summary>
    public string? F_MachineName { get; set; }

    /// <summary>
    /// 机台规格.
    /// </summary>
    public string? F_MachineSpec { get; set; }

    /// <summary>
    /// 机台状态.
    /// </summary>
    public string F_MachineStatus { get; set; }

    /// <summary>
    /// 机台图片.
    /// </summary>
    public List<FileControlsModel> F_Image { get; set; }

    /// <summary>
    /// 机台类别.
    /// </summary>
    public List<string> F_MachineType { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    public List<string> F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string F_LineId { get; set; }

    /// <summary>
    /// 单日运行时长.
    /// </summary>
    public int? F_DailyRunHours { get; set; }

    /// <summary>
    /// 标准工时(时).
    /// </summary>
    public int? F_StdHour { get; set; }

    /// <summary>
    /// 标准工时(分).
    /// </summary>
    public int? F_StdMinute { get; set; }

    /// <summary>
    /// 标准工时(秒).
    /// </summary>
    public int? F_StdSecond { get; set; }

    /// <summary>
    /// 标准产出.
    /// </summary>
    public int? F_StdOutput { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}