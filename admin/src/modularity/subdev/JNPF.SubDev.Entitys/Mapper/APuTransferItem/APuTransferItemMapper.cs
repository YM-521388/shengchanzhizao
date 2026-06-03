using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuTransferItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuTransferItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuTransferItemCrInput, APuTransferItemEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SalesPrice, src => src.F_SalesPrice != null ? src.F_SalesPrice : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		//config.ForType<APuTransferItemEntity, APuTransferItemListOutput>()
		;
		config.ForType<APuTransferItemEntity, APuTransferItemInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SalesPrice, src => src.F_SalesPrice != null ? src.F_SalesPrice : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<APuTransferItemEntity, APuTransferItemDetailOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SalesPrice, src => src.F_SalesPrice != null ? src.F_SalesPrice : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
