using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdAnalysisItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdAnalysisItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdAnalysisItemCrInput, AProdAnalysisItemEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_DemandQty, src => src.F_DemandQty != null ? src.F_DemandQty : null)
			.Map(dest => dest.F_AvailableStock, src => src.F_AvailableStock != null ? src.F_AvailableStock : null)
			.Map(dest => dest.F_WipStock, src => src.F_WipStock != null ? src.F_WipStock : null)
			.Map(dest => dest.F_TransitStock, src => src.F_TransitStock != null ? src.F_TransitStock : null)
			.Map(dest => dest.F_ShouldSelfMake, src => src.F_ShouldSelfMake != null ? src.F_ShouldSelfMake : null)
			.Map(dest => dest.F_ShouldOutsource, src => src.F_ShouldOutsource != null ? src.F_ShouldOutsource : null)
			.Map(dest => dest.F_ShouldPurchase, src => src.F_ShouldPurchase != null ? src.F_ShouldPurchase : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		//config.ForType<AProdAnalysisItemEntity, AProdAnalysisItemListOutput>()
		;
		config.ForType<AProdAnalysisItemEntity, AProdAnalysisItemInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_DemandQty, src => src.F_DemandQty != null ? src.F_DemandQty : null)
			.Map(dest => dest.F_AvailableStock, src => src.F_AvailableStock != null ? src.F_AvailableStock : null)
			.Map(dest => dest.F_WipStock, src => src.F_WipStock != null ? src.F_WipStock : null)
			.Map(dest => dest.F_TransitStock, src => src.F_TransitStock != null ? src.F_TransitStock : null)
			.Map(dest => dest.F_ShouldSelfMake, src => src.F_ShouldSelfMake != null ? src.F_ShouldSelfMake : null)
			.Map(dest => dest.F_ShouldOutsource, src => src.F_ShouldOutsource != null ? src.F_ShouldOutsource : null)
			.Map(dest => dest.F_ShouldPurchase, src => src.F_ShouldPurchase != null ? src.F_ShouldPurchase : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdAnalysisItemEntity, AProdAnalysisItemDetailOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_DemandQty, src => src.F_DemandQty != null ? src.F_DemandQty : null)
			.Map(dest => dest.F_AvailableStock, src => src.F_AvailableStock != null ? src.F_AvailableStock : null)
			.Map(dest => dest.F_WipStock, src => src.F_WipStock != null ? src.F_WipStock : null)
			.Map(dest => dest.F_TransitStock, src => src.F_TransitStock != null ? src.F_TransitStock : null)
			.Map(dest => dest.F_ShouldSelfMake, src => src.F_ShouldSelfMake != null ? src.F_ShouldSelfMake : null)
			.Map(dest => dest.F_ShouldOutsource, src => src.F_ShouldOutsource != null ? src.F_ShouldOutsource : null)
			.Map(dest => dest.F_ShouldPurchase, src => src.F_ShouldPurchase != null ? src.F_ShouldPurchase : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
