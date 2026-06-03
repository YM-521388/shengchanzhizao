using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuCheckItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuCheckItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuCheckItemCrInput, APuCheckItemEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_CheckQtyBefore, src => src.F_CheckQtyBefore != null ? src.F_CheckQtyBefore : null)
			.Map(dest => dest.F_CheckQtyDone, src => src.F_CheckQtyDone != null ? src.F_CheckQtyDone : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		//config.ForType<APuCheckItemEntity, APuCheckItemListOutput>()
		;
		config.ForType<APuCheckItemEntity, APuCheckItemInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_CheckQtyBefore, src => src.F_CheckQtyBefore != null ? src.F_CheckQtyBefore : null)
			.Map(dest => dest.F_CheckQtyDone, src => src.F_CheckQtyDone != null ? src.F_CheckQtyDone : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<APuCheckItemEntity, APuCheckItemDetailOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_CheckQtyBefore, src => src.F_CheckQtyBefore != null ? src.F_CheckQtyBefore : null)
			.Map(dest => dest.F_CheckQtyDone, src => src.F_CheckQtyDone != null ? src.F_CheckQtyDone : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
