using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdPlan;
using JNPF.example.Entitys.Dto.AProdPlanItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdPlan;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdPlanCrInput, AProdPlanEntity>()
			.Map(dest => dest.F_PlanNo, src => src.F_PlanNo != null ? src.F_PlanNo : null)
			.Map(dest => dest.F_PlanName, src => src.F_PlanName != null ? src.F_PlanName : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdPlanEntity, AProdPlanInfoOutput>()
			.Map(dest => dest.F_PlanNo, src => src.F_PlanNo != null ? src.F_PlanNo : null)
			.Map(dest => dest.F_PlanName, src => src.F_PlanName != null ? src.F_PlanName : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField2152f8, src => src.AProdPlanItemList.Adapt<List<AProdPlanItemInfoOutput>>())
		;
		config.ForType<AProdPlanEntity, AProdPlanDetailOutput>()
            .Map(dest => dest.tableField2152f8, src => src.AProdPlanItemList.Adapt<List<AProdPlanItemDetailOutput>>())
		;
	}
}
