using JNPF.Common.Models;

namespace JNPF.example.Entitys.Dto.APuOrderInvoice;
 
/// <summary>
/// 采购单发票单管理输出参数.
/// </summary>
public class APuOrderInvoiceInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 采购单ID.
    /// </summary>
    public string? F_OrderId { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 发票金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 开票日期.
    /// </summary>
    public DateTime? F_InvoiceDate { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

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

}