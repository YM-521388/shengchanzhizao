using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdProcessing;
using JNPF.example.Entitys.Dto.AProdBomItem;
using JNPF.example.Entitys.Dto.AProdTask;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdProcessing;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProcessingCrInput, AProdProcessingEntity>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ProjectId, src => src.F_ProjectId != null ? src.F_ProjectId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_ProcessNo, src => src.F_ProcessNo != null ? src.F_ProcessNo : null)
			.Map(dest => dest.F_PlanQty, src => src.F_PlanQty != null ? src.F_PlanQty : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
			.Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
			.Map(dest => dest.F_DrawingFile, src => src.F_DrawingFile != null && src.F_DrawingFile.Count > 0 ? src.F_DrawingFile.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_DoorPlateThickness, src => src.F_DoorPlateThickness != null ? src.F_DoorPlateThickness : null)
			.Map(dest => dest.F_BoxPlateThickness, src => src.F_BoxPlateThickness != null ? src.F_BoxPlateThickness : null)
			.Map(dest => dest.F_InstallOrBeam, src => src.F_InstallOrBeam != null ? src.F_InstallOrBeam : null)
			.Map(dest => dest.F_LockSet, src => src.F_LockSet != null ? src.F_LockSet : null)
			.Map(dest => dest.F_Hinge, src => src.F_Hinge != null ? src.F_Hinge : null)
			.Map(dest => dest.F_Color, src => src.F_Color != null ? src.F_Color : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null && src.F_Type.Count > 0 ? src.F_Type.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_BOMImages, src => src.F_BOMImages != null && src.F_BOMImages.Count > 0 ? src.F_BOMImages.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_SequenceNo, src => src.F_SequenceNo != null ? src.F_SequenceNo : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdProcessingEntity, AProdProcessingInfoOutput>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ProjectId, src => src.F_ProjectId != null ? src.F_ProjectId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_ProcessNo, src => src.F_ProcessNo != null ? src.F_ProcessNo : null)
			.Map(dest => dest.F_PlanQty, src => src.F_PlanQty != null ? src.F_PlanQty : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
			.Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
			.Map(dest => dest.F_DrawingFile, src => src.F_DrawingFile != null ? src.F_DrawingFile.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_DoorPlateThickness, src => src.F_DoorPlateThickness != null ? src.F_DoorPlateThickness : null)
			.Map(dest => dest.F_BoxPlateThickness, src => src.F_BoxPlateThickness != null ? src.F_BoxPlateThickness : null)
			.Map(dest => dest.F_InstallOrBeam, src => src.F_InstallOrBeam != null ? src.F_InstallOrBeam : null)
			.Map(dest => dest.F_LockSet, src => src.F_LockSet != null ? src.F_LockSet : null)
			.Map(dest => dest.F_Hinge, src => src.F_Hinge != null ? src.F_Hinge : null)
			.Map(dest => dest.F_Color, src => src.F_Color != null ? src.F_Color : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type.ToObject<List<string>>() : null)
			.Map(dest => dest.F_BOMImages, src => src.F_BOMImages != null ? src.F_BOMImages.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_SequenceNo, src => src.F_SequenceNo != null ? src.F_SequenceNo : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField449e6c, src => src.AProdBomItemList.Adapt<List<AProdBomItemInfoOutput>>())
            .Map(dest => dest.tableField0c720e, src => src.AProdTaskList.Adapt<List<AProdTaskInfoOutput>>())
		;
		config.ForType<AProdProcessingEntity, AProdProcessingDetailOutput>()
            .Map(dest => dest.tableField449e6c, src => src.AProdBomItemList.Adapt<List<AProdBomItemDetailOutput>>())
            .Map(dest => dest.tableField0c720e, src => src.AProdTaskList.Adapt<List<AProdTaskDetailOutput>>())
		;
	}
}
