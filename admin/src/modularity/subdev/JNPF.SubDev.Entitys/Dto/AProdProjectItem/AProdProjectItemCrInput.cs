using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdProjectGood;
using JNPF.example.Entitys.Dto.AProdProjectStep;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdProjectItem;
 
/// <summary>
/// 商品修改输入参数.
/// </summary>
public class AProdProjectItemCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 上级ID.
    /// </summary>
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 工艺路线ID.
    /// </summary>
    public string F_RoutingId { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    public string? F_WorkOrderNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanNum { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string F_Priority { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string F_BomId { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public List<FileControlsModel> F_DrawingFiles { get; set; }

    /// <summary>
    /// 门板厚度.
    /// </summary>
    public string F_DoorPlateThickness { get; set; }

    /// <summary>
    /// 箱体板厚.
    /// </summary>
    public string F_BoxPlateThickness { get; set; }

    /// <summary>
    /// 安装板或安装梁.
    /// </summary>
    public string F_InstallPlateOrBeam { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    public string? F_CustomerName { get; set; }

    /// <summary>
    /// 锁具.
    /// </summary>
    public string? F_LockSet { get; set; }

    /// <summary>
    /// 铰链.
    /// </summary>
    public string? F_Hinge { get; set; }

    /// <summary>
    /// BOM图片.
    /// </summary>
    public List<FileControlsModel> F_BomImages { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    public string? F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public List<string> F_Category { get; set; }


    /// <summary>
    /// 项目商品工序列表.
    /// </summary>
    public List<AProdProjectStepCrInput> Process { get; set; }


    /// <summary>
    /// 项目商品用料清单.
    /// </summary>
    public List<AProdProjectGoodCrInput> ProjectGoods { get; set; }
}