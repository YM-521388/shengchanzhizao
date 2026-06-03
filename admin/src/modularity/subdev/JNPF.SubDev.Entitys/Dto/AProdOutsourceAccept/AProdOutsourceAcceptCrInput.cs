using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdOutsourceAcceptItem;
using JNPF.example.Entitys.Dto.AProdOutsourceSettleLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdOutsourceAccept;

/// <summary>
/// a_prod_outsource_accept修改输入参数.
/// </summary>
public class AProdOutsourceAcceptCrInput
{
    /// <summary>
    /// 外协工单ID.
    /// </summary>
    public string? F_OutsourceId { get; set; }

    /// <summary>
    /// 合格数量.
    /// </summary>
    public int? F_PassNum { get; set; }

    /// <summary>
    /// 不合格数量.
    /// </summary>
    public int? F_FailNum { get; set; }

    /// <summary>
    /// 外协类型.
    /// </summary>
    public string? F_OutsourceType { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string F_SettleStatus { get; set; }

    /// <summary>
    /// 结算单价.
    /// </summary>
    public decimal? F_SettleUnitPrice { get; set; }

    /// <summary>
    /// 结算人.
    /// </summary>
    public string F_SettleUserId { get; set; }

    /// <summary>
    /// 结算时间.
    /// </summary>
    public DateTime? F_SettleTime { get; set; }

    /// <summary>
    /// 结算附件.
    /// </summary>
    public List<FileControlsModel> F_SettleFiles { get; set; }

    /// <summary>
    /// 结算备注.
    /// </summary>
    public string? F_SettleDesc { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }
    public string? F_Description { get; set; }


    /// <summary>
    /// 外协验收内容.
    /// </summary>
    public List<AProdOutsourceAcceptItemCrInput> tableField561a64 { get; set; }

    /// <summary>
    /// 外协结算日志.
    /// </summary>
    public List<AProdOutsourceSettleLogCrInput> tableField4b440e { get; set; }

}