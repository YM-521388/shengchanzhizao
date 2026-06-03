using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdRoutingStepItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdRoutingStep;
 
/// <summary>
/// 工序修改输入参数.
/// </summary>
public class AProdRoutingStepCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

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
    /// 计价方式.
    /// </summary>
    public string F_PriceType { get; set; }

    /// <summary>
    /// 单位关系(生产).
    /// </summary>
    public int? F_UnitRatioProd { get; set; }

    /// <summary>
    /// 单位关系(报工).
    /// </summary>
    public int? F_UnitRatioReport { get; set; }

    /// <summary>
    /// 单位关系(单位).
    /// </summary>
    public string F_UnitRatioBase { get; set; }

    /// <summary>
    /// 工序需外协.
    /// </summary>
    public int? F_IsOutsource { get; set; }

    /// <summary>
    /// 工序需质检.
    /// </summary>
    public int? F_IsQc { get; set; }

    /// <summary>
    /// 不良品需报废、返工.
    /// </summary>
    public int? F_DefectHandle { get; set; }

    /// <summary>
    /// 生产任务计时.
    /// </summary>
    public int? F_TaskTimer { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }


    /// <summary>
    /// 工序物料信息.
    /// </summary>
    public List<AProdRoutingStepItemCrInput> tableField3b6f08 { get; set; }

}