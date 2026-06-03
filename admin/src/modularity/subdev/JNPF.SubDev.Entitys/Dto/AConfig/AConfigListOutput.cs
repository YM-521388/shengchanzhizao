namespace JNPF.example.Entitys.Dto.AConfig;

/// <summary>
/// 配置信息输入参数.
/// </summary>
public class AConfigListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 名称.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 值.
    /// </summary>
    public string F_Value { get; set; }

    /// <summary>
    /// 编码.
    /// </summary>
    public string? F_Code { get; set; }

}