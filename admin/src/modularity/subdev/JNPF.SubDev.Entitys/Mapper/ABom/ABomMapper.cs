using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ABom;
using JNPF.example.Entitys.Dto.ABomGoods;
using Mapster;

namespace JNPF.example.Entitys.Mapper.ABom;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ABomCrInput, ABomEntity>()
			.Map(dest => dest.F_BomCode, src => src.F_BomCode != null ? src.F_BomCode : null)
			.Map(dest => dest.F_BomName, src => src.F_BomName != null ? src.F_BomName : null)
			.Map(dest => dest.F_CategoryId, src => src.F_CategoryId != null && src.F_CategoryId.Count > 0 ? src.F_CategoryId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ABomEntity, ABomInfoOutput>()
			.Map(dest => dest.F_BomCode, src => src.F_BomCode != null ? src.F_BomCode : null)
			.Map(dest => dest.F_BomName, src => src.F_BomName != null ? src.F_BomName : null)
			.Map(dest => dest.F_CategoryId, src => src.F_CategoryId != null ? src.F_CategoryId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldd2a80d, src => src.ABomGoodsList.Adapt<List<ABomGoodsInfoOutput>>())
		;
		config.ForType<ABomEntity, ABomDetailOutput>()
            .Map(dest => dest.tableFieldd2a80d, src => src.ABomGoodsList.Adapt<List<ABomGoodsDetailOutput>>())
		;
	}
}
