using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuPlan;
using JNPF.example.Entitys.Dto.APuPlanItem;
using JNPF.example.Entitys.Dto.APuPlanLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuPlan;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuPlanCrInput, APuPlanEntity>()
			.Map(dest => dest.F_PlanNo, src => src.F_PlanNo != null ? src.F_PlanNo : null)
			.Map(dest => dest.F_PlanName, src => src.F_PlanName != null ? src.F_PlanName : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_PurchaseNum, src => src.F_PurchaseNum != null ? src.F_PurchaseNum : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuPlanEntity, APuPlanInfoOutput>()
			.Map(dest => dest.F_PlanNo, src => src.F_PlanNo != null ? src.F_PlanNo : null)
			.Map(dest => dest.F_PlanName, src => src.F_PlanName != null ? src.F_PlanName : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_PurchaseNum, src => src.F_PurchaseNum != null ? src.F_PurchaseNum : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldc87c94, src => src.APuPlanItemList.Adapt<List<APuPlanItemInfoOutput>>())
            .Map(dest => dest.tableFielde82301, src => src.APuPlanLogList.Adapt<List<APuPlanLogInfoOutput>>())
		;
		config.ForType<APuPlanEntity, APuPlanDetailOutput>()
            .Map(dest => dest.tableFieldc87c94, src => src.APuPlanItemList.Adapt<List<APuPlanItemDetailOutput>>())
            .Map(dest => dest.tableFielde82301, src => src.APuPlanLogList.Adapt<List<APuPlanLogDetailOutput>>())
		;
	}
}
