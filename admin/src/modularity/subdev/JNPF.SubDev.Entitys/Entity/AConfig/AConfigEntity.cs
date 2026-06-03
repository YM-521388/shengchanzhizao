using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 配置信息实体.
/// </summary>
[SugarTable("a_config")]
public class AConfigEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_Name")]
    public string? F_Name { get; set; }

    /// <summary>
    /// 值.
    /// </summary>
    [SugarColumn(ColumnName = "F_Value")]
    public string? F_Value { get; set; }

    /// <summary>
    /// 编码.
    /// </summary>
    [SugarColumn(ColumnName = "F_Code")]
    public string? F_Code { get; set; }

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
}