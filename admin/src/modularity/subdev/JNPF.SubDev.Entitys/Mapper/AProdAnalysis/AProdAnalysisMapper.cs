using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdAnalysis;
using JNPF.example.Entitys.Dto.AProdAnalysisItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdAnalysis;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdAnalysisCrInput, AProdAnalysisEntity>()
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_ConsiderMainStock, src => src.F_ConsiderMainStock != null ? src.F_ConsiderMainStock : null)
			.Map(dest => dest.F_ConsiderMaterialOccupy, src => src.F_ConsiderMaterialOccupy != null ? src.F_ConsiderMaterialOccupy : null)
			.Map(dest => dest.F_ConsiderWipGoods, src => src.F_ConsiderWipGoods != null ? src.F_ConsiderWipGoods : null)
			.Map(dest => dest.F_OccupyAllowOtherOut, src => src.F_OccupyAllowOtherOut != null ? src.F_OccupyAllowOtherOut : null)
			.Map(dest => dest.F_ConsiderMaterialStock, src => src.F_ConsiderMaterialStock != null ? src.F_ConsiderMaterialStock : null)
			.Map(dest => dest.F_RoundUpQty, src => src.F_RoundUpQty != null ? src.F_RoundUpQty : null)
			.Map(dest => dest.F_ConsiderTransitGoods, src => src.F_ConsiderTransitGoods != null ? src.F_ConsiderTransitGoods : null)
			.Map(dest => dest.F_AnalysisUserId, src => src.F_AnalysisUserId != null ? src.F_AnalysisUserId : null)
			.Map(dest => dest.F_AnalysisTime, src => src.F_AnalysisTime != null ? src.F_AnalysisTime : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdAnalysisEntity, AProdAnalysisInfoOutput>()
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_ConsiderMainStock, src => src.F_ConsiderMainStock != null ? src.F_ConsiderMainStock : null)
			.Map(dest => dest.F_ConsiderMaterialOccupy, src => src.F_ConsiderMaterialOccupy != null ? src.F_ConsiderMaterialOccupy : null)
			.Map(dest => dest.F_ConsiderWipGoods, src => src.F_ConsiderWipGoods != null ? src.F_ConsiderWipGoods : null)
			.Map(dest => dest.F_OccupyAllowOtherOut, src => src.F_OccupyAllowOtherOut != null ? src.F_OccupyAllowOtherOut : null)
			.Map(dest => dest.F_ConsiderMaterialStock, src => src.F_ConsiderMaterialStock != null ? src.F_ConsiderMaterialStock : null)
			.Map(dest => dest.F_RoundUpQty, src => src.F_RoundUpQty != null ? src.F_RoundUpQty : null)
			.Map(dest => dest.F_ConsiderTransitGoods, src => src.F_ConsiderTransitGoods != null ? src.F_ConsiderTransitGoods : null)
			.Map(dest => dest.F_AnalysisUserId, src => src.F_AnalysisUserId != null ? src.F_AnalysisUserId : null)
			.Map(dest => dest.F_AnalysisTime, src => src.F_AnalysisTime != null ? src.F_AnalysisTime : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField4d57ab, src => src.AProdAnalysisItemList.Adapt<List<AProdAnalysisItemInfoOutput>>())
		;
		config.ForType<AProdAnalysisEntity, AProdAnalysisDetailOutput>()
            .Map(dest => dest.tableField4d57ab, src => src.AProdAnalysisItemList.Adapt<List<AProdAnalysisItemDetailOutput>>())
		;
	}
}
