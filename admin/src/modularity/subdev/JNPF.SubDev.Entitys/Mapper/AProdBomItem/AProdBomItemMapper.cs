using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdBomItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdBomItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdBomItemCrInput, AProdBomItemEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_StockInProcess, src => src.F_StockInProcess != null ? src.F_StockInProcess : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<AProdBomItemEntity, AProdBomItemInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_StockInProcess, src => src.F_StockInProcess != null ? src.F_StockInProcess : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdBomItemEntity, AProdBomItemDetailOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_StockInProcess, src => src.F_StockInProcess != null ? src.F_StockInProcess : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
