using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ABomGoods;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ABomGoods;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ABomGoodsCrInput, ABomGoodsEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Process, src => src.F_Process != null ? src.F_Process : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ABomGoodsEntity, ABomGoodsListOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Process, src => src.F_Process != null ? src.F_Process : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ABomGoodsEntity, ABomGoodsInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Process, src => src.F_Process != null ? src.F_Process : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ABomGoodsEntity, ABomGoodsDetailOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Process, src => src.F_Process != null ? src.F_Process : null)
			.Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
