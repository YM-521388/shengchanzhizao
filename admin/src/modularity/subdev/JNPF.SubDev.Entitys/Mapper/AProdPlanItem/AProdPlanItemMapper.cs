using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdPlanItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdPlanItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdPlanItemCrInput, AProdPlanItemEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
		;
		config.ForType<AProdPlanItemEntity, AProdPlanItemListOutput>()
		;
		config.ForType<AProdPlanItemEntity, AProdPlanItemInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdPlanItemEntity, AProdPlanItemDetailOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
