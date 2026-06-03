using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcContractItemGood;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcContractItemGood;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcContractItemGoodCrInput, ACtcContractItemGoodEntity>()
			.Map(dest => dest.F_ProjectItemId, src => src.F_ProjectItemId != null ? src.F_ProjectItemId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_ProdUnit, src => src.F_ProdUnit != null ? src.F_ProdUnit : null)
			.Map(dest => dest.F_StockInProcess, src => src.F_StockInProcess != null ? src.F_StockInProcess : null)
			.Map(dest => dest.F_FeedProcess, src => src.F_FeedProcess != null ? src.F_FeedProcess : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<ACtcContractItemGoodEntity, ACtcContractItemGoodInfoOutput>()
			.Map(dest => dest.F_ProjectItemId, src => src.F_ProjectItemId != null ? src.F_ProjectItemId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_ProdUnit, src => src.F_ProdUnit != null ? src.F_ProdUnit : null)
			.Map(dest => dest.F_StockInProcess, src => src.F_StockInProcess != null ? src.F_StockInProcess : null)
			.Map(dest => dest.F_FeedProcess, src => src.F_FeedProcess != null ? src.F_FeedProcess : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
