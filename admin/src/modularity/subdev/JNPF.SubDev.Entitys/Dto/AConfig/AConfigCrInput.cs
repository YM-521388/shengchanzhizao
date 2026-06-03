using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AConfig;
 
/// <summary>
/// 配置信息修改输入参数.
/// </summary>
public class AConfigCrInput
{
    /// <summary>
    /// 名称.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 值.
    /// </summary>
    public int? F_Value { get; set; }

    /// <summary>
    /// 编码.
    /// </summary>
    public string? F_Code { get; set; }

}