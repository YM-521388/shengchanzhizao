using JNPF.example.Entitys.Dto.APuCheckItem;
using JNPF.example.Entitys.Dto.APuCheckLog;

namespace JNPF.example.Entitys.Dto.APuCheck;

/// <summary>
/// a_pu_check详情输出参数.
/// </summary>
public class APuCheckDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 盘库单号.
    /// </summary>
    public string F_CheckNo { get; set; }

    /// <summary>
    /// 盘点日期.
    /// </summary>
    public string F_CheckDate { get; set; }

    /// <summary>
    /// 盘点人.
    /// </summary>
    public string? F_CheckUserId { get; set; }

    /// <summary>
    /// 盘点仓库.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

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
    /// 库存盘点商品列表.
    /// </summary>
    public List<APuCheckItemDetailOutput> tableField91ea29 { get; set; }

    /// <summary>
    /// 库存盘点日志.
    /// </summary>
    public List<APuCheckLogDetailOutput> tableField960db3 { get; set; }

}