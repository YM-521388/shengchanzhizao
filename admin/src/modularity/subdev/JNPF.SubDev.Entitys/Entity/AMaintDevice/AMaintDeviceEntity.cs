using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_maint_device实体.
/// </summary>
[SugarTable("a_maint_device")]
public class AMaintDeviceEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 设备编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceNo")]
    public string? F_DeviceNo { get; set; }

    /// <summary>
    /// 设备名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceName")]
    public string? F_DeviceName { get; set; }

    /// <summary>
    /// 设备规格.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceSpec")]
    public string? F_DeviceSpec { get; set; }

    /// <summary>
    /// 设备状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceStatus")]
    public string? F_DeviceStatus { get; set; }

    /// <summary>
    /// 设备类别.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceType")]
    public string? F_DeviceType { get; set; }

    /// <summary>
    /// 分组.
    /// </summary>
    [SugarColumn(ColumnName = "F_GroupId")]
    public string? F_GroupId { get; set; }

    /// <summary>
    /// 设备负责人.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceUsersId")]
    public string? F_DeviceUsersId { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    [SugarColumn(ColumnName = "F_WorkshopId")]
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    [SugarColumn(ColumnName = "F_LineId")]
    public string? F_LineId { get; set; }

    /// <summary>
    /// 设备二维码.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceQrCode")]
    public string? F_DeviceQrCode { get; set; }

    /// <summary>
    /// 设备图片.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceImages")]
    public string? F_DeviceImages { get; set; }

    /// <summary>
    /// 启用状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_EnabledMark")]
    public string? F_EnabledMark { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_maint_device\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem46b38e\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_maint_device\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemba8191\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 设备物料清单.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AMaintDeviceItemEntity.F_DeviceId), nameof(id))]
    public List<AMaintDeviceItemEntity> AMaintDeviceItemList { get; set; }

}