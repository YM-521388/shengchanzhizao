using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_pu_stock_pu实体.
/// </summary>
[SugarTable("a_pu_stock_pu")]
public class APuStockPuEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 采购入库单号.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInNo")]
    public string? F_StockInNo { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_WarehouseId")]
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 采购单id.
    /// </summary>
    [SugarColumn(ColumnName = "F_pu_Orderld")]
    public string? F_pu_Orderld { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInType")]
    public string? F_StockInType { get; set; }

    /// <summary>
    /// 入库日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInDate")]
    public DateTime? F_StockInDate { get; set; }

    /// <summary>
    /// 收货人.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInUserId")]
    public string? F_StockInUserId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 逻辑删除.
    /// </summary>
    [SugarColumn(ColumnName = "F_Delete_Mark")]
    public int? DeleteMark{ get; set; }

    /// <summary>
    /// 获取或设置 删除时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_TIME", ColumnDescription = "删除时间")]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 获取或设置 删除用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_USER_ID", ColumnDescription = "删除用户")]
    public string DeleteUserId { get; set; }

    /// <summary>
    /// 流程引擎ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_Flow_Id")]
    public string FlowId { get; set; }
    
    /// <summary>
    /// 获取或设置 租户id.
    /// </summary>
    [SugarColumn(ColumnName = "F_TENANT_ID", ColumnDescription = "租户id", IsPrimaryKey = true)]
    public string TenantId { get; set; }

    /// <summary>
    /// 采购入库单商品列表.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuStockPuItemEntity.F_StockInPUId), nameof(id))]
    public List<APuStockPuItemEntity> APuStockPuItemList { get; set; }

    /// <summary>
    /// 采购入库单日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuStockPuLogEntity.F_StockInPUId), nameof(id))]
    public List<APuStockPuLogEntity> APuStockPuLogList { get; set; }

}