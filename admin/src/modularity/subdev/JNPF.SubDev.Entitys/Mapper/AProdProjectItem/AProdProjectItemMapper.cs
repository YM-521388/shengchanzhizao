using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdProjectItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdProjectItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProjectItemCrInput, AProdProjectItemEntity>()
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_WorkOrderNo, src => src.F_WorkOrderNo != null ? src.F_WorkOrderNo : null)
			.Map(dest => dest.F_PlanNum, src => src.F_PlanNum != null ? src.F_PlanNum : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_BomId, src => src.F_BomId != null ? src.F_BomId : null)
			.Map(dest => dest.F_DrawingFiles, src => src.F_DrawingFiles != null && src.F_DrawingFiles.Count > 0 ? src.F_DrawingFiles.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_DoorPlateThickness, src => src.F_DoorPlateThickness != null ? src.F_DoorPlateThickness : null)
			.Map(dest => dest.F_BoxPlateThickness, src => src.F_BoxPlateThickness != null ? src.F_BoxPlateThickness : null)
			.Map(dest => dest.F_InstallPlateOrBeam, src => src.F_InstallPlateOrBeam != null ? src.F_InstallPlateOrBeam : null)
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_LockSet, src => src.F_LockSet != null ? src.F_LockSet : null)
			.Map(dest => dest.F_Hinge, src => src.F_Hinge != null ? src.F_Hinge : null)
			.Map(dest => dest.F_BomImages, src => src.F_BomImages != null && src.F_BomImages.Count > 0 ? src.F_BomImages.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Color, src => src.F_Color != null ? src.F_Color : null)
			.Map(dest => dest.F_Category, src => src.F_Category != null && src.F_Category.Count > 0 ? src.F_Category.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
		;
		config.ForType<AProdProjectItemEntity, AProdProjectItemInfoOutput>()
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_WorkOrderNo, src => src.F_WorkOrderNo != null ? src.F_WorkOrderNo : null)
			.Map(dest => dest.F_PlanNum, src => src.F_PlanNum != null ? src.F_PlanNum : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_BomId, src => src.F_BomId != null ? src.F_BomId : null)
			.Map(dest => dest.F_DrawingFiles, src => src.F_DrawingFiles != null ? src.F_DrawingFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_DoorPlateThickness, src => src.F_DoorPlateThickness != null ? src.F_DoorPlateThickness : null)
			.Map(dest => dest.F_BoxPlateThickness, src => src.F_BoxPlateThickness != null ? src.F_BoxPlateThickness : null)
			.Map(dest => dest.F_InstallPlateOrBeam, src => src.F_InstallPlateOrBeam != null ? src.F_InstallPlateOrBeam : null)
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_LockSet, src => src.F_LockSet != null ? src.F_LockSet : null)
			.Map(dest => dest.F_Hinge, src => src.F_Hinge != null ? src.F_Hinge : null)
			.Map(dest => dest.F_BomImages, src => src.F_BomImages != null ? src.F_BomImages.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_Color, src => src.F_Color != null ? src.F_Color : null)
			.Map(dest => dest.F_Category, src => src.F_Category != null ? src.F_Category.ToObject<List<string>>() : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdProjectItemEntity, AProdProjectItemDetailOutput>()
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_WorkOrderNo, src => src.F_WorkOrderNo != null ? src.F_WorkOrderNo : null)
			.Map(dest => dest.F_PlanNum, src => src.F_PlanNum != null ? src.F_PlanNum : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_BomId, src => src.F_BomId != null ? src.F_BomId : null)
			.Map(dest => dest.F_DrawingFiles, src => src.F_DrawingFiles != null ? src.F_DrawingFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_DoorPlateThickness, src => src.F_DoorPlateThickness != null ? src.F_DoorPlateThickness : null)
			.Map(dest => dest.F_BoxPlateThickness, src => src.F_BoxPlateThickness != null ? src.F_BoxPlateThickness : null)
			.Map(dest => dest.F_InstallPlateOrBeam, src => src.F_InstallPlateOrBeam != null ? src.F_InstallPlateOrBeam : null)
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_LockSet, src => src.F_LockSet != null ? src.F_LockSet : null)
			.Map(dest => dest.F_Hinge, src => src.F_Hinge != null ? src.F_Hinge : null)
			.Map(dest => dest.F_BomImages, src => src.F_BomImages != null ? src.F_BomImages.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_Color, src => src.F_Color != null ? src.F_Color : null)
			.Map(dest => dest.F_Category, src => src.F_Category != null ? src.F_Category : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
